using UIKit;
using Foundation;

namespace InvestmentDataSampleApp.iOS
{
    [Register(nameof(AppDelegate))]
	public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
	{
		public override bool FinishedLaunching(UIApplication app, NSDictionary options)
		{
#if ENABLE_TEST_CLOUD
			Xamarin.Calabash.Start();
#endif

			global::Xamarin.Forms.Forms.Init();
			EntryCustomReturn.Forms.Plugin.iOS.CustomReturnEntryRenderer.Init();

			LoadApplication(new App());

			return base.FinishedLaunching(app, options);
		}
	}
}

