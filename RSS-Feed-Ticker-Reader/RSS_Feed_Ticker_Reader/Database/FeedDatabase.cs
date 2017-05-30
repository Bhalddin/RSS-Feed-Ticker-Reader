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
                connection.CreateTableAsync<FeedData.Feed>().Wait();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }
        }
        public async Task<List<FeedData.RSSHost>> GetRSSAsync()
        {
            List<FeedData.RSSHost> resultH = await connection.Table<FeedData.RSSHost>().ToListAsync();
            List<FeedData.Feed> resultF = await connection.Table<FeedData.Feed>().ToListAsync();
            foreach(FeedData.RSSHost host in resultH)
            {
                host.Feeds.AddRange( resultF.FindAll(res => res.ParentID == host.ID) );
            }
            return resultH;

        }
        public async Task<int> SaveHostAsync(FeedData.RSSHost item)
        {
            int result = -1;
            if (item.ID != -1)
            {
                result =  await connection.UpdateAsync(item);
            }
            else
            {
                result = await connection.InsertAsync(item);
            }
            FeedData.RSSHost host = (await GetRSSAsync()).Find(h => h.FeedUrl.Equals(item.FeedUrl));
            foreach(FeedData.Feed feed in item.Feeds)
            {
                feed.ParentID = host.ID;
                await SaveFeedAsync(feed);
            }
            return result;
        }
        public Task<int> SaveFeedAsync(FeedData.Feed item)
        {
            if (item.ID != -1)
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
