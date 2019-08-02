using System;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;

using InvestmentDataSampleApp.Shared;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesPage : BaseContentPage<OpportunitiesViewModel>
    {
        readonly RelativeLayout _mainLayout;

        WelcomeView _welcomeView;

        public OpportunitiesPage()
        {
            ViewModel.OkButtonTapped += HandleWelcomeViewDisappearing;

            var collectionView = new CollectionView
            {
                ItemTemplate = new OpportunitiesDataTemplate(),
                SelectionMode = SelectionMode.Single,
                Margin = new Thickness(20,0,0,0)
            };
            collectionView.SelectionChanged += HandleSelectionChanged;
            collectionView.SetBinding(CollectionView.ItemsSourceProperty, nameof(OpportunitiesViewModel.ViewableOpportunitiesData));

            var addButtonToolBar = new ToolbarItem
            {
                IconImageSource = "Add",
                AutomationId = AutomationIdConstants.AddOpportunityButton
            };
            addButtonToolBar.Clicked += HandleAddButtonClicked;
            ToolbarItems.Add(addButtonToolBar);

            var searchBar = new SearchBar
            {
                AutomationId = AutomationIdConstants.OpportunitySearchBar
            };
            searchBar.SetBinding(SearchBar.TextProperty, nameof(OpportunitiesViewModel.SearchBarText));

            _mainLayout = new RelativeLayout();

            _mainLayout.Children.Add(searchBar,
                Constraint.Constant(0),
                Constraint.Constant(0),
                 Constraint.RelativeToParent(parent => parent.Width));
            _mainLayout.Children.Add(collectionView,
                Constraint.Constant(0),
                Constraint.RelativeToParent(getSearchBarHeight),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height - getSearchBarHeight(parent)));

            Title = PageTitleConstants.OpportunitiesPage;

            NavigationPage.SetBackButtonTitle(this, "");

            Content = _mainLayout;

            DisplayWelcomeView();

            double getSearchBarHeight(RelativeLayout p) => searchBar.Measure(p.Width, p.Height).Request.Height;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            ViewModel.RefreshDataCommand?.Execute(null);
        }

        void HandleSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(async () =>
            {
                if (e.CurrentSelection.FirstOrDefault() is OpportunityModel itemTapped)
                    await Navigation.PushAsync(new OpportunityDetailsPage(itemTapped));

                if (sender is CollectionView collectionView)
                    collectionView.SelectedItem = null;
            });
        }

        async void HandleAddButtonClicked(object sender, EventArgs e) =>
            await Navigation.PushModalAsync(new NavigationPage(new AddOpportunityPage()));

        async void HandleWelcomeViewDisappearing(object sender, EventArgs e)
        {
            if (_welcomeView is null || _mainLayout is null)
                return;

            await _welcomeView.HideView();

            Device.BeginInvokeOnMainThread(() =>
            {
                if (_mainLayout.Children.Contains(_welcomeView))
                    _mainLayout.Children.Remove(_welcomeView);
            });
        }

        Task DisplayWelcomeView()
        {
            if (Settings.ShouldShowWelcomeView && _mainLayout != null)
            {
                _welcomeView = new WelcomeView();

                return Device.InvokeOnMainThreadAsync(() =>
                {
                    _mainLayout.Children.Add(_welcomeView,
                       Constraint.Constant(0),
                       Constraint.Constant(0));

                    return _welcomeView.ShowView();
                });
            }

            return Task.CompletedTask;
        }
    }
}

