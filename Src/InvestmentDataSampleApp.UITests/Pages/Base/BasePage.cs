using Xamarin.UITest;

namespace InvestmentDataSampleApp.UITests
{
    public abstract class BasePage
    {
        protected BasePage(IApp app) => App = app;

        protected IApp App { get; }
    }
}
