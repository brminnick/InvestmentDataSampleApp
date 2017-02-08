using SQLite;

namespace InvestmentDataSampleApp
{
	public interface ISQLite
	{
		SQLiteAsyncConnection GetConnection();
	}
}

