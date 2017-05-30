using SQLite;
using System;

namespace RSS_Feed_Ticker_Reader.FeedData
{
    public class Feed
    {
        [PrimaryKey, AutoIncrement]
        public int ID { get; set; } = -1;
        public int ParentID { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime PublishDate { get; set; }
        public FeedTypes.FeedType FeedType { get; set; }
    }
}

