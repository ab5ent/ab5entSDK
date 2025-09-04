using System;
using System.Collections.Generic;
using ab5entSDK.Core;
using UnityEngine;

namespace ab5entSDK.Features.CheckIn
{
    [Serializable]
    public class CheckInData
    {
        private HashSet<int> _checkInDays;

        [field: SerializeField] public DailyCheckInData[] Daily { get; protected set; }

        [field: SerializeField] public bool StreakCheckIn { get; protected set; }

        public void Initialize()
        {
            _checkInDays = new HashSet<int>();

            foreach (DailyCheckInData daily in Daily)
            {
                _checkInDays.Add(daily.DayIndex);
            }
        }

        public bool HasCheckInDay(int day)
        {
            return _checkInDays.Contains(day);
        }

        public DailyCheckInData GetDailyCheckInData(int dayIndex)
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

    [Serializable]
    public class DailyCheckInData
    {
        [field: SerializeField] public string DayKey { get; protected set; }

        [field: SerializeField] public int DayIndex { get; private set; }

        [field: SerializeField] public Reward[] Rewards { get; protected set; }

        public void SetDay(int day)
        {
            DayIndex = day;
            DayKey = $"Day_{day}";
        }
    }
}