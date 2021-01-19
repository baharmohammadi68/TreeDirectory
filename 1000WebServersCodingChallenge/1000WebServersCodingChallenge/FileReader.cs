using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;
using Newtonsoft.Json.Linq;
using System.IO;
using Newtonsoft.Json;

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

        //remove this method later:
        public static void ReadResponseFile()
        {
            string resFile = File.ReadAllText(@"C:\Users\bahar\source\repos\1000WebServersCodingChallenge\1000WebServersCodingChallenge\Responses.json");
            
            var parsed = Newtonsoft.Json.JsonConvert.DeserializeObject<List<ServerResponse>>(resFile);
            //todo:aggregate by success rate and version
        }
        #endregion
    }
}
