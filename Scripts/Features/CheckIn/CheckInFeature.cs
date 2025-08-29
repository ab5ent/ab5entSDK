using ab5entSDK.Feature.Base;
using ab5entSDK.Features.StorableData;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInFeature<TRewardData> : BaseFeature where TRewardData : struct
    {
        #region Members

        private CheckInData<TRewardData> _data;

        private CheckInUserData _userData;

        #endregion

        #region Properties

        public IFeatureContainer Container { get; private set; }

        public EFeatureType FeatureType { get; protected set; } = EFeatureType.CheckIn;

        #endregion

        #region Methods

        public void Initialize(IFeatureContainer container, CheckInData<TRewardData> data)
        {
            Container = container;

            _data = data;
            _data.Initialize();

            _userData = BaseStorableData.Load<CheckInUserData>();

            ValidateSessionCheckIn();
        }

        #region Data

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

        private bool IsStreakCheckIn()
        {
            return _data.StreakCheckIn;
        }

        #endregion

        #region UserData

        private void ValidateSessionCheckIn()
        {
            if (!IsStreakCheckIn())
            {
                CheckIn(_userData.LastCheckInDayIndex + 1);
            }
            else
            {
                StreakCheckIn(_userData.LastCheckInDayIndex + 1);
            }
        }

        private void StreakCheckIn(int dayIndex)
        {
            if (CanCheckIn(dayIndex))
            {
                _userData.StreakCheckIn(dayIndex);
            }
        }

        private bool CanCheckIn(int dayIndex)
        {
            return HasCheckInData(dayIndex) && _userData.CanCheckIn(dayIndex);
        }

        private void CheckIn(int dayIndex)
        {
            if (CanCheckIn(dayIndex))
            {
                _userData.CheckIn(dayIndex);
            }
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

        public override void ResetUserData()
        {
            _userData.ResetData();
        }

        #endregion

        #endregion
    }
}