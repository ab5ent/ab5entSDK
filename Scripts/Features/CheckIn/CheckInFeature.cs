using ab5entSDK.Feature.Base;
using ab5entSDK.Features.StorableData;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInFeature<TRewardData> : IFeature where TRewardData : struct
    {
        #region Members

        private CheckInData<TRewardData> _data;

        private CheckInUserData _userData;

        #endregion

        #region Properties

        public IFeatureContainer Container { get; private set; }

        #endregion

        #region Methods

        public string GetKey()
        {
            return GetType().Name.ToLower();
        }

        public CheckInFeature(IFeatureContainer container, CheckInData<TRewardData> data)
        {
            Container = container;

            _data = data;
            _data.Initialize();

            _userData = BaseStorableData.CreateInstance<CheckInUserData>();
        }

        public CheckInData<TRewardData> GetCheckedInData()
        {
            return _data;
        }

        public DailyCheckInData<TRewardData> GetDailyCheckInData(int dayIndex)
        {
            return HasCheckInData(dayIndex) ? _data.GetDailyCheckInData(dayIndex) : null;
        }

        private bool HasCheckInData(int dayIndex)
        {
            return _data.HasCheckInDay(dayIndex);
        }

        #region UserData

        public void ResetUserData()
        {
            _userData.ResetData();
        }

        private bool CanCheckIn(int dayIndex)
        {
            return HasCheckInData(dayIndex) && _userData.CanCheckIn(dayIndex);
        }

        public void CheckIn(int dayIndex)
        {
            if (CanCheckIn(dayIndex))
            {
                _userData.CheckIn(dayIndex);
            }
        }

        public bool IsCheckedIn(int dayIndex)
        {
            return HasCheckInData(dayIndex) && _userData.IsCheckedIn(dayIndex);
        }

        private bool CanClaimReward(int dayIndex)
        {
            return HasCheckInData(dayIndex) && _userData.CanClaimedReward(dayIndex);
        }

        public void ClaimReward(int dayIndex)
        {
            if (CanClaimReward(dayIndex))
            {
                _userData.ClaimReward(dayIndex);
            }
        }

        public bool IsClaimedReward(int dayIndex)
        {
            return HasCheckInData(dayIndex) && _userData.IsClaimedReward(dayIndex);
        }

        #endregion

        #endregion
    }
}