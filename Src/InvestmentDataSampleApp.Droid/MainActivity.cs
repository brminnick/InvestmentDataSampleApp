using Android.OS;
using Android.App;
using Android.Content.PM;

using Xamarin;

namespace InvestmentDataSampleApp.Droid
{
	[Activity(Label = "InvestmentDataSampleApp.Droid", Theme = "@style/MyTheme", Icon = "@drawable/icon", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			Insights.Initialize(InsightsConstants.InsightsAPIKey, this);

			Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
			{
				if (isStartupCrash)
					Insights.PurgePendingCrashReports().GetAwaiter().GetResult();
			};

			base.OnCreate(savedInstanceState);

			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			EntryCustomReturn.Forms.Plugin.Android.CustomReturnEntryRenderer.Init();

			LoadApplication(new App());
		}
	}
}

