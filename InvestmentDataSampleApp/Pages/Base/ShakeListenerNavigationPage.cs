using System;

using Xamarin.Forms;

using DeviceMotion.Plugin;
using DeviceMotion.Plugin.Abstractions;
using System.Diagnostics;

namespace InvestmentDataSampleApp
{
	public class ShakeListenerNavigationPage : NavigationPage
	{
		#region Constant Fields
		const int _shakeDetectionTimeLapse = 250;
		readonly double _shakeThreshold;
		#endregion

		#region Fields
		bool _hasUpdated;
		DateTime _lastUpdate;
		double _lastX, _lastY, _lastZ;
		#endregion

		#region Constructors
		public ShakeListenerNavigationPage(Page root) : base(root)
		{
			if (Device.OS == TargetPlatform.iOS)
				_shakeThreshold = 20;
			else if (Device.OS == TargetPlatform.Android)
				_shakeThreshold = 800;

			#region Implement ShakeListener
			CrossDeviceMotion.Current.Start(MotionSensorType.Accelerometer, MotionSensorDelay.Default);

			CrossDeviceMotion.Current.SensorValueChanged += HandleSensorValueChanged;
			#endregion
		}
		#endregion

		#region Methods
		void HandleSensorValueChanged(object sender, SensorValueChangedEventArgs e)
		{
			if (e.SensorType == MotionSensorType.Accelerometer)
			{
				double x = ((MotionVector)e.Value).X;
				double y = ((MotionVector)e.Value).Y;
				double z = ((MotionVector)e.Value).Z;

				var currentTime = DateTime.Now;

				if (_hasUpdated == false)
				{
					_hasUpdated = true;
					_lastUpdate = currentTime;
				}
				else
				{
					var hasMinimumTimeElapsed = (currentTime - _lastUpdate).TotalMilliseconds > _shakeDetectionTimeLapse;

					if (!hasMinimumTimeElapsed)
						return;

					_lastUpdate = currentTime;

					var timeSinceLastShakeInMilliseconds = (currentTime - _lastUpdate).TotalMilliseconds;
					var totalMovementDistance = x + y + z - _lastX - _lastY - _lastZ;
					var shakeSpeed = Math.Abs(totalMovementDistance) / timeSinceLastShakeInMilliseconds * 10000;

					Debug.WriteLine($"Shake Speed: {shakeSpeed}");

					if (shakeSpeed > _shakeThreshold)
						HandleShake();
				}

				_lastX = x;
				_lastY = y;
				_lastZ = z;
			}
		}

		void HandleShake()
		{
			Device.BeginInvokeOnMainThread(async () => await DisplayAlert("Shake Detected", "You shook your device!", "Ok"));
		}
		#endregion
	}
}
