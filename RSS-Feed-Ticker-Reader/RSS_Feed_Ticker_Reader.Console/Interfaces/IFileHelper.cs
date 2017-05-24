using RSS_Feed_Ticker_Reader.Database;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RSS_Feed_Ticker_Reader.Console.Interfaces
{
    class FileHelper : IFileHelper
    {
        public string GetLocalFilePath(string filename)
        {
            return Path.Combine(System.Reflection.Assembly.GetExecutingAssembly().Location, filename);
        }
    }
}
