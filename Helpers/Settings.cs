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
		#endregion

		#region Fields
		static readonly bool ShouldShowWelcomeViewDefault = true;
		#endregion

		#region Properties
		public static bool ShouldShowWelcomeView
		{
			get
			{
				return AppSettings.GetValueOrDefault<bool>(ShouldShowWelcomeViewKey, ShouldShowWelcomeViewDefault);
			}
			set
			{
				AppSettings.AddOrUpdateValue<bool>(ShouldShowWelcomeViewKey, value);
			}
		}

		static ISettings AppSettings
		{
			get
			{
				return CrossSettings.Current;
			}
		}
		#endregion
	}
}