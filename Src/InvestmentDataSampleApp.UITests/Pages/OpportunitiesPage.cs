using System;
using System.Linq;
using System.Threading.Tasks;
using InvestmentDataSampleApp.Shared;
using Xamarin.UITest;
using Xamarin.UITest.Android;
using Xamarin.UITest.iOS;
using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
    public class OpportunitiesPage : BasePage
    {
        readonly Query _addOpportunityButton, _opportunitySearchBar, _welcomeViewOkButton,
            _androidContextMenuOverflowButton, _androidSearchBarButton;

        public OpportunitiesPage(IApp app) : base(app)
        {
            _addOpportunityButton = x => x.Marked(AutomationIdConstants.AddOpportunityButton);
            _opportunitySearchBar = x => x.Marked(AutomationIdConstants.OpportunitySearchBar);
            _welcomeViewOkButton = x => x.Marked(AutomationIdConstants.WelcomeViewOkButton);
            _androidContextMenuOverflowButton = x => x.Class("android.support.v7.widget.ActionMenuPresenter$OverflowMenuButton");
            _androidSearchBarButton = x => x.Id("ActionSearch");
        }

        public bool IsWelcomeViewVisible => IsWelcomeViewOnScreen();

        public bool IsRefreshActivityIndicatorDisplayed => App switch
        {
            AndroidApp androidApp => (bool)(androidApp.Query(x => x.Class("ListViewRenderer_SwipeRefreshLayoutWithFixedNestedScrolling")?.Invoke("isRefreshing"))?.FirstOrDefault() ?? false),
            iOSApp iosApp => iosApp.Query(x => x.Class("UIRefreshControl")).Any(),
            _ => throw new NotSupportedException(),
        };

        public async Task WaitForPullToRefreshIndicator(int timeoutInSeconds = 60)
        {
            int counter = 0;
            while (!IsRefreshActivityIndicatorDisplayed && counter < timeoutInSeconds)
            {
                await Task.Delay(1000).ConfigureAwait(false);
                counter++;

                if (counter >= timeoutInSeconds)
                    throw new Exception($"Loading the list took longer than {timeoutInSeconds}s");
            }
        }

        public async Task WaitForNoPullToRefreshIndicator(int timeoutInSeconds = 60)
        {
            int counter = 0;
            while (IsRefreshActivityIndicatorDisplayed && counter < timeoutInSeconds)
            {
                await Task.Delay(1000).ConfigureAwait(false);
                counter++;

                if (counter >= timeoutInSeconds)
                    throw new Exception($"Loading the list took longer than {timeoutInSeconds}s");
            }
        }

        public void TriggerPullToRefresh()
        {
            switch (App)
            {
                case iOSApp iOSApp:
                    iOSApp.Invoke("triggerPullToRefresh:", "");
                    break;

                case AndroidApp androidApp:
                    androidApp.Invoke("TriggerPullToRefresh");
                    break;
            }

            App.Screenshot("Triggered Pull To Refresh");
        }

        public void TapAddOpportunityButton()
        {
            if (App.Query(_androidContextMenuOverflowButton).Any())
            {
                App.Tap(_androidContextMenuOverflowButton);
                App.Screenshot("Android Overflow Button Tapped");
            }

            App.Tap(_addOpportunityButton);

            App.Screenshot("Tapped Add Opportunity Button");
        }

        public void TapOpportunityViewCell(string topic)
        {
            App.ScrollDownTo(topic);
            App.Tap(topic);
            App.Screenshot($"Tapped ${topic} View Cell");
        }

        public void DeleteViewCell(string topic)
        {
            App.ScrollDownTo(topic);

            if (App is iOSApp)
                App.SwipeRightToLeft(topic);
            else
                App.TouchAndHold(topic);

            App.Tap("Delete");
        }

        public void TapWelcomeViewOkButton()
        {
            App.Tap(_welcomeViewOkButton);
            App.Screenshot("Welcome View Ok Button Tapped");
        }

        public void Search(string searchString)
        {
            if (App.Query(_androidSearchBarButton).Any())
            {
                App.Tap(_androidSearchBarButton);
                App.Screenshot("Tapped Android Search Bar Button");
            }

            App.Tap(_opportunitySearchBar);
            App.EnterText(searchString);
            App.DismissKeyboard();
            App.Screenshot($"Entered {searchString} into Search Bar");
        }

        public bool DoesViewCellExist(string topic, int timeoutInSeconds = 10)
        {
            try
            {
                App.ScrollDownTo(topic, timeout: TimeSpan.FromSeconds(timeoutInSeconds));
                return true;
            }
            catch
            {
                try
                {
                    App.ScrollUpTo(topic, timeout: TimeSpan.FromSeconds(timeoutInSeconds));
                    return true;
                }
                catch
                {
                    return false;
                }
            }
        }

        public void WaitForNoWelcomeView() => App.WaitForNoElement(_welcomeViewOkButton);

        bool IsWelcomeViewOnScreen(int timeoutInSeconds = 10)
        {
            try
            {
                App.WaitForElement(_welcomeViewOkButton, timeout: TimeSpan.FromSeconds(timeoutInSeconds));
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
