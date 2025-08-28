using System.Collections.Generic;
using UnityEngine;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInData<TRewardData> where TRewardData : struct
    {
        private HashSet<int> _checkInDays;

        [field: SerializeField] public DailyCheckInData<TRewardData>[] Daily { get; protected set; }

        [field: SerializeField] public bool StreakCheckIn { get; protected set; }

        public void Initialize()
        {
            _checkInDays = new HashSet<int>();

            foreach (DailyCheckInData<TRewardData> daily in Daily)
            {
                _checkInDays.Add(daily.DayIndex);
            }
        }

        public bool HasCheckInDay(int day)
        {
            return _checkInDays.Contains(day);
        }

        public DailyCheckInData<TRewardData> GetDailyCheckInData(int dayIndex)
        {
            for (int i = 0; i < Daily.Length; i++)
            {
                if (Daily[i].DayIndex == dayIndex)
                {
                    return Daily[i];
                }
            }

            return null;
        }

        public void AutoAssignDays()
        {
            for (int i = 0; i < Daily.Length; i++)
            {
                Daily[i].SetDay(i + 1);
            }
        }
    }

    public abstract class DailyCheckInData<TRewardData> where TRewardData : struct
    {
        [field: SerializeField] public int DayIndex { get; private set; }

        [field: SerializeField] public TRewardData[] Rewards { get; protected set; }

        public void SetDay(int day)
        {
            DayIndex = day;
        }
    }
}