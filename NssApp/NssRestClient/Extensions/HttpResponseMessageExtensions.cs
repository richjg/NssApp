using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace NssRestClient
{ 
    internal static class HttpResponseMessageExtensions
    { 
    
        public static Task<T> ContentFromJsonAsync<T>(this HttpResponseMessage httpResponseMessage) => httpResponseMessage.Content.ReadAsStringAsync().FromJsonAsync<T>();

        public static HttpRequestMessage WithHeaders(this HttpRequestMessage httpRequestMessage, IEnumerable<KeyValuePair<HttpRequestHeader, string>> headers)
        {
            if (headers != null)
            {
                foreach (var header in headers)
                {
                    httpRequestMessage.Headers.Add(header.Key.ToString(), header.Value);
                }
            }
            return httpRequestMessage;
        }
    }
}
