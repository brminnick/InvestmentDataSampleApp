using UIKit;

using Xamarin;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp.iOS
{
	public class Application
	{
		// This is the main entry point of the application.
		static void Main(string[] args)
		{

			Insights.Initialize(InsightsConstants.InsightsAPIKey);

			Insights.HasPendingCrashReport += (sender, isStartupCrash) =>
			{
				if (isStartupCrash)
				{
					Insights.PurgePendingCrashReports().Wait();
				}
			};
			
 			UIApplication.Main(args, null, "AppDelegate");
		}
	}
}

