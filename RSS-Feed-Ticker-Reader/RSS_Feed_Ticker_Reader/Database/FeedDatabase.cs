using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RSS_Feed_Ticker_Reader.Database
{
    public class FeedDatabase
    {
        static FeedDatabase database;
        private SQLiteAsyncConnection connection;
        public static FeedDatabase Database
        {
            get
            {
                if (database == null)
                {
                    database = new FeedDatabase(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
                }
                return database;
            }
        }
        private FeedDatabase(string dbPath)
        {
            connection = new SQLiteAsyncConnection(dbPath);
            connection.CreateTableAsync<FeedData.RSSHost>().Wait();
        }
        public Task<List<FeedData.RSSHost>> GetRSSAsync()
        {
            return connection.Table<FeedData.RSSHost>().ToListAsync();
        }
        public Task<int> SaveItemAsync(FeedData.RSSHost item)
        {
            if (item.ID != 0)
            {
                return connection.UpdateAsync(item);
            }
            else
            {
                return connection.InsertAsync(item);
            }
        }
    }
}
