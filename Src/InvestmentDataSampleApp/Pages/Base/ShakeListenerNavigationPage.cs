using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace InvestmentDataSampleApp
{
    public class ShakeListenerNavigationPage : NavigationPage
    {
        #region Constructors
        public ShakeListenerNavigationPage(Page root) : base(root)
        {
            try
            {
                Accelerometer.Start(SensorSpeed.UI);
                Accelerometer.ShakeDetected += HandleShakeDetected;
            }
            catch (FeatureNotSupportedException)
            {
                Debug.WriteLine("Accelerometer Not Supported");
            }
        }
        #endregion

        #region Methods
        void HandleShakeDetected(object sender, EventArgs e) =>
            Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Shake Detected", "You shook your device!", "Ok"));
        #endregion

    }
}
