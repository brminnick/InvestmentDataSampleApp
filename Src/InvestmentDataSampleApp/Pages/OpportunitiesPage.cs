using System;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesPage : BaseContentPage<OpportunitiesViewModel>
    {
        #region Constant Fields
        readonly RelativeLayout _mainLayout;
        readonly SearchBar _searchBar;
        #endregion

        #region Fields
        ListView _listView;
        WelcomeView _welcomeView;
        #endregion

        #region Constructors
        public OpportunitiesPage()
        {
            ViewModel.OkButtonTapped += HandleWelcomeViewDisappearing;

            #region Create the ListView
            _listView = new ListView
            {
                ItemTemplate = new DataTemplate(typeof(OpportunitiesViewCell)),
                RowHeight = 75,
                IsPullToRefreshEnabled = true
            };
            _listView.ItemSelected += HandleListViewItemSelected;
            _listView.SetBinding(ListView.ItemsSourceProperty, nameof(ViewModel.ViewableOpportunitiesData));
            _listView.SetBinding(ListView.RefreshCommandProperty, nameof(ViewModel.RefreshAllDataCommand));
            _listView.SetBinding(ListView.IsRefreshingProperty, nameof(ViewModel.IsListViewRefreshing));
            #endregion

            #region Initialize the Toolbar Add Button
            var addButtonToolBar = new ToolbarItem
            {
                Icon = "Add",
                AutomationId = AutomationIdConstants.AddOpportunityButton
            };
            addButtonToolBar.Clicked += HandleAddButtonClicked;
            ToolbarItems.Add(addButtonToolBar);
            #endregion

            #region Create Searchbar
            _searchBar = new SearchBar
            {
                AutomationId = AutomationIdConstants.OpportunitySearchBar
            };
            _searchBar.SetBinding(SearchBar.TextProperty, nameof(ViewModel.SearchBarText));
            #endregion

            _mainLayout = new RelativeLayout();

            _mainLayout.Children.Add(_searchBar,
                Constraint.Constant(0),
                Constraint.Constant(0),
                 Constraint.RelativeToParent(parent => parent.Width));
            _mainLayout.Children.Add(_listView,
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => getSearchBarHeight(parent)),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height - getSearchBarHeight(parent)));

            Title = PageTitleConstants.OpportunitiesPageTitle;

            NavigationPage.SetBackButtonTitle(this, "");

            Content = _mainLayout;

            DisplayWelcomeView();

            double getSearchBarHeight(RelativeLayout p) => _searchBar.Measure(p.Width, p.Height).Request.Height;
        }

        #endregion

        #region Methods
        protected override void OnAppearing()
        {
            base.OnAppearing();

            _listView.BeginRefresh();
        }

        void HandleListViewItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                var itemSelected = e?.SelectedItem as OpportunityModel;

                await Navigation?.PushAsync(new OpportunityDetailsPage(itemSelected));

                _listView.SelectedItem = null;
            });
        }

        async void HandleAddButtonClicked(object sender, EventArgs e) =>
            await Navigation?.PushModalAsync(new NavigationPage(new AddOpportunityPage()));

        void HandleWelcomeViewDisappearing(object sender, EventArgs e) => _welcomeView?.HideView();

        void DisplayWelcomeView()
        {
            if (!Settings.ShouldShowWelcomeView)
                return;

            Device.BeginInvokeOnMainThread(() =>
            {
                _welcomeView = new WelcomeView();

                _mainLayout?.Children?.Add(_welcomeView,
                   Constraint.Constant(0),
                   Constraint.Constant(0));

                _welcomeView?.ShowView(true);
            });
        }
        #endregion
    }
}

