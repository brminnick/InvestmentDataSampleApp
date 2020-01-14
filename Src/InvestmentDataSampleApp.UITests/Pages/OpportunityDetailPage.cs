using System;
using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Android;

using InvestmentDataSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
	public class OpportunityDetailPage : BasePage
	{
		public OpportunityDetailPage(IApp app) : base(app)
		{
		}

		public string Title => GetTitle();

		public void WaitForPageToAppear()
		{
			App.WaitForElement(PageTitleConstants.OpportunityDetailPage);
		}

		string GetTitle()
		{
            var titleQuery = App switch
            {
                iOSApp iOSApp => iOSApp.Query(x => x.Class("UILabel").Marked(PageTitleConstants.OpportunityDetailPage)),

                AndroidApp androidApp => androidApp.Query(x => x.Class("AppCompatTextView").Marked(PageTitleConstants.OpportunityDetailPage)),

                _ => throw new NotSupportedException(),
            };

            return titleQuery.First().Text;
		}
	}
}
