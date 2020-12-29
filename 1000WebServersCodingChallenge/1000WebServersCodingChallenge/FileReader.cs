using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace _1000WebServersCodingChallenge
{
    public class FileReader
    {
        #region Methods
        public static List<string> ReadFile(string filePath)
        {
            List<string> lines = new List<string>();
            try
            {
                lines = System.IO.File.ReadAllLines(filePath)?.ToList();
            }
            catch(Exception ex)
            {
                //todo:add logging message
                Console.WriteLine($"Failed to read the list of servers. {ex.Message}");
            }
            return lines;
        }
        #endregion
    }
}
