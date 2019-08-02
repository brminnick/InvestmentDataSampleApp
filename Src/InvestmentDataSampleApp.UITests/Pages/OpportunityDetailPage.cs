using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.iOS;
using Xamarin.UITest.Queries;

using InvestmentDataSampleApp.Shared;

using Query = System.Func<Xamarin.UITest.Queries.AppQuery, Xamarin.UITest.Queries.AppQuery>;

namespace InvestmentDataSampleApp.UITests
{
	public class OpportunityDetailPage : BasePage
	{
		#region Constructors 
		public OpportunityDetailPage(IApp app) : base(app)
		{
		}
		#endregion

		#region Properties
		public string Title => GetTitle();
		#endregion

		#region Methods
		public void WaitForPageToAppear()
		{
			App.WaitForElement(PageTitleConstants.OpportunityDetailPage);
		}

		string GetTitle()
		{
			AppResult[] titleQuery;

			if (App is iOSApp)
				titleQuery = App.Query(x => x.Class("UILabel").Marked(PageTitleConstants.OpportunityDetailPage));
			else
				titleQuery = App.Query(x => x.Class("AppCompatTextView").Marked(PageTitleConstants.OpportunityDetailPage));

			return titleQuery?.FirstOrDefault()?.Text;
		}
		#endregion
	}
}
