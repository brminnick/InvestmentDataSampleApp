#if DEBUG
using System.Linq;
using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public static class UITestBackdoorMethodService
    {
        public static void TriggerPullToRefresh()
        {
            var navigationPage = (NavigationPage)Application.Current.MainPage;
            var opportunitiesPage = (OpportunitiesPage)navigationPage.CurrentPage;

            var relativeLayout = (RelativeLayout)opportunitiesPage.Content;
            var refreshView = relativeLayout.Children.OfType<RefreshView>().First();

            refreshView.IsRefreshing = true;
        }
    }
}
#endif
