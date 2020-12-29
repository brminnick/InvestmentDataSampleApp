using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using AsyncAwaitBestPractices;
using InvestmentDataSampleApp.Shared;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.CommunityToolkit.Markup;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesPage : BaseContentPage<OpportunitiesViewModel>, ISearchPage
    {
        readonly WeakEventManager<string> _searchBarChangedEventManager = new WeakEventManager<string>();
        readonly WelcomeView _welcomeView = new WelcomeView();

        readonly RelativeLayout _mainLayout;

        public OpportunitiesPage()
        {
            SearchBarTextChanged += HandleSearchBarTextChanged;
            ViewModel.OkButtonTapped += HandleOkButtonTapped;

            var refreshView = new RefreshView
            {
                RefreshColor = Color.DarkSlateGray,
                Content = new CollectionView
                {
                    ItemTemplate = new OpportunitiesDataTemplate(),
                    SelectionMode = SelectionMode.Single,
                }.Bind(CollectionView.ItemsSourceProperty, nameof(OpportunitiesViewModel.VisibleOpportunitiesCollection))
            }.Bind(RefreshView.CommandProperty, nameof(OpportunitiesViewModel.RefreshDataCommand))
             .Bind(RefreshView.IsRefreshingProperty, nameof(OpportunitiesViewModel.IsCollectionRefreshing));

            var addButtonToolBar = new ToolbarItem
            {
                IconImageSource = "Add",
                Text = "Add",
                AutomationId = AutomationIdConstants.AddOpportunityButton,
                Order = Device.RuntimePlatform is Device.Android ? ToolbarItemOrder.Secondary : ToolbarItemOrder.Default
            };
            addButtonToolBar.Clicked += HandleAddButtonClicked;
            ToolbarItems.Add(addButtonToolBar);

            _mainLayout = new RelativeLayout();

            _mainLayout.Children.Add(refreshView,
                Constraint.Constant(0),
                Constraint.Constant(0),
                Constraint.RelativeToParent(parent => parent.Width),
                Constraint.RelativeToParent(parent => parent.Height));

            Title = PageTitleConstants.OpportunitiesPage;

            NavigationPage.SetBackButtonTitle(this, "");

            Content = _mainLayout;
        }

        public event EventHandler<string> SearchBarTextChanged
        {
            add => _searchBarChangedEventManager.AddEventHandler(value);
            remove => _searchBarChangedEventManager.RemoveEventHandler(value);
        }

        public void OnSearchBarTextChanged(in string text) =>
            _searchBarChangedEventManager.RaiseEvent(this, text, nameof(SearchBarTextChanged));

        protected override async void OnAppearing()
        {
            base.OnAppearing();

            await TryDisplayWelcomeView();

            if (Content is Layout<View> layout
                && layout.Children.OfType<RefreshView>().First() is RefreshView refreshView
                && refreshView.Content is CollectionView collectionView
                && IsNullOrEmpty(collectionView.ItemsSource))
            {
                refreshView.IsRefreshing = true;
            }

            static bool IsNullOrEmpty(in IEnumerable? enumerable) => !enumerable?.GetEnumerator().MoveNext() ?? true;
        }

        async void HandleAddButtonClicked(object sender, EventArgs e)
        {
            if (Device.RuntimePlatform is Device.iOS)
                await Navigation.PushModalAsync(new AddOpportunityPage());
            else
                await Navigation.PushModalAsync(new BaseNavigationPage(new AddOpportunityPage()));
        }

        async void HandleOkButtonTapped(object sender, EventArgs e) => await _welcomeView.HideView();

        async Task TryDisplayWelcomeView()
        {
            if (VersionTracking.IsFirstLaunchEver && !_mainLayout.Children.Contains(_welcomeView))
            {
                await MainThread.InvokeOnMainThreadAsync(() =>
                {
                    _mainLayout.Children.Add(_welcomeView,
                       Constraint.Constant(0),
                       Constraint.Constant(0));

                    return _welcomeView.ShowView();
                });
            }
        }

        void HandleSearchBarTextChanged(object sender, string text) => ViewModel.FilterTextCommand.Execute(text);
    }
}

