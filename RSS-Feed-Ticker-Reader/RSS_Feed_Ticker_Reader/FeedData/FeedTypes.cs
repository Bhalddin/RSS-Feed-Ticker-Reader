using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Feed_Ticker_Reader
{
    public class FeedTypes
    {
        public enum FeedType
        {
            /// <summary>
            /// Really Simple Syndication format.
            /// </summary>
            RSS,
            /// <summary>
            /// RDF site summary format.
            /// </summary>
            RDF,
            /// <summary>
            /// Atom Syndication format.
            /// </summary>
            Atom
        }
    }
}
