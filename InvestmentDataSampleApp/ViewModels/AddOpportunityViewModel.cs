using System;
using System.Windows.Input;
using System.Threading.Tasks;

using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
	public class AddOpportunityViewModel : BaseViewModel
	{
		#region Fields
		string _topic, _company, _dba, _owner;
		long _leaseAmount;
		SalesStages _salesStage;
		DateTime _dateCreated;
		ICommand _saveButtonTapped;
		#endregion

		#region Events
		public event EventHandler SaveError;
		public event EventHandler SaveToDatabaseCompleted;
		#endregion

		#region Properties
		public string Topic
		{
			get { return _topic; }
			set
			{
				SetProperty(ref _topic, value);
			}
		}

		public string Company
		{
			get { return _company; }
			set
			{
				SetProperty(ref _company, value);
			}
		}

		public string DBA
		{
			get { return _dba; }
			set
			{
				SetProperty(ref _dba, value);
			}
		}

		public long LeaseAmount
		{
			get { return _leaseAmount; }
			set
			{
				SetProperty(ref _leaseAmount, value);
			}
		}

		public SalesStages SalesStage
		{
			get { return _salesStage; }
			set
			{
				SetProperty(ref _salesStage, value);
			}
		}

		public string Owner
		{
			get { return _owner; }
			set
			{
				SetProperty(ref _owner, value);
			}
		}

		public DateTime DateCreated
		{
			get { return _dateCreated; }
			set
			{
				SetProperty(ref _dateCreated, value);
			}
		}

		public ICommand SaveButtonTapped => _saveButtonTapped ??
			(_saveButtonTapped = new Command(async () => await ExecuteSaveButtonTapped()));
		#endregion

		public AddOpportunityViewModel()
		{
			SalesStage = SalesStages.New;
		}

		async Task ExecuteSaveButtonTapped()
		{
			if (Topic?.Length == 0 || Company?.Length == 0 || Owner?.Length == 0 || DBA?.Length == 0 || LeaseAmount == 0)
			{
				OnSaveError();
				return;
			}

			DateCreated = DateTime.Now;
			await OpportunityModelDatabase.SaveOpportunityAsync(new OpportunityModel
			{
				Topic = Topic,
				Company = Company,
				DBA = DBA,
				LeaseAmount = LeaseAmount,
				SalesStage = SalesStage,
				Owner = Owner,
				DateCreated = DateCreated
			});

			OnSaveToDatabaseCompleted();
		}

		void OnSaveError()
		{
			var handle = SaveError;
			handle?.Invoke(this, EventArgs.Empty);
		}

		void OnSaveToDatabaseCompleted()
		{
			var handle = SaveToDatabaseCompleted;
			handle?.Invoke(this, EventArgs.Empty);
		}
	}
}

