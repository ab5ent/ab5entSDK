using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInData<TRewardData> where TRewardData : struct
    {
        private HashSet<int> _checkInDays;

        [field: SerializeField] public DailyCheckInData<TRewardData>[] Daily { get; protected set; }

        [field: SerializeField] public bool StreakCheckIn { get; protected set; }

        public void SetCheckedInDays()
        {
            _checkInDays = new HashSet<int>();

            foreach (DailyCheckInData<TRewardData> daily in Daily)
            {
                _checkInDays.Add(daily.Day);
            }
        }

        public bool HasCheckInDay(int day)
        {
            return _checkInDays.Contains(day);
        }

        public void AutoAssignDays()
        {
            for (int i = 0; i < Daily.Length; i++)
            {
                Daily[i].SetDay(i + 1);
            }
        }
    }

    public class DailyCheckInData<TRewardData> where TRewardData : struct
    {
        [field: SerializeField] public int Day { get; protected set; }

        [field: SerializeField] public TRewardData[] Rewards { get; protected set; }

        public void SetDay(int day)
        {
            Day = day;
        }
    }
}