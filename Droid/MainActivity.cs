
using Android.App;
using Android.Content.PM;
using Android.OS;
using System;
using Xamarin;

namespace InvestmentDataSampleApp.Droid
{
	[Activity(Label = "InvestmentDataSampleApp.Droid", Theme = "@style/MyTheme", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
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
    }
}

