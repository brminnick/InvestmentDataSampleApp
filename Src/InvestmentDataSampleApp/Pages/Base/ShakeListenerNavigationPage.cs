using System;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Essentials;

namespace InvestmentDataSampleApp
{
    public class ShakeListenerNavigationPage : BaseNavigationPage
    {
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

        void HandleShakeDetected(object sender, EventArgs e) =>
            MainThread.BeginInvokeOnMainThread(async () => await DisplayAlert("Shake Detected", "You shook your device!", "Ok"));
    }
}
