using UIKit;

using Xamarin;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp.iOS
{
	public class Application
	{
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

