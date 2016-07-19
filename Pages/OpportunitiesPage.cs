using System.Threading.Tasks;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
	public class OpportunitiesPage : ContentPage
	{
		ListView listView;
		OpportunitiesViewModel opportunitiesViewModel;

		public OpportunitiesPage()
		{
			opportunitiesViewModel = new OpportunitiesViewModel();
			BindingContext = opportunitiesViewModel;

			#region Create the ListView
			listView = new ListView()
			{
				ItemTemplate = new DataTemplate(typeof(OpportunitiesViewCell)),
				RowHeight = 75
			};

			listView.IsPullToRefreshEnabled = true;
			listView.Refreshing += (async (sender, e) =>
			{
				await opportunitiesViewModel.RefreshOpportunitiesDataAsync();
				listView.EndRefresh();
			});

			listView.ItemSelected += (sender, e) =>
			{
				Navigation.PushAsync(new CreditBuilderCarouselPage());
			};

			listView.SetBinding(ListView.ItemsSourceProperty, "AllOpportunitiesData");
			#endregion

			Title = $"Opportunities";

			#region Initialize the Toolbar Add Button
			var addButtonToolBar = new ToolbarItem();
			addButtonToolBar.Icon = "Add";
			addButtonToolBar.Clicked += async (sender, e) => await Navigation.PushModalAsync(new NavigationPage(new AddOpportunityPage()));

			ToolbarItems.Add(addButtonToolBar);
			#endregion

			#region Create Searchbar
			var searchBar = new SearchBar();
			searchBar.TextChanged += (sender, e) => opportunitiesViewModel.FilterLocations(searchBar.Text);
			#endregion

			#region Create Stack
			var listSearchStack = new StackLayout
			{
				Padding = 0,
				Spacing = 0,
				Children = {
					searchBar,
					listView
				}
			};
			#endregion

			Content = listSearchStack;
		}

		protected override void OnAppearing()
		{
			base.OnAppearing();
			opportunitiesViewModel.RefreshOpportunitiesDataAsync();
		}
	}
}

