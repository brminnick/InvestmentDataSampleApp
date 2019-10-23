using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class BaseNavigationPage : NavigationPage
    {
        public BaseNavigationPage(Page root) : base(root)
        {
            BarBackgroundColor = Color.DarkSlateGray;
            BarTextColor = Color.White;
        }
    }
}
