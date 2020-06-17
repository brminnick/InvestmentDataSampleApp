using System.Linq;
using System.Threading.Tasks;
using AsyncAwaitBestPractices.MVVM;
using FFImageLoading.Forms;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Markup;
using static Xamarin.Forms.Markup.GridRowsColumns;
using static InvestmentDataSampleApp.MarkupExtensions;

namespace InvestmentDataSampleApp
{
    class OpportunitiesDataTemplate : DataTemplate
    {
        readonly static int _rowHeight = Device.Idiom is TargetIdiom.Phone ? 50 : 150;

        public OpportunitiesDataTemplate() : base(LoadTemplate)
        {

        }

        enum Row { Title, Description };
        enum Column { Icon, Topic }

        static SwipeView LoadTemplate() => new ExtendedSwipeView
        {
            RightItems = new SwipeItems(new[]
            {
                new DeleteSwipeItem().Bind(SwipeItem.CommandParameterProperty, nameof(OpportunityModel.Topic)),
            })
            {
                Mode = SwipeMode.Execute
            },

            TappedCommand = new AsyncCommand<OpportunityModel>(ExecuteSwipeViewTappedCommand),

            Margin = new Thickness(0, 5, 0, 15),

            Content = new Grid
            {
                BackgroundColor = Color.White,
                Padding = new Thickness(5, 0, 0, 0),
                ColumnSpacing = 20,

                RowDefinitions = Rows.Define(
                    (Row.Title, Star),
                    (Row.Description, Star)),

                ColumnDefinitions = Columns.Define(
                    (Column.Icon, Absolute(_rowHeight / 3)),
                    (Column.Topic, Star)),

                Children =
                {
                    new CachedImage { Source = "beaconfundingicon", HeightRequest = _rowHeight }
                        .RowSpan(All<Row>()).Column(Column.Icon),

                    new Label { Text = "Topic", FontAttributes = FontAttributes.Bold }.TextStart().TextBottom()
                        .Row(Row.Title).Column(Column.Topic),

                    new Label().TextStart().TextTop()
                        .Row(Row.Description).Column(Column.Topic)
                        .Bind(Label.TextProperty, nameof(OpportunityModel.Topic))
                }
            }
        }.Bind(ExtendedSwipeView.TappedCommandParameterProperty, ".");

        static Task ExecuteSwipeViewTappedCommand(OpportunityModel opportunityModel) =>
            Application.Current.MainPage.Navigation.PushAsync(new OpportunityDetailsPage(opportunityModel));

        class DeleteSwipeItem : SwipeItem
        {
            public DeleteSwipeItem()
            {
                Text = "Delete";
                IconImageSource = "Delete";
                BackgroundColor = Color.Red;
                Command = new AsyncCommand<string>(ExecuteSwipeToDeleteCommand);
            }

            static async Task ExecuteSwipeToDeleteCommand(string topic)
            {
                var opportunityModelToDelete = await OpportunityModelDatabase.GetOpportunityByTopic(topic).ConfigureAwait(false);
                await OpportunityModelDatabase.DeleteItem(opportunityModelToDelete).ConfigureAwait(false);

                await TriggerPullToRefresh().ConfigureAwait(false);
            }

            static Task TriggerPullToRefresh()
            {
                var navigationPage = (NavigationPage)Application.Current.MainPage;
                var opportunityPage = navigationPage.Navigation.NavigationStack.OfType<OpportunitiesPage>().First();

                var opportunityPageLayout = (Layout<View>)opportunityPage.Content;
                var refreshView = opportunityPageLayout.Children.OfType<RefreshView>().First();

                return MainThread.InvokeOnMainThreadAsync(() => refreshView.IsRefreshing = true);
            }
        }
    }
}

