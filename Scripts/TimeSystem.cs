using System;
using UnityEngine;

namespace ab5entSDK.Core
{
    public class TimeSystem
    {

        #region Members

        private static readonly DateTime Jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        private static float GetRealtimeSinceStartup => Mathf.Max(Time.realtimeSinceStartup, 0);


        private DateTime _syncedTime = DateTime.UtcNow;
        private float _startTimeSinceStartup;

        #endregion

        #region Properties

        public bool IsSyncedTime { get; private set; }

        #endregion

        #region StaticsMethods

        public static DateTime EndTimeOfDay(DateTime dateTime)
        {
            return dateTime.AddDays(1).AddMilliseconds(-1);
        }

        public static DateTime StartTimeOfDay(DateTime dateTime)
        {
            return dateTime.Date;
        }

        public static long DateTimeConverted(DateTime dateTime, ConvertTime convertTime = ConvertTime.InSeconds)
        {
            return convertTime switch
            {
                ConvertTime.InSeconds => (long)(dateTime - Jan1st1970).TotalSeconds,
                ConvertTime.InMiniSeconds => (long)(dateTime - Jan1st1970).TotalMilliseconds,
                ConvertTime.InDays => (long)(dateTime - Jan1st1970).TotalDays,
                _ => 0
            };
        }

        public static TimeSpan GetTimeSpanBetween(DateTime startTime, DateTime endTime)
        {
            return (endTime - startTime);
        }

        public static (float totalDays, float totalHours, float totalMinutes, long totalSeconds) GetTotalTimeParts(TimeSpan timeSpan)
        {
            return ((float)timeSpan.TotalDays, (float)timeSpan.TotalHours, (float)timeSpan.TotalMinutes, (long)timeSpan.Seconds);
        }

        public static (int days, int hours, int minutes, int seconds, int milliseconds) GetTimeParts(TimeSpan timeSpan)
        {
            return (timeSpan.Days, timeSpan.Hours, timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
        }

        public static TimeSpan GetTimeSpanFromSeconds(long totalSeconds)
        {
            return TimeSpan.FromSeconds(totalSeconds);
        }

        public string ConvertSecondsToTime(int totalSeconds)
        {
            TimeSpan timeSpan = GetTimeSpanFromSeconds(totalSeconds);
            (int days, int hours, int minutes, int seconds, int milliseconds) = GetTimeParts(timeSpan);
            return days > 0 ? $"{days}d {hours:00}:{minutes:00}:{seconds:00}" : $"{hours:00}:{minutes:00}:{seconds:00}";
        }

        #endregion

        #region Methods

        public void Setup(DateTime timeServer, bool isSyncSuccess)
        {
            if (IsSyncedTime)
            {
                return;
            }

            if (!isSyncSuccess)
            {
                return;
            }

            IsSyncedTime = true;

            _syncedTime = timeServer;
            _startTimeSinceStartup = GetRealtimeSinceStartup;
        }

        public void AddTimeSync(long seconds)
        {
            _syncedTime = _syncedTime.AddSeconds(seconds);
        }

        public DateTime Now(TimeType timeType = TimeType.Synced)
        {
            return timeType == TimeType.Local ? DateTime.UtcNow : _syncedTime.AddSeconds(GetRealtimeSinceStartup - _startTimeSinceStartup);
        }

        public long NowConverted(TimeType timeType = TimeType.Synced, ConvertTime convertTime = ConvertTime.InSeconds)
        {
            return DateTimeConverted(Now(timeType), convertTime);
        }

        #region TimeOfDay

        #region StartTimeOfDay

        public DateTime StartTimeOfDay(TimeType timeType = TimeType.Synced)
        {
            return StartTimeOfDay(Now(timeType));
        }

        public TimeSpan StartTimeOfDayElapsed(TimeType timeType = TimeType.Synced)
        {
            return GetTimeSpanBetween(Now(timeType), StartTimeOfDay(Now(timeType)));
        }

        #endregion

        #region EndTimeOfDay

        public DateTime EndTimeOfDay(TimeType timeType = TimeType.Synced)
        {
            return EndTimeOfDay(Now(timeType));
        }

        public TimeSpan EndTimeOfDayRemain(TimeType timeType = TimeType.Synced)
        {
            return GetTimeSpanBetween(EndTimeOfDay(timeType), Now(timeType));
        }

        #endregion

        #endregion

        #region TimeOfWeek

        #region StartTimeOfWeek

        public DateTime StartTimeOfWeek(TimeType timeType = TimeType.Synced)
        {
            DateTime now = Now(timeType);
            int daysSinceMonday = ((int)now.DayOfWeek + 6) % 7; // Monday = 0, Sunday = 6
            return now.Date.AddDays(-daysSinceMonday);
        }

        public TimeSpan StartTimeOfWeekElapsed(TimeType timeType = TimeType.Synced)
        {
            return GetTimeSpanBetween(Now(timeType), StartTimeOfWeek(timeType));
        }

        #endregion

        #region EndTimeOfWeek

        public DateTime EndTimeOfWeek(DateTime dateTime)
        {
            int daysUntilNextMonday = ((int)DayOfWeek.Monday - (int)dateTime.DayOfWeek + 7) % 7;

            if (daysUntilNextMonday == 0)
            {
                daysUntilNextMonday = 7; // If today is Monday, next Monday
            }

            return dateTime.Date.AddDays(daysUntilNextMonday).AddMilliseconds(-1);
        }

        public TimeSpan RemainTimeOfWeek(TimeType timeType = TimeType.Synced)
        {
            return GetTimeSpanBetween(EndTimeOfWeek(Now(timeType)), Now(timeType));
        }

        #endregion

        #endregion

        #region TimeOfMonth

        #region StartTimeOfMonth

        public DateTime StartTimeOfMonth(TimeType timeType = TimeType.Synced)
        {
            DateTime now = Now(timeType);
            return new DateTime(now.Year, now.Month, 1);
        }

        public TimeSpan StartTimeOfMonthElapsed(TimeType timeType = TimeType.Synced)
        {
            return GetTimeSpanBetween(Now(timeType), StartTimeOfMonth(timeType));
        }

        #endregion

        #region EndTimeOfMonth

        public DateTime EndTimeOfMonth(TimeType timeType = TimeType.Synced)
        {
            DateTime now = Now(timeType);
            DateTime firstDayNextMonth = new DateTime(now.Year, now.Month, 1).AddMonths(1);
            return firstDayNextMonth.AddMilliseconds(-1);
        }

        public TimeSpan RemainTimeOfMonth(TimeType timeType = TimeType.Synced)
        {
            return GetTimeSpanBetween(EndTimeOfMonth(timeType), Now(timeType));
        }

        #endregion

        #endregion

        #endregion

        #region Enums

        public enum TimeType
        {
            Local,
            Synced
        }

        public enum ConvertTime
        {
            InSeconds,
            InMiniSeconds,
            InDays
        }

        #endregion

    }

}