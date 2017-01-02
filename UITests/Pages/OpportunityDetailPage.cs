using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

using InvestmentDataSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
	public class OpportunityDetailPage : BasePage
	{
		#region Constructors 
		public OpportunityDetailPage(IApp app, Platform platform) : base(app,platform)
		{
		}
		#endregion

		#region Properties
		public string Title => GetTitle();
		#endregion

		#region Methods
		public void WaitForPageToAppear()
		{
			app.WaitForElement(PageTitleConstants.OpportunityDetailPageTitle);
		}

		string GetTitle()
		{
			AppResult[] titleQuery;

			if (OniOS)
				titleQuery = app.Query(x => x.Class("UILabel").Marked(PageTitleConstants.OpportunityDetailPageTitle));
			else
				titleQuery = app.Query(x => x.Class("TextView").Marked(PageTitleConstants.OpportunityDetailPageTitle));

			return titleQuery?.FirstOrDefault()?.Text;
		}
		#endregion
	}
}
