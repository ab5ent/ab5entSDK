using System.Collections.Generic;
using ab5entSDK.Core;
using ab5entSDK.Features.StorableData;
using UnityEngine;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInUserData : BaseStorableData
    {

        #region Members

        [field: SerializeField] private int _highestStreakDayCheckIn;

        [field: SerializeField] private int _lastCheckInDayIndex;

        [field: SerializeField] private List<int> _checkedInDays;

        [field: SerializeField] private List<int> _claimedRewardsDays;

        [field: SerializeField] private long _lastCheckInTimeInSeconds;

        #endregion

        #region Properties

        public int LastCheckInDayIndex
        {
            get => _lastCheckInDayIndex;
            set
            {
                _lastCheckInDayIndex = value;
                DataChanged();
            }
        }

        public List<int> CheckedInDays
        {
            get => _checkedInDays;
            set
            {
                _checkedInDays = value;
                DataChanged();
            }
        }

        public List<int> ClaimedRewardsDays
        {
            get => _claimedRewardsDays;
            set
            {
                _claimedRewardsDays = value;
                DataChanged();
            }
        }

        #endregion

        #region Methods

        #region Checkin

        public void CheckIn(int dayIndex)
        {
            _checkedInDays.Add(dayIndex);
            _lastCheckInDayIndex = dayIndex;
            _lastCheckInTimeInSeconds = TimeSystem.NowConverted();

            DataChanged();
        }

        public void StreakCheckIn(int dayIndex)
        {
            if (HasADayChangedSinceLastCheckIn())
            {
                _lastCheckInDayIndex = dayIndex;
                _highestStreakDayCheckIn = Mathf.Max(_highestStreakDayCheckIn, dayIndex);
                _checkedInDays.Add(dayIndex);
            }
            else
            {
                _lastCheckInDayIndex = -1;
                _highestStreakDayCheckIn = -1;
                _checkedInDays.Clear();
            }

            _lastCheckInTimeInSeconds = TimeSystem.NowConverted();

            DataChanged();
        }

        public bool CanCheckIn(int dayIndex)
        {
            return !IsCheckedIn(dayIndex) && HasDayChangedSinceLastCheckIn();
        }

        private bool IsCheckedIn(int dayIndex)
        {
            return CheckedInDays.Contains(dayIndex);
        }

        private bool HasADayChangedSinceLastCheckIn()
        {
            return TimeSystem.HasADayChangedSince(_lastCheckInTimeInSeconds);
        }

        private bool HasDayChangedSinceLastCheckIn()
        {
            return TimeSystem.HasDayChangedSince(_lastCheckInTimeInSeconds);
        }

        #endregion

        #region ClaimReward

        public bool CanClaimedReward(int dayIndex)
        {
            return IsCheckedIn(dayIndex) && !IsClaimedReward(dayIndex);
        }

        public void ClaimReward(int dayIndex)
        {
            _claimedRewardsDays.Add(dayIndex);
            DataChanged();
        }

        public bool IsClaimedReward(int dayIndex)
        {
            return ClaimedRewardsDays.Contains(dayIndex);
        }

        public override void ResetData()
        {
            _highestStreakDayCheckIn = -1;
            _lastCheckInTimeInSeconds = 0;
            _lastCheckInDayIndex = -1;

            _checkedInDays.Clear();
            _claimedRewardsDays.Clear();

            base.ResetData();
        }

        #endregion

        #endregion

    }
}