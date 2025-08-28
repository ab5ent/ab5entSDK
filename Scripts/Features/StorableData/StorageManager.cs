using System;
using System.Collections.Generic;
using ab5entSDK.Core;
using Unity.VisualScripting;
using UnityEngine;

#if UNITASK_SUPPORT
using Cysharp.Threading.Tasks;
#else
using System.Threading.Tasks;

#endif

namespace ab5entSDK.Features.StorableData
{
    public class StorageManager : IStorageManager
    {
        #region Constants

        private const float AddSaveRequestInterval = 0.02f;

        private const float FlushToDiskInterval = 4;

        #endregion

        #region Fields

        #region Readonly Fields

        private readonly IStorageProvider _provider;
        private readonly IJsonConvert _jsonConvert;
        private readonly IEncryption _encryption;

        private readonly Dictionary<string, IStorableData> _rawRequests, _tempRawRequests;
        private readonly Queue<SaveRequest> _pendingSaveRequests;

#if UNITASK_SUPPORT
        private readonly List<UniTask> _pendingSaveRequestTasks;
#else
        private readonly List<Task> _pendingSaveRequestTasks;
#endif

        #endregion

        private float _lastTryAddSaveRequestTime, _lastTryFlushToDisk;
        private bool _isAddingSaveRequest, _isFlushingToDisk;

        #endregion

        public StorageManager(IStorageProvider provider, IJsonConvert jsonConvert, IEncryption encryption)
        {
            _provider = provider;
            _encryption = encryption;
            _jsonConvert = jsonConvert;

#if UNITASK_SUPPORT
            _pendingSaveRequestTasks = new List<UniTask>();
#else
            _pendingSaveRequestTasks = new List<Task>();
#endif

            _pendingSaveRequests = new Queue<SaveRequest>();
            _rawRequests = new Dictionary<string, IStorableData>();
            _tempRawRequests = new Dictionary<string, IStorableData>();

            _lastTryAddSaveRequestTime = 0;
            _lastTryFlushToDisk = -FlushToDiskInterval;
        }

        #region Methods

        #region Save

        public void CreateRawRequest(IStorableData data, bool forceFlush = false)
        {
            if (data == null)
            {
                Debug.LogWarning("Attempted to save null object");
                return;
            }

            _rawRequests[data.Key] = data;

            if (forceFlush)
            {
                FlushToDisk(true);
            }
        }

        #region SaveRequest

        public void TryToAddSaveRequest()
        {
            if (_isFlushingToDisk)
            {
                return;
            }

            if (_isAddingSaveRequest || !(Time.realtimeSinceStartup - _lastTryAddSaveRequestTime >= AddSaveRequestInterval))
            {
                return;
            }

            if (_rawRequests.Count <= 0)
            {
                return;
            }

            _lastTryAddSaveRequestTime = Time.realtimeSinceStartup;
            AddSaveRequest();
        }

        private void AddSaveRequest()
        {
            _isAddingSaveRequest = true;

            var enumerator = _rawRequests.GetEnumerator();

            if (!enumerator.MoveNext())
            {
                return;
            }

            var (key, snapshotData) = enumerator.Current;

            try
            {
#if UNITASK_SUPPORT
                UniTask task = UniTask.RunOnThreadPool(() => ConvertToSaveRequest(snapshotData));
#else
                Task task = Task.Run(() => ConvertToSaveRequest(snapshotData));
#endif
                lock (_pendingSaveRequests)
                {
                    _pendingSaveRequestTasks.Add(task);
                }
            }
            catch (Exception e)
            {
                Debug.LogError($"[AutoSave] Error saving {key}: {e.Message}");
            }
            finally
            {
                if (_rawRequests.TryGetValue(key, out var currentData))
                {
                    if (ReferenceEquals(currentData, snapshotData))
                    {
                        _rawRequests.Remove(key);
                    }
                    else
                    {
                        _rawRequests.Remove(key);
                        _rawRequests[key] = currentData;
                    }
                }
            }
        }

        private void ConvertToSaveRequest(IStorableData data)
        {
            try
            {
                string json = _jsonConvert.ConvertToJson(data);

                if (string.IsNullOrEmpty(json))
                {
                    Debug.LogWarning($"Failed to serialize object with key: {data.Key}");
                }

                lock (_pendingSaveRequests)
                {
                    var newSaveRequest = new SaveRequest()
                    {
                        Key = data.Key,
                        EncryptedData = _encryption.Encrypt(json),
                        RequestTime = TimeSystem.Now(),
                    };

                    _pendingSaveRequests.Enqueue(newSaveRequest);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to process save request for key '{data.Key}': {ex.Message}");
            }
            finally
            {
                _isAddingSaveRequest = false;
            }
        }

        #endregion

        #region FlushToDisk

        private void WaitToConvertToSaveRequests()
        {
#if UNITASK_SUPPORT
            UniTask[] tasksToWait;
#else
            Task[] tasksToWait;
#endif
            lock (_pendingSaveRequestTasks)
            {
                tasksToWait = _pendingSaveRequestTasks.ToArray();
                _pendingSaveRequestTasks.Clear();
            }

            try
            {
#if UNITASK_SUPPORT
                UniTask.WhenAll(tasksToWait).AsTask().Wait(100);
#else
                Task.WaitAll(tasksToWait, 100);
#endif
            }
            catch (Exception e)
            {
                Debug.LogWarning($"WaitToCompletePendingSaveRequest error: {e.Message}");
            }
        }

        private void ProcessSaveRequests()
        {
            lock (_pendingSaveRequests)
            {
                while (_pendingSaveRequests.Count > 0)
                {
                    SaveRequest saveRequest = _pendingSaveRequests.Dequeue();
                    _provider.Set(saveRequest.Key, saveRequest.EncryptedData);
                }
            }
        }

        public void TryToFlushToDisk()
        {
            if (Time.realtimeSinceStartup - _lastTryFlushToDisk >= FlushToDiskInterval)
            {
                return;
            }

            if (_isFlushingToDisk)
            {
                return;
            }

            if (_pendingSaveRequests.Count < 0)
            {
                return;
            }

            FlushToDisk();
        }

        public void FlushToDisk(bool forceFlush = false)
        {
            try
            {
                _isFlushingToDisk = true;
                float startFlushTime = Time.realtimeSinceStartup;

                if (forceFlush)
                {
                    WaitToConvertToSaveRequests();
                }

                lock (_rawRequests)
                {
                    // for not look _rawRequests
                    _tempRawRequests.AddRange(_rawRequests);
                    _rawRequests.Clear();
                }

                foreach (var kvp in _tempRawRequests)
                {
                    try
                    {
                        string json = _jsonConvert.ConvertToJson(kvp.Value);

                        if (string.IsNullOrEmpty(json))
                        {
                            Debug.LogWarning($"Can't convert json {kvp.Key}");
                            continue;
                        }

                        var saveRequest = new SaveRequest()
                        {
                            Key = kvp.Key,
                            EncryptedData = _encryption.Encrypt(json),
                            RequestTime = TimeSystem.Now(),
                        };

                        lock (_pendingSaveRequests)
                        {
                            _pendingSaveRequests.Enqueue(saveRequest);
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to process raw data for key '{kvp.Key}': {ex.Message}");
                    }
                }

                if (forceFlush)
                {
                    WaitToConvertToSaveRequests();
                    ProcessSaveRequests();
                }

                _provider.FlushToDisk();

                #region Log

                var endFlushTime = Time.realtimeSinceStartup;
                var flushDurationMs = (endFlushTime - startFlushTime) * 1000f;

                Debug.Log($"[StorageManager] Flushed {_rawRequests.Count} objects in {flushDurationMs:F2} ms");

                #endregion
            }
            catch (Exception e)
            {
                Debug.LogError($"FlushToDisk error: {e.Message}");
                throw;
            }
            finally
            {
                _isFlushingToDisk = false;
                _lastTryFlushToDisk = Time.realtimeSinceStartup;
            }
        }

        #endregion

        #endregion

        #region Load

        public T Load<T>(string key) where T : class
        {
            try
            {
                string encrypted = _provider.Get(key);

                if (string.IsNullOrEmpty(encrypted))
                {
                    return null;
                }

                string json = _encryption.Decrypt(encrypted);

                if (string.IsNullOrEmpty(json))
                {
                    return null;
                }

                return _jsonConvert.ConvertToObject<T>(json);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load key '{key}': {ex.Message}");
                return null;
            }
        }

        public bool TryLoad<T>(string key, out T value)
        {
            value = default;

            try
            {
                string encrypted = _provider.Get(key);

                if (string.IsNullOrEmpty(encrypted))
                {
                    return false;
                }

                string json = _encryption.Decrypt(encrypted);

                if (string.IsNullOrEmpty(json))
                {
                    return false;
                }

                value = _jsonConvert.ConvertToObject<T>(json);
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Failed to load key '{key}': {ex.Message}");
                return false;
            }
        }

        #endregion

        #endregion

        #region Classes And Structs

        private struct SaveRequest
        {
            public string Key;

            public string EncryptedData;

            public DateTime RequestTime;
        }

        #endregion
    }
}