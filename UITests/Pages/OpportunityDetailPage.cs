using System.Linq;

using Xamarin.UITest;
using Xamarin.UITest.Queries;

using InvestmentDataSampleApp.Constants;

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
			App.WaitForElement(PageTitleConstants.OpportunityDetailPageTitle);
		}

		string GetTitle()
		{
			AppResult[] titleQuery;

			if (OniOS)
				titleQuery = App.Query(x => x.Class("UILabel").Marked(PageTitleConstants.OpportunityDetailPageTitle));
			else
				titleQuery = App.Query(x => x.Class("AppCompatTextView").Marked(PageTitleConstants.OpportunityDetailPageTitle));

			return titleQuery?.FirstOrDefault()?.Text;
		}
		#endregion
	}
}
