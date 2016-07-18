
using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using Xamarin;

namespace InvestmentDataSampleApp.Droid
{
	[Activity(Label = "InvestmentDataSampleApp.Droid", Theme = "@style/MyTheme", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity, Android.Hardware.ISensorEventListener
    {
        bool _hasUpdated;
        DateTime _lastUpdate;
        float _lastX;
        float _lastY;
        float _lastZ;

        const int ShakeDetectionTimeLapse = 250;
        const double ShakeThreshold = 800;

        protected override void OnCreate(Bundle bundle)
		{
 			base.OnCreate(bundle);

			Insights.Initialize(InsightsConstants.InsightsAPIKey, this);

			global::Xamarin.Forms.Forms.Init(this, bundle);


			LoadApplication(new App());
		}

        #region Android.Hardware.ISensorEventListener implementation

        public void OnAccuracyChanged(Android.Hardware.Sensor sensor, Android.Hardware.SensorStatus accuracy)
        {
        }

        public void OnSensorChanged(Android.Hardware.SensorEvent e)
        {
            if (e.Sensor.Type == Android.Hardware.SensorType.Accelerometer)
            {
                float x = e.Values[0];
                float y = e.Values[1];
                float z = e.Values[2];

                var curTime = DateTime.Now;
                if (_hasUpdated == false)
                {
                    _hasUpdated = true;
                    _lastUpdate = curTime;
                    _lastX = x;
                    _lastY = y;
                    _lastZ = z;
                }
                else
                {
                    if ((curTime - _lastUpdate).TotalMilliseconds > ShakeDetectionTimeLapse)
                    {
                        var diffTime = (float)(curTime - _lastUpdate).TotalMilliseconds;
                        _lastUpdate = curTime;
                        var total = x + y + z - _lastX - _lastY - _lastZ;
                        var speed = Math.Abs(total) / diffTime * 10000;

                        if (speed > ShakeThreshold)
                        {
                            ShakeListenerNavigationPageRenderer.HandleDeviceShake();
                        }

                        _lastX = x;
                        _lastY = y;
                        _lastZ = z;
                    }
                }
            }
        }
        #endregion
    }
}

