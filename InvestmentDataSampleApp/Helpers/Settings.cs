// Helpers/Settings.cs
using Plugin.Settings;
using Plugin.Settings.Abstractions;

namespace InvestmentDataSampleApp
{
    /// <summary>
    /// This is the Settings static class that can be used in your Core solution or in any
    /// of your client applications. All settings are laid out the same exact way with getters
    /// and setters. 
    /// </summary>
    public static class Settings
    {
        #region Constant Feilds
        const string ShouldShowWelcomeViewKey = "ShouldShowWelcomeViewKey";
        static readonly bool ShouldShowWelcomeViewDefault = true;
        #endregion

        #region Properties
        public static bool ShouldShowWelcomeView
        {
            get => AppSettings.GetValueOrDefault(ShouldShowWelcomeViewKey, ShouldShowWelcomeViewDefault);
            set => AppSettings.AddOrUpdateValue(ShouldShowWelcomeViewKey, value);
        }

        static ISettings AppSettings => CrossSettings.Current;
        #endregion
    }
}