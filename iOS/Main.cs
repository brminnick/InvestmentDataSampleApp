using UIKit;

using Xamarin;

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

