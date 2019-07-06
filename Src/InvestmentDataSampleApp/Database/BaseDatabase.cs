using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using SQLite;
using Xamarin.Essentials;

namespace InvestmentDataSampleApp
{
    public abstract class BaseDatabase
    {
        #region Constant Fields
        public static readonly string DatabasePath = Path.Combine(FileSystem.AppDataDirectory, $"{nameof(InvestmentDataSampleApp)}.db3");

        static readonly Lazy<SQLiteAsyncConnection> _databaseConnectionHolder =
            new Lazy<SQLiteAsyncConnection>(() => new SQLiteAsyncConnection(DatabasePath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache));
        #endregion

        #region Properties
        static SQLiteAsyncConnection DatabaseConnection => _databaseConnectionHolder.Value;
        #endregion

        #region Methods
        protected static async ValueTask<SQLiteAsyncConnection> GetDatabaseConnectionAsync()
        {
            if (!DatabaseConnection.TableMappings.Any())
                await DatabaseConnection.CreateTablesAsync(CreateFlags.None, typeof(OpportunityModel)).ConfigureAwait(false);

            return DatabaseConnection;
        }
        #endregion
    }
}
