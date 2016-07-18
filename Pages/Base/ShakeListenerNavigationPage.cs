using System;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class ShakeListenerNavigationPage : NavigationPage
    {
        public ShakeListenerNavigationPage(Page root) : base(root) { }

        public async void HandleShake(object sender, EventArgs e)
        {
            await DisplayAlert("Shake Detected", "You shook your device!", "Ok");
        }
    }
}
