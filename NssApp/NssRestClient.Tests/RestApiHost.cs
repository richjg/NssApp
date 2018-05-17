using Microsoft.AspNetCore.Http;
using Microsoft.Owin;
using Microsoft.Owin.Hosting;
using Newtonsoft.Json;
using NUnit.Framework;
using Owin;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using PathString = Microsoft.Owin.PathString;

namespace NssRestClient.Tests
{
    [TestFixture]
    public abstract class RestApiHost
    {
        protected static int Port = 44399; //Ports 44300 to 44399 reserved for Certificates > Personal > Cedrtificate > IIS Express Development Certificate - run 'netsh http show sslcert' in command prompt to see list.
        protected static string Url = $"https://localhost:{Port}/NssRestClient/";
        private IDisposable webApp = null;
        private readonly List<RouteFunc> routeHandlers = new List<RouteFunc>();

        public void AddRouteHandler(string route, Action<Request, Response> handler)
        {
            this.routeHandlers.Add(new RouteFunc { Route = route, Handler = handler });
        }

        public void ClearRouterHandlers() => this.routeHandlers.Clear();

        private RouteFunc FindRouteHandler(IOwinRequest owinRequest) => this.routeHandlers.FirstOrDefault(r => owinRequest.Path.StartsWithSegments(new PathString(r.Route)));

        [SetUp]
        public void RestApiHostSetup()
        {
            this.ClearRouterHandlers();
            ServicePointManager.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            this.webApp = WebApp.Start(Url, (appBuilder) =>
            {
                appBuilder.Use((c, t) =>
                {
                    var routeHandler = this.FindRouteHandler(c.Request);
                    if (routeHandler == null)
                    {
                        Console.WriteLine($"No handler found for {c.Request.Uri.AbsoluteUri} Add one using AddRouteHandler");
                        c.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                        c.Response.ReasonPhrase = $"No handler found for {c.Request.Uri.AbsoluteUri} Add one using AddRouteHandler";
                    }
                    else
                    {
                        routeHandler.Handler(new Request(c.Request), new Response(c.Response));
                    }

                    return Task.CompletedTask;
                });
            });
        }

        [TearDown]
        public void RestApiHostTearDown()
        {
            this.webApp?.Dispose();
        }

        private class RouteFunc
        {
            public string Route { get; set; }
            public Action<Request, Response> Handler { get; set; }
        }

        public class Request
        {
            private readonly IOwinRequest owinRequest;

            public Request(IOwinRequest owinRequest)
            {
                this.owinRequest = owinRequest;
            }

            public Uri Uri => this.owinRequest.Uri;

            private string body = null;
            public string Body
            {
                get
                {
                    if (body == null)
                    {
                        body = new StreamReader(this.owinRequest.Body).ReadToEnd() ?? string.Empty;
                    }
                    return body;
                }
            }

            public HttpMethod HttpMethod => (HttpMethod)Enum.Parse(typeof(HttpMethod), this.owinRequest.Method);
            public IDictionary<string, string[]> Headers => this.owinRequest.Headers;
        }

        public class Response
        {
            private readonly IOwinResponse owinResponse;

            public Response(IOwinResponse owinResponse)
            {
                this.owinResponse = owinResponse;
            }

            public void SetStatus(HttpStatusCode httpStatusCode) => this.owinResponse.StatusCode = (int)httpStatusCode;
            public void Write(string text) => this.owinResponse.WriteAsync(text).GetAwaiter().GetResult();
            public void WriteJson(object obj) => this.Write(JsonConvert.SerializeObject(obj));
        }
    }
}
