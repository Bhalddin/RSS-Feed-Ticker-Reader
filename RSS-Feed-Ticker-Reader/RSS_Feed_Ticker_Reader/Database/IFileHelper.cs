using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Feed_Ticker_Reader.Database
{
    public interface IFileHelper
    {
        string GetLocalFilePath(string filename);
    }
}
