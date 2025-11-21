using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Maui.ApplicationModel;

namespace Criteria.Services
{
    public static class OrientationService
    {
        public static void LockPortrait()
        {
#if ANDROID
            if (Platform.CurrentActivity != null)
            {
                Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Portrait;
            }
#endif
        }

        public static void UnlockOrientation()
        {
#if ANDROID
            if (Platform.CurrentActivity != null)
            {
                Platform.CurrentActivity.RequestedOrientation = Android.Content.PM.ScreenOrientation.Unspecified;
            }
#endif
        }
    }

}
