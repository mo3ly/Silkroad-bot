using System;
using System.IO.Packaging;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace SRO_INGAME.Libraries.WebLib
{
    class APIClient : IDisposable
    {
        /**
         *  @TODO:
         *      Add post method
         *      Add JSON handelling
         */

        /// <summary>
        /// Declarations
        /// </summary>
        private string BaseAddress;
        private TimeSpan ClinetTimeout;
        private bool isCreated = false;
        private HttpClient HttpClient;
        private HttpClientHandler ClientHandler;
        private bool Debug = false;

        /// <summary>
        /// Constructor
        /// </summary>
        public APIClient(string baseUrl, TimeSpan? timeout = null)
        {
            SetBaseAddress(baseUrl);
            ClinetTimeout = timeout ?? TimeSpan.FromSeconds(60);
        }

        /// <summary>
        /// Create the http client
        /// </summary>
        private void CreateClient()
        {
            ClientHandler = new HttpClientHandler()
            {
                AutomaticDecompression = DecompressionMethods.GZip | DecompressionMethods.Deflate,
                Proxy = null,
                UseProxy = false
            };

            HttpClient = new HttpClient(ClientHandler)
            {
                BaseAddress = new Uri(BaseAddress),
                Timeout = ClinetTimeout
            };
        }

        /// <summary>
        /// RetrieveData
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <returns></returns>
        public async Task<T> RetrieveData<T>(string url) where T : class, new()
        {
            try
            {
                var response = await RetrieveData(url);

                return JsonConvert.DeserializeObject<T>(response, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver()
                });
            }
            catch { throw new Exception("Couldn't retrieve data"); }
        }

        /// <summary>
        /// Fetch API Data
        /// </summary>
        /// <returns></returns>
        public async Task<string> RetrieveData(string url)
        {
            EnsureClientCreated();

            if (Debug)
                Console.WriteLine($"[DEBUG] BaseAddress: {BaseAddress}, subURL: {url}");

            try
            {
                HttpResponseMessage response = HttpClient.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                    return await response.Content.ReadAsStringAsync();
                else
                    throw new Exception("Invalid request");
            }
            catch { throw new Exception("Couldn't retrieve data"); }

        }

        /// <summary>
        /// Ensure that client is created if not create it
        /// </summary>
        private void EnsureClientCreated()
        {
            if (!isCreated)
            {
                CreateClient();
                isCreated = true;
            }
        }

        /// <summary>
        /// Set base url
        /// </summary>
        private void SetBaseAddress(string baseUrl)
        {
            if (string.IsNullOrWhiteSpace(baseUrl))
                throw new ArgumentException("Base URL cannot be null!", "Base Url");
            else
                BaseAddress = baseUrl.EndsWith("/") ? baseUrl : baseUrl + "/";
        }

        /// <summary>
        /// check the client is created or not
        /// </summary>
        /// <returns> bool is client created </returns>
        public bool isClientCreated()
        {
            return isCreated;
        }

        /// <summary>
        /// Dispose the class
        /// </summary>
        public void Dispose()
        {
            HttpClient?.Dispose();
            ClientHandler?.Dispose();
        }
    }
}
