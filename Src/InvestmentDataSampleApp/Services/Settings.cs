using Xamarin.Essentials;

namespace InvestmentDataSampleApp
{
    public static class Settings
    {
        public static bool ShouldShowWelcomeView
        {
            get => Preferences.Get(nameof(ShouldShowWelcomeView), true);
            set => Preferences.Set(nameof(ShouldShowWelcomeView), value);
        }
    }
}