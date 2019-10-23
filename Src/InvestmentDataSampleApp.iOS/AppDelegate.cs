using UIKit;
using Foundation;

namespace InvestmentDataSampleApp.iOS
{
    [Register(nameof(AppDelegate))]
    public class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        public override bool FinishedLaunching(UIApplication uiApplication, NSDictionary launchOptions)
        {
#if DEBUG
            Xamarin.Calabash.Start();
#endif
            global::Xamarin.Forms.Forms.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init();
            FFImageLoading.Forms.Platform.CachedImageRenderer.InitImageSourceHandler();

            LoadApplication(new App());

            return base.FinishedLaunching(uiApplication, launchOptions);
        }

        #region Backdoor Methods
#if DEBUG
        [Preserve, Export("triggerPullToRefresh:")]
        public void TriggerPullToRefresh(NSString noValue) => UITestBackdoorMethodService.TriggerPullToRefresh();
#endif
#endregion

    }
}

