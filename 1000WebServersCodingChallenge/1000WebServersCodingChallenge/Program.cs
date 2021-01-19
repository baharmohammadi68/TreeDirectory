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
    class Program
    {
        #region
        public const string filePath = ""; //todo: make this constant in app setting and read from there
        //todo: make a const file for success/error/info message
        #endregion
        static async Task Main(string[] args)
        {
            Console.WriteLine("Reading the list of 1000 servers!");
            //List<string> servers = FileReader.ReadFile(filePath);
            //await CreateHTTPRequests(servers);
            FileReader.ReadResponseFile();
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
                var requests = urls.Select(url => client.GetAsync(url)).ToList();

                //Wait for all the requests to finish
                await Task.WhenAll(requests);

                //Get the responses
                IEnumerable<HttpResponseMessage> responses = requests.Select(task => task.Result);

                foreach (var r in responses)
                {
                    // Extract the message body
                    string response = await r.Content.ReadAsStringAsync();
                    ServerResponse parsed = Newtonsoft.Json.JsonConvert.DeserializeObject<ServerResponse>(response);
                }
            }
            catch (Exception ex)
            {
                //todo: log error message using Nlog here
            }
        }



        /*
         public static Task<(T[] Results, Exception[] Exceptions)> WhenAllEx<T>(
    params Task<T>[] tasks)
{
    tasks = tasks.ToArray(); // Defensive copy
    return Task.WhenAll(tasks).ContinueWith(t => // return a continuation of WhenAll
    {
        var results = tasks
            .Where(t => t.Status == TaskStatus.RanToCompletion)
            .Select(t => t.Result)
            .ToArray();
        var aggregateExceptions = tasks
            .Where(t => t.IsFaulted)
            .Select(t => t.Exception) // The Exception is of type AggregateException
            .ToArray();
        var exceptions = new AggregateException(aggregateExceptions).Flatten()
            .InnerExceptions.ToArray(); // Flatten the hierarchy of AggregateExceptions
        if (exceptions.Length == 0 && t.IsCanceled)
        {
            // No exceptions and at least one task was canceled
            exceptions = new[] { new TaskCanceledException(t) };
        }
        return (results, exceptions);
    }, default, TaskContinuationOptions.ExecuteSynchronously, TaskScheduler.Default);
}*/
    }
}

