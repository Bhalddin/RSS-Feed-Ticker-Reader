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
                return DatabaseGet(DependencyService.Get<IFileHelper>().GetLocalFilePath("TodoSQLite.db3"));
            }
        }
        public static FeedDatabase DatabaseGet(string path)
        {
            if (database == null)
            {
                new Page1();
                database = new FeedDatabase(path);
            }
            return database;
        }
        private FeedDatabase(string dbPath)
        {
            try
            {
                connection = new SQLiteAsyncConnection(dbPath);
                connection.CreateTableAsync<FeedData.RSSHost>().Wait();
            }catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
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
