using System.Collections.Generic;
using ab5entSDK.Core;
using ab5entSDK.Features.StorableData;
using UnityEngine;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInUserData : BaseStorableData
    {
        #region Members

        [field: SerializeField] private List<int> _checkedInDays;

        [field: SerializeField] private List<int> _claimedRewardsDays;

        [field: SerializeField] private long _lastCheckInTimeInSeconds;

        #endregion

        #region Properties

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

        public void CheckIn(int dayIndex)
        {
            if (dayIndex < 0 || CanCheckIn(dayIndex))
            {
                return;
            }

            _checkedInDays.Add(dayIndex);
            _lastCheckInTimeInSeconds = TimeSystem.NowConverted();

            DataChanged();
        }

        public bool CanCheckIn(int dayIndex)
        {
            return !CheckedInDays.Contains(dayIndex) && TimeSystem.HasDayChangedSince(_lastCheckInTimeInSeconds);
        }

        public bool IsCheckedIn(int dayIndex)
        {
            return CheckedInDays.Contains(dayIndex);
        }

        #endregion

        public bool CanClaimedReward(int dayIndex)
        {
            return (CanCheckIn(dayIndex) || IsCheckedIn(dayIndex)) && !IsClaimedReward(dayIndex);
        }

        public void ClaimReward(int dayIndex)
        {
            if (dayIndex < 0)
            {
                return;
            }

            if (!IsCheckedIn(dayIndex))
            {
                _checkedInDays.Add(dayIndex);
                _claimedRewardsDays.Add(dayIndex);
                _lastCheckInTimeInSeconds = TimeSystem.NowConverted();
            }
            else
            {
                _claimedRewardsDays.Add(dayIndex);
            }

            DataChanged();
        }

        public bool IsClaimedReward(int dayIndex)
        {
            return ClaimedRewardsDays.Contains(dayIndex);
        }

        public override void ResetData()
        {
            _lastCheckInTimeInSeconds = 0;

            _checkedInDays.Clear();
            _claimedRewardsDays.Clear();

            base.ResetData();
        }
    }
}