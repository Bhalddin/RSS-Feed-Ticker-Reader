using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Feed_Ticker_Reader.FeedData
{
    public class RSSHost
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } = -1;
        [Ignore]
        public List<Feed> Feeds { get; set; }
        public String HostName { get; set; }
        public String FeedUrl { get; set; }
    }
}
