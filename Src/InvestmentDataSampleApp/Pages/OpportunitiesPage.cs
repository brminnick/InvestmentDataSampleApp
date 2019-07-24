using System;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;
using System.Threading.Tasks;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesPage : BaseContentPage<OpportunitiesViewModel>
    {
        #region Constant Fields
        readonly RelativeLayout _mainLayout;
        readonly ListView _listView;
        #endregion

        #region Fields
        WelcomeView _welcomeView;
        #endregion

        #region Constructors
        public OpportunitiesPage()
        {
            ViewModel.OkButtonTapped += HandleWelcomeViewDisappearing;

            #region Create the ListView
            _listView = new ListView(ListViewCachingStrategy.RecycleElement)
            {
                ItemTemplate = new DataTemplate(typeof(OpportunitiesViewCell)),
                RowHeight = 75,
                IsPullToRefreshEnabled = true
            };
            _listView.ItemTapped += HandleListViewItemTapped;
            _listView.SetBinding(ListView.ItemsSourceProperty, nameof(OpportunitiesViewModel.ViewableOpportunitiesData));
            _listView.SetBinding(ListView.RefreshCommandProperty, nameof(OpportunitiesViewModel.RefreshAllDataCommand));
            _listView.SetBinding(ListView.IsRefreshingProperty, nameof(OpportunitiesViewModel.IsListViewRefreshing));
            #endregion

            #region Initialize the Toolbar Add Button
            var addButtonToolBar = new ToolbarItem
            {
                IconImageSource = "Add",
                AutomationId = AutomationIdConstants.AddOpportunityButton
            };
            addButtonToolBar.Clicked += HandleAddButtonClicked;
            ToolbarItems.Add(addButtonToolBar);
            #endregion

            #region Create Searchbar
            var searchBar = new SearchBar
            {
                AutomationId = AutomationIdConstants.OpportunitySearchBar
            };
            searchBar.SetBinding(SearchBar.TextProperty, nameof(OpportunitiesViewModel.SearchBarText));
            #endregion

            _mainLayout = new RelativeLayout();

            _mainLayout.Children.Add(searchBar,
                Constraint.Constant(0),
                Constraint.Constant(0),
                 Constraint.RelativeToParent(parent => parent.Width));
            _mainLayout.Children.Add(_listView,
                Constraint.Constant(0),
                Constraint.RelativeToParent(getSearchBarHeight),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height - getSearchBarHeight(parent)));

            Title = PageTitleConstants.OpportunitiesPageTitle;

            NavigationPage.SetBackButtonTitle(this, "");

            Content = _mainLayout;

            DisplayWelcomeView();

            double getSearchBarHeight(RelativeLayout p) => searchBar.Measure(p.Width, p.Height).Request.Height;
        }

        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _listView.BeginRefresh();
        }

        void HandleListViewItemTapped(object sender, ItemTappedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (e.Item is OpportunityModel itemTapped)
                    await Navigation.PushAsync(new OpportunityDetailsPage(itemTapped));

                _listView.SelectedItem = null;
            });
        }

        async void HandleAddButtonClicked(object sender, EventArgs e) =>
            await Navigation.PushModalAsync(new NavigationPage(new AddOpportunityPage()));

        async void HandleWelcomeViewDisappearing(object sender, EventArgs e)
        {
            await (_welcomeView?.HideView() ?? Task.CompletedTask);

            Device.BeginInvokeOnMainThread(() =>
            {
                if (_mainLayout.Children.Contains(_welcomeView))
                    _mainLayout.Children.Remove(_welcomeView);
            });
        }

        Task DisplayWelcomeView()
        {
            if (Settings.ShouldShowWelcomeView)
            {
                _welcomeView = new WelcomeView();

                return Device.InvokeOnMainThreadAsync(() =>
                {
                    _mainLayout?.Children?.Add(_welcomeView,
                       Constraint.Constant(0),
                       Constraint.Constant(0));

                    return _welcomeView.ShowView() ?? Task.CompletedTask;
                });
            }

            return Task.CompletedTask;
        }
        #endregion
    }
}

