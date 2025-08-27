using System.Collections.Generic;
using ab5entSDK.Scripts.Features;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInUserData : UserData
    {

        #region Members

        private bool _isCheckedInToday;

        private List<int> _checkedInDays;

        private long _lastCheckInTimeInSeconds;

        #endregion

        #region Properties

        public bool IsCheckedInToday
        {
            get => _isCheckedInToday;
            set
            {
                _isCheckedInToday = value;
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

        #endregion

        #region Methods

        public CheckInUserData(string key) : base(key)
        {

        }

        public void CheckIn(int day)
        {
            if (day < 0 || _checkedInDays.Contains(day))
            {
                return;
            }

            _checkedInDays.Add(day);
            _isCheckedInToday = true;

            DataChanged();
        }

        #endregion

    }
}