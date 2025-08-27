using ab5entSDK.Feature.Base;
using Unity.VisualScripting;

namespace ab5entSDK.Features.CheckIn
{
    public class CheckInFeature<TRewardData> : IFeature where TRewardData : struct
    {

        #region Members

        private CheckInData<TRewardData> _data;

        private CheckInUserData _userData;

        #endregion

        #region Properties

        public IFeatureContainer Container { get; set; }

        #endregion

        #region Methods

        public void SetContainer(IFeatureContainer container)
        {
            Container = container;
        }

        public string GetKey()
        {
            return GetType().Name.ToLower();
        }

        public CheckInFeature(IFeatureContainer container, CheckInData<TRewardData> data)
        {
            SetContainer(container);

            _data = data;
            _data.SetCheckedInDays();

            _userData = new CheckInUserData(Container.Key);
        }

        public CheckInData<TRewardData> GetCheckedInData()
        {
            return _data;
        }

        public void CheckIn(int day)
        {
            bool hasCheckInDay = _data.HasCheckInDay(day);

            if (hasCheckInDay || _userData.IsCheckedInToday)
            {
                _userData.CheckIn(day);
            }
        }

        public bool HasCheckInDay(int day)
        {
            return _data.HasCheckInDay(day);
        }

        #endregion

    }
}