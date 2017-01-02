using System;

using InvestmentDataSampleApp.Shared;
using Xamarin.Forms;

namespace InvestmentDataSampleApp
{
	public class TermsViewModel : BaseViewModel
	{
		#region Fields
		double _pickerValue;

		string _term1Data;
		string _term2Data;
		string _term3Data;
		string _term4Data;
		string _term5Data;
		string _term6Data;
		string _term7Data;
		string _term8Data;
		string _term9Data;
		string _term10Data;
		string _term11Data;
		string _term12Data;
		string _term13Data;
		string _term14Data;
		string _term15Data;
		string _term16Data;
		string _term17Data;
		string _term18Data;
		string _term19Data;
		string _term20Data;

		Command _updateTermDataCommand;
		#endregion

		#region Properties
		public double PickerValue
		{
			get { return _pickerValue; }
			set { SetProperty(ref _pickerValue, value, UpdateTermData); }
		}

		public string Term1Data
		{
			get { return _term1Data; }
			set { SetProperty(ref _term1Data, value); }
		}

		public string Term2Data
		{
			get { return _term2Data; }
			set { SetProperty(ref _term2Data, value); }
		}

		public string Term3Data
		{
			get { return _term3Data; }
			set { SetProperty(ref _term3Data, value); }
		}

		public string Term4Data
		{
			get { return _term4Data; }
			set { SetProperty(ref _term4Data, value); }
		}

		public string Term5Data
		{
			get { return _term5Data; }
			set { SetProperty(ref _term5Data, value); }
		}

		public string Term6Data
		{
			get { return _term6Data; }
			set { SetProperty(ref _term6Data, value); }
		}

		public string Term7Data
		{
			get { return _term7Data; }
			set { SetProperty(ref _term7Data, value); }
		}

		public string Term8Data
		{
			get { return _term8Data; }
			set { SetProperty(ref _term8Data, value); }
		}

		public string Term9Data
		{
			get { return _term9Data; }
			set { SetProperty(ref _term9Data, value); }
		}

		public string Term10Data
		{
			get { return _term10Data; }
			set { SetProperty(ref _term10Data, value); }
		}

		public string Term11Data
		{
			get { return _term11Data; }
			set { SetProperty(ref _term11Data, value); }
		}

		public string Term12Data
		{
			get { return _term12Data; }
			set { SetProperty(ref _term12Data, value); }
		}

		public string Term13Data
		{
			get { return _term13Data; }
			set { SetProperty(ref _term13Data, value); }
		}

		public string Term14Data
		{
			get { return _term14Data; }
			set { SetProperty(ref _term14Data, value); }
		}

		public string Term15Data
		{
			get { return _term15Data; }
			set { SetProperty(ref _term15Data, value); }
		}

		public string Term16Data
		{
			get { return _term16Data; }
			set { SetProperty(ref _term16Data, value); }
		}

		public string Term17Data
		{
			get { return _term17Data; }
			set { SetProperty(ref _term17Data, value); }
		}

		public string Term18Data
		{
			get { return _term18Data; }
			set { SetProperty(ref _term18Data, value); }
		}

		public string Term19Data
		{
			get { return _term19Data; }
			set { SetProperty(ref _term19Data, value); }
		}

		public string Term20Data
		{
			get { return _term20Data; }
			set { SetProperty(ref _term20Data, value); }
		}

		public Command UpdateTermDataCommand => _updateTermDataCommand ??
			(_updateTermDataCommand = new Command(ExecuteUpdateTermDataCommand));
		#endregion

		#region Methods
		public void UpdateTermData()
		{
			var rnd = new Random();

			Term1Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term2Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term3Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term4Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term5Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term6Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term7Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term8Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term9Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term10Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term11Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term12Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term13Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term14Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term15Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term16Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term17Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term18Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term19Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
			Term20Data = LoremIpsumConstants.LoremIpsum.Substring(rnd.Next(100), 10);
		}

		void ExecuteUpdateTermDataCommand(object obj)
		{
			UpdateTermData();
		}
		#endregion
	}
}

