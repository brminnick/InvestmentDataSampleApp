using System;
using System.Windows.Input;
using System.Threading.Tasks;

using AsyncAwaitBestPractices.MVVM;
using AsyncAwaitBestPractices;

namespace InvestmentDataSampleApp
{
    public class AddOpportunityViewModel : BaseViewModel
    {
        #region Constant Fields
        readonly WeakEventManager _saveErrorEventManager = new WeakEventManager();
        readonly WeakEventManager _saveToDatabaseCompleted = new WeakEventManager();
        #endregion

        #region Fields
        string _topic, _company, _dba, _owner;
        long _leaseAmount;
        SalesStages _salesStage;
        DateTimeOffset _dateCreated;
        ICommand _saveButtonTapped;
        #endregion

        #region Events
        public event EventHandler SaveError
        {
            add => _saveErrorEventManager.AddEventHandler(value);
            remove => _saveErrorEventManager.RemoveEventHandler(value);
        }

        public event EventHandler SaveToDatabaseCompleted
        {
            add => _saveToDatabaseCompleted.AddEventHandler(value);
            remove => _saveToDatabaseCompleted.RemoveEventHandler(value);
        }
        #endregion

        #region Properties
        public ICommand SaveButtonTapped => _saveButtonTapped ??
            (_saveButtonTapped = new AsyncCommand(ExecuteSaveButtonTapped, continueOnCapturedContext: false));

        public string Topic
        {
            get => _topic;
            set => SetProperty(ref _topic, value);
        }

        public string Company
        {
            get => _company;
            set => SetProperty(ref _company, value);
        }

        public string DBA
        {
            get => _dba;
            set => SetProperty(ref _dba, value);
        }

        public long LeaseAmount
        {
            get => _leaseAmount;
            set => SetProperty(ref _leaseAmount, value);
        }

        public SalesStages SalesStage
        {
            get => _salesStage;
            set => SetProperty(ref _salesStage, value);
        }

        public string Owner
        {
            get => _owner;
            set => SetProperty(ref _owner, value);
        }

        public DateTimeOffset DateCreated
        {
            get => _dateCreated;
            set => SetProperty(ref _dateCreated, value);
        }
        #endregion

        public AddOpportunityViewModel() => SalesStage = SalesStages.New;

        async Task ExecuteSaveButtonTapped()
        {
            if (Topic?.Length == 0 ||
                Company?.Length == 0 ||
                Owner?.Length == 0 ||
                DBA?.Length == 0 ||
                LeaseAmount == 0)
            {
                OnSaveError();
                return;
            }

            DateCreated = DateTimeOffset.UtcNow;
            await OpportunityModelDatabase.SaveOpportunityAsync(new OpportunityModel
            {
                Topic = Topic,
                Company = Company,
                DBA = DBA,
                LeaseAmount = LeaseAmount,
                SalesStage = SalesStage,
                Owner = Owner,
                CreatedAt = DateCreated
            }).ConfigureAwait(false);

            OnSaveToDatabaseCompleted();
        }

        void OnSaveError() =>
            _saveErrorEventManager?.HandleEvent(this, EventArgs.Empty, nameof(SaveError));

        void OnSaveToDatabaseCompleted() =>
            _saveToDatabaseCompleted?.HandleEvent(this, EventArgs.Empty, nameof(SaveToDatabaseCompleted));
    }
}

