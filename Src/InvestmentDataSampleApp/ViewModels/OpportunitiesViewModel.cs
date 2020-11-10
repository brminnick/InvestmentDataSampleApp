using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using AsyncAwaitBestPractices.MVVM;
using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
    public class OpportunitiesViewModel : BaseViewModel
    {
        readonly AsyncAwaitBestPractices.WeakEventManager _okButtonTappedEventManager = new();

        bool _isCollectionRefreshing;
        IReadOnlyList<OpportunityModel> _allOpportunitiesList = Enumerable.Empty<OpportunityModel>().ToList();
        ICommand? _refreshDataCommand, _okButtonTappedCommand, _filterTextCommand;

        public OpportunitiesViewModel()
        {
            //https://codetraveler.io/2019/09/11/using-observablecollection-in-a-multi-threaded-xamarin-forms-application/
            BindingBase.EnableCollectionSynchronization(VisibleOpportunitiesCollection, null, ObservableCollectionCallback);
        }

        public event EventHandler OkButtonTapped
        {
            add => _okButtonTappedEventManager.AddEventHandler(value);
            remove => _okButtonTappedEventManager.RemoveEventHandler(value);
        }

        public ICommand OkButtonTappedCommand => _okButtonTappedCommand ??= new Command(OnOkButtonTapped);
        public ICommand FilterTextCommand => _filterTextCommand ??= new Command<string>(FilterList);
        public ICommand RefreshDataCommand => _refreshDataCommand ??= new AsyncCommand(ExecuteRefreshDataCommand);

        public ObservableCollection<OpportunityModel> VisibleOpportunitiesCollection { get; } = new ObservableCollection<OpportunityModel>();

        public bool IsCollectionRefreshing
        {
            get => _isCollectionRefreshing;
            set => SetProperty(ref _isCollectionRefreshing, value);
        }

        void FilterList(string filter)
        {
            if (string.IsNullOrWhiteSpace(filter))
            {
                VisibleOpportunitiesCollection.Clear();

                foreach (var opportunity in _allOpportunitiesList)
                    VisibleOpportunitiesCollection.Add(opportunity);
            }
            else
            {
                var upperCaseFilter = filter.ToUpper();

                var filteredOpportunitiesList = _allOpportunitiesList.Where(x => x != null &&
                   (x.Company.ToUpper().Contains(upperCaseFilter) ||
                    x.CreatedAt.ToString().ToUpper().Contains(upperCaseFilter) ||
                    x.DBA.ToUpper().Contains(upperCaseFilter) ||
                    x.LeaseAmountAsCurrency.ToUpper().Contains(upperCaseFilter) ||
                    x.Owner.ToUpper().Contains(upperCaseFilter) ||
                    x.SalesStage.ToString().ToUpper().Contains(upperCaseFilter) ||
                    x.Topic.ToUpper().Contains(upperCaseFilter)));

                VisibleOpportunitiesCollection.Clear();

                foreach (var opportunity in filteredOpportunitiesList)
                    VisibleOpportunitiesCollection.Add(opportunity);
            }
        }

        async Task InitializeDataInDatabaseAsync(int numberOfOpportunityModelsToGenerate = 20)
        {
            for (int i = 0; i < numberOfOpportunityModelsToGenerate; i++)
            {

                var rnd = new Random((int)DateTime.Now.Ticks);
                var companyIndex = rnd.Next(50);
                var dbaIndex = rnd.Next(50);
                var leaseAmount = rnd.Next(1000000);
                var ownerIndex = rnd.Next(50);
                var dayIndex = rnd.Next(1, 28);
                var monthIndex = rnd.Next(1, 12);
                var yearIndex = rnd.Next(2000, 2015);
                var salesStage = (SalesStages)rnd.Next(2);

                var newOpportunity = new OpportunityModel
                {
                    Topic = $"{i + 715003} / Investment Data Corp",
                    Company = $"{LoremIpsumConstants.LoremIpsum.Substring(companyIndex, 10)}",
                    DBA = $"{LoremIpsumConstants.LoremIpsum.Substring(dbaIndex, 10)}",
                    LeaseAmount = leaseAmount,
                    SalesStage = salesStage,
                    Owner = $"{LoremIpsumConstants.LoremIpsum.Substring(ownerIndex, 10)}",
                    CreatedAt = new DateTimeOffset(yearIndex, monthIndex, dayIndex, 0, 0, 0, default)
                };

                await OpportunityModelDatabase.SaveOpportunity(newOpportunity).ConfigureAwait(false);
            }
        }

        async Task ExecuteRefreshDataCommand()
        {
            VisibleOpportunitiesCollection.Clear();

            try
            {
                var opportunityModelsFromDatabase = await OpportunityModelDatabase.GetAllOpportunityData_OldestToNewest().ConfigureAwait(false);

                // If the database is empty, initialize the database with dummy data
                if (!opportunityModelsFromDatabase.Any())
                {
                    await InitializeDataInDatabaseAsync().ConfigureAwait(false);
                    opportunityModelsFromDatabase = await OpportunityModelDatabase.GetAllOpportunityData_OldestToNewest().ConfigureAwait(false);
                }

                _allOpportunitiesList = opportunityModelsFromDatabase.ToList();

                foreach (var opportunity in _allOpportunitiesList)
                    VisibleOpportunitiesCollection.Add(opportunity);
            }
            finally
            {
                IsCollectionRefreshing = false;
            }
        }

        //https://codetraveler.io/2019/09/11/using-observablecollection-in-a-multi-threaded-xamarin-forms-application/
        void ObservableCollectionCallback(IEnumerable collection, object context, Action accessMethod, bool writeAccess)
        {
            lock (collection)
            {
                accessMethod?.Invoke();
            }
        }

        void OnOkButtonTapped() =>
             _okButtonTappedEventManager.RaiseEvent(this, EventArgs.Empty, nameof(OkButtonTapped));
    }
}

