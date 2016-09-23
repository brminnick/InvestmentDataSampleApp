using System.Linq;
using System.Collections.Generic;

using SQLite;

using Xamarin.Forms;
using System.Threading.Tasks;

namespace InvestmentDataSampleApp
{
	public class OpportunityModelDatabase
	{
		static object locker = new object();

		readonly SQLiteConnection database;

		public OpportunityModelDatabase()
		{
			database = DependencyService.Get<ISQLite>().GetConnection();
			// create the tables
			database.CreateTable<OpportunityModel>();
		}

		public async Task<IEnumerable<OpportunityModel>> GetAllOpportunityData_OldestToNewest_Filter(string filter)
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					var tempList = (from i in database.Table<OpportunityModel>() select i).ToList();
					return tempList.Where(x => x.ID > 0 &&
							  (x.Company.ToLower().Contains(filter.ToLower())) ||
							x.DateCreated.ToString().ToLower().Contains(filter.ToLower()) ||
							x.DBA.ToLower().Contains(filter.ToLower()) ||
							x.LeaseAmountAsCurrency.ToLower().Contains(filter.ToLower()) ||
							x.Owner.ToLower().Contains(filter.ToLower()) ||
							x.SalesStage.ToString().ToLower().Contains(filter.ToLower()) ||
							  x.Topic.ToLower().Contains(filter.ToLower()));
				}
			});
		}

		public async Task<IEnumerable<OpportunityModel>> GetAllOpportunityData_OldestToNewest()
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					return (from i in database.Table<OpportunityModel>()
							select i).ToList().Where(x => x.ID > 0);
				}
			});
		}

		public async Task<IEnumerable<OpportunityModel>> GetAllOpportunityData_NewestToOldest()
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					List<OpportunityModel> tempList = (from i in database.Table<OpportunityModel>()
													   select i).ToList();
					return tempList.OrderByDescending(x => x.ID).Where(x => x.ID > 0);
				}
			});
		}

		public async Task<OpportunityModel> GetOpportunityByID(int id)
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					return database.Table<OpportunityModel>().FirstOrDefault(x => x.ID == id);
				}
			});
		}

		public async Task<OpportunityModel> GetOpportunityByTopic(string topic)
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					return database.Table<OpportunityModel>().FirstOrDefault(x => x.Topic == topic);
				}
			});
		}

		public async Task<int> SaveOpportunity(OpportunityModel opportunity)
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					if (GetOpportunityByTopic(opportunity.Topic) != null)
					{
						database.Update(opportunity);
						return opportunity.ID;
					}
					return database.Insert(opportunity);
				}
			});
		}

		public async Task<int> DeleteItem(int id)
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					return database.Delete<OpportunityModel>(id);
				}
			});
		}

		public async Task<OpportunityModel> GetNewestOpportunity()
		{
			return await Task.Run(() =>
			{
				lock (locker)
				{
					return database.Table<OpportunityModel>().OrderByDescending(x => x.ID).Take(1).First();
				}
			});
		}

		public async Task<int> GetNumberOfRows()
		{
			return await Task.Run(() =>
			{
				return database.Table<OpportunityModel>().Count();
			});
		}
	}
}

