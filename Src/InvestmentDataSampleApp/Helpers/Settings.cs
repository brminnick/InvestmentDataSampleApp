using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace InvestmentDataSampleApp
{
    public static class Settings
    {
        #region Properties
        public static bool ShouldShowWelcomeView
        {
            get => AppSettings.GetValueOrDefault(nameof(ShouldShowWelcomeView), true);
            set => AppSettings.AddOrUpdateValue(nameof(ShouldShowWelcomeView), value);
        }

        static ISettings AppSettings => CrossSettings.Current;
        #endregion
    }
}