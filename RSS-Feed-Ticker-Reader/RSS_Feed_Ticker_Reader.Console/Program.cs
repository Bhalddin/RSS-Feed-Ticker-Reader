using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Feed_Ticker_Reader.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            do
            {
                System.Console.WriteLine("Press 'A' for Add RSS or 'D' for DB");
                switch(System.Console.ReadKey().Key)
                {
                    case ConsoleKey.A:
                        ReadRSS();
                        break;
                    case ConsoleKey.D:
                        ReadDB();
                        break;
                    default:
                        break;
                }
                System.Console.WriteLine("Press Enter to Continue or write EXIT to leave");
            } while (System.Console.ReadLine().ToUpper().Equals("EXIT"));
        }
        static void ReadRSS()
        {
            System.Console.WriteLine("Write RSS RDF or Atom");
            FeedTypes.FeedType fty;
            switch (System.Console.ReadLine().ToUpper())
            {
                case "ATOM":
                    fty = FeedTypes.FeedType.Atom;
                    break;
                case "RDF":
                    fty = FeedTypes.FeedType.RDF;
                    break;
                default:
                case "RSS":
                    fty = FeedTypes.FeedType.RSS;
                    break;
            }

            System.Console.WriteLine("Write feed URL");
            FeedData.RSSHost host = FeedData.FeedConverter.Parse(System.Console.ReadLine(), fty);
            System.Console.Clear();

            System.Console.WriteLine(writeFeeds(host));
            System.Console.Write("Save To DB?");
            if(System.Console.ReadKey().Key == ConsoleKey.Y)
                Database.FeedDatabase.Database.SaveItemAsync(host);
        }
        static void ReadDB()
        {
            int id = 0;
            Task<List<FeedData.RSSHost>> hosts = Database.FeedDatabase.Database.GetRSSAsync();
            hosts.Wait();
            do
            {
                StringBuilder sbui = new StringBuilder("Select Number of Host");
                int i = 1;
                foreach (FeedData.RSSHost host in hosts.Result)
                {
                    sbui.AppendLine(host.HostName + "(" + i + ")");
                }
                System.Console.WriteLine(sbui.ToString());
            } while (Int32.TryParse(System.Console.ReadLine(), out id));
            System.Console.Clear();
            StringBuilder sb = new StringBuilder();
            System.Console.WriteLine(writeFeeds(hosts.Result.ElementAt(id - 1)));
        }
        static string writeFeeds(FeedData.RSSHost feeds)
        {
            StringBuilder sb = new StringBuilder();
            foreach (FeedData.Feed feed in feeds.Feeds)
            {
                sb.AppendLine("\r\n-------------------------\r\n");
                sb.AppendLine("Title: " + feed.Title);
                sb.AppendLine("MSG: " + feed.Content);
                sb.AppendLine("Date: " + feed.PublishDate);
            }
            return sb.ToString();
        }
    }
}
