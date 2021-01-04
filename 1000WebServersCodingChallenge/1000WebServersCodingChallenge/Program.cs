using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace _1000WebServersCodingChallenge
{
    class Program
    {
        #region
        public const string filePath = ""; //todo: make this constant in app setting and read from there
        //todo: make a const file for success/error/info message
        #endregion
        static async Task Main(string[] args)
        {
            Console.WriteLine("Reading the list of 1000 servers!");
            List<string> servers = FileReader.ReadFile(filePath);
            await CreateHTTPRequests(servers);
        }

        private static async Task CreateHTTPRequests(List<string> urls)
        {
            try
            {
                var client = new HttpClient();
                //todo: add a solution for the following:
                //Also consider that having a list of servers, like this one, does not guarantee a given
                // server will be online when you attempt to reach it
                //Start requests for all of them
                var requests = urls.Select( url => client.GetAsync(url)).ToList();

                //Wait for all the requests to finish
                await Task.WhenAll(requests);

                //Get the responses
                IEnumerable<HttpResponseMessage> responses = requests.Select( task => task.Result);

                foreach (var r in responses)
                {
                    // Extract the message body
                    var s = await r.Content.ReadAsStringAsync();
                    Console.WriteLine(s);
                }
            }
            catch (Exception ex)
            {
                //todo: log error message using Nlog here
            }
        }
    }
}

