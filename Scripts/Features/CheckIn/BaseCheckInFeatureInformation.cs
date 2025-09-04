using ab5entSDK.Core;
using ab5entSDK.Features.Core;
using UnityEngine;

namespace ab5entSDK.Features.CheckIn
{
    public class BaseCheckInFeatureInformation : FeatureInformation
    {
        [field: SerializeField]
        public CheckInData Data { get; protected set; }

        [ContextMenu("Auto Assign Day")]
        private void AutoAssignDay()
        {
            Data.AutoAssignDays();
        }
    }
}