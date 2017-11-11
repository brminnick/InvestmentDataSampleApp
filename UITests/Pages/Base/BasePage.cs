using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Android;

namespace InvestmentDataSampleApp.UITests
{
	public abstract class BasePage
	{
		protected BasePage(IApp app)
		{
			App = app;
            OnAndroid = app is AndroidApp;
            OniOS = app is iOSApp;
		}

        protected IApp App { get; }
        protected bool OnAndroid { get; }
        protected bool OniOS { get; }
	}
}
