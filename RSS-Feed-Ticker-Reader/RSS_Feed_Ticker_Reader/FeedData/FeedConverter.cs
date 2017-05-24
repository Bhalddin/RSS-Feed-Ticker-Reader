using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace RSS_Feed_Ticker_Reader.FeedData
{
    public class FeedConverter
    {
        public static RSSHost Parse(string url, FeedTypes.FeedType feedType)
        {
            RSSHost host = new RSSHost
            {
                FeedUrl = url
            };
            XDocument doc = XDocument.Load(url);
            switch (feedType)
            {
                case FeedTypes.FeedType.RSS:
                    host.Feeds = ParseRss(doc);
                    host.HostName = doc.Root.Descendants()
                           .First(i => i.Name.LocalName == "channel")
                           .Elements().
                           Where(i => i.Name.LocalName == "title").FirstOrDefault().Value;
                    break;
                case FeedTypes.FeedType.RDF:
                    host.Feeds = ParseRdf(doc);
                    host.HostName = doc.Root.Descendants()
                           .First(i => i.Name.LocalName == "Feed" || i.Name.LocalName == "channel")
                           .Elements().
                           Where(i => i.Name.LocalName == "title").FirstOrDefault().Value;
                    break;
                case FeedTypes.FeedType.Atom:
                    host.Feeds = ParseAtom(doc);
                    host.HostName = doc.Root.Elements().Where(i => i.Name.LocalName == "title").FirstOrDefault().Value;
                    break;
                default:
                    throw new NotSupportedException(string.Format("{0} is not supported", feedType.ToString()));
            }
            return host;
        }
        private static List<Feed> ParseAtom(XDocument doc)
        {
            try
            {
                IEnumerable<Feed> entries = from Feed in doc.Root.Elements().Where(i => i.Name.LocalName == "entry")
                              select new Feed
                              {
                                  FeedType = FeedTypes.FeedType.Atom,
                                  Content = Feed.Elements().First(i => i.Name.LocalName == "content").Value,
                                  Link = Feed.Elements().First(i => i.Name.LocalName == "link").Attribute("href").Value,
                                  PublishDate = DateTime.Parse(Feed.Elements().First(i => i.Name.LocalName == "published").Value),
                                  Title = Feed.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList();
            }
            catch
            {
                return new List<Feed>();
            }
        }

        private static List<Feed> ParseRss(XDocument doc)
        {
            try
            {
                IEnumerable<Feed> entries = from Feed in doc.Root.Descendants()
                                            .First(i => i.Name.LocalName == "channel")
                                            .Elements()
                                            .Where(i => i.HasElements 
                                                && (i.FirstNode as XElement).NodeType == System.Xml.XmlNodeType.Element)
                              select new Feed
                              {
                                  FeedType = FeedTypes.FeedType.RSS,
                                  Content = Feed.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Link = Feed.Elements().First(i => i.Name.LocalName == "link").Value,
                                  PublishDate = DateTime.Parse(Feed.Elements().First(i => i.Name.LocalName == "pubDate").Value),
                                  Title = Feed.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList();
            }
            catch (Exception ex)
            {
                string stri = ex.ToString();
                return new List<Feed>();
            }
        }

        private static List<Feed> ParseRdf(XDocument doc)
        {
            try
            {
                IEnumerable<Feed> entries = from Feed in doc.Root.Descendants().Where(i => (i.Name.LocalName != "Feed"
                                            && i.Name.LocalName != "channel")
                                            && i.HasElements)
                              select new Feed
                              {
                                  FeedType = FeedTypes.FeedType.RDF,
                                  Content = Feed.Elements().First(i => i.Name.LocalName == "description").Value,
                                  Link = Feed.Elements().First(i => i.Name.LocalName == "link").Value,
                                  PublishDate = Feed.Elements().Any(i => i.Name.LocalName == "date")?DateTime.Parse(Feed.Elements().First(i => i.Name.LocalName == "date").Value):DateTime.Now,
                                  Title = Feed.Elements().First(i => i.Name.LocalName == "title").Value
                              };
                return entries.ToList();
            }
            catch
            {
                return new List<Feed>();
            }
        }

        private DateTime ParseDate(string date)
        {
            DateTime result;
            if (DateTime.TryParse(date, out result))
                return result;
            else
                return DateTime.MinValue;
        }
    }
}
