using Xamarin.Essentials;

namespace InvestmentDataSampleApp
{
    public static class Settings
    {
        #region Properties
        public static bool ShouldShowWelcomeView
        {
            get => Preferences.Get(nameof(ShouldShowWelcomeView), true);
            set => Preferences.Set(nameof(ShouldShowWelcomeView), value);
        }
        #endregion
    }
}