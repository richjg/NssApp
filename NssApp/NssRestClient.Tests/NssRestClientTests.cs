using Moq;
using NssRestClient.Services;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NssRestClient.Tests
{
    [TestFixture]
    public class NssRestClientTests : RestApiHost
    {
        [Test]
        public async Task GetAsync_ReturnsData()
        {
            //Arrange
            base.AddRouteHandler("/v6/test", (req, res) => res.WriteJson(new { Data = "Hello" }));


            var clientCredentialStore = new InMemoryClientCredentialStore();
            await clientCredentialStore.SetAsync(new NssConnectionState { BaseUrl = Url });
            var nssRestClient = new RestClient(new NssHttpClientFactory(), clientCredentialStore);

            //Act
            var result = await nssRestClient.GetAsync<dynamic>("v6/test");

            //Assert
            Assert.That(result.LoginRequired, Is.False);
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.Result, Is.EqualTo("Hello"));
            Assert.That(result.RestResultError, Is.Null);
        }

        [Test]
        public async Task GetAsync_RestResultReturnsLoginRequired_WhneNoNssConnectionState()
        {
            //Arrange
            var clientCredentialStore = new Mock<IClientCredentialStore>().Configure(c =>
            {
                c.Setup(s => s.GetAsync()).ReturnsAsync((NssConnectionState)null);
            });
            var nssRestClient = new RestClient(new NssHttpClientFactory(), clientCredentialStore);
            
            //Act
            var result = await nssRestClient.GetAsync<dynamic>("v6/test");

            //Assert
            Assert.That(result.LoginRequired, Is.True);
            Assert.That(result.HasResult, Is.False);
            Assert.That(result.RestResultError, Is.Null);
        }

        [Test]
        public async Task GetAsync_ReAuthenticates_WhenIntialRequestIsNotAuthorized()
        {
            //Arrange
            int authRequestCount = 0;
            base.AddRouteHandler("/auth/token", (req, res) =>
            {
                authRequestCount++;
                res.WriteJson(new { access_token = "open sesame" });
            });

            int requestCount = 0;
            base.AddRouteHandler("/v6/test", (req, res) => 
            {
                requestCount++;
                if (req.Headers["Authorization"][0] == "Bearer open sesame")
                {
                    res.WriteJson(new { Data = "Hello" });
                }
                else
                {
                    res.SetStatus(System.Net.HttpStatusCode.Unauthorized);
                }
                
            });

            var clientCredentialStore = new Mock<IClientCredentialStore>().Configure(c =>
            {
                c.Setup(s => s.GetAsync()).ReturnsAsync((NssConnectionState)new NssConnectionState { AccessToken = "123", BaseUrl = Url,   });
            });
            var nssRestClient = new RestClient(new NssHttpClientFactory(), clientCredentialStore);

            //Act
            var result = await nssRestClient.GetAsync<dynamic>("v6/test");

            //Assert
            Assert.That(authRequestCount, Is.EqualTo(1));
            Assert.That(requestCount, Is.EqualTo(2));
            Assert.That(result.HasResult, Is.True);
            Assert.That(result.LoginRequired, Is.False);
            Assert.That(result.RestResultError, Is.Null);
            Assert.That(result.Result, Is.EqualTo("Hello"));
        }

        [Test]
        public async Task GetAsync_NoHttpCallIsMade_WhenReAuthenticateIsNotAuthorizedSoWeDontLockAccountsOut()
        {
            //Arrange
            int authRequestCount = 0;
            base.AddRouteHandler("/auth/token", (req, res) =>
            {
                authRequestCount++;
                res.SetStatus(System.Net.HttpStatusCode.Unauthorized);
            });

            int requestCount = 0;
            base.AddRouteHandler("/v6/test", (req, res) =>
            {
                requestCount++;
                if (req.Headers["Authorization"][0] == "Bearer open sesame")
                {
                    res.WriteJson(new { Data = "Hello" });
                }
                else
                {
                    res.SetStatus(System.Net.HttpStatusCode.Unauthorized);
                }

            });

            var clientCredentialStore = new Mock<IClientCredentialStore>().Configure(c =>
            {
                c.Setup(s => s.GetAsync()).ReturnsAsync((NssConnectionState)new NssConnectionState { AccessToken = "123", BaseUrl = Url, });
            });
            var nssRestClient = new RestClient(new NssHttpClientFactory(), clientCredentialStore);

            //Act
            var result1 = await nssRestClient.GetAsync<dynamic>("v6/test");
            var result2 = await nssRestClient.GetAsync<dynamic>("v6/test");

            //Assert
            Assert.That(authRequestCount, Is.EqualTo(1));
            Assert.That(requestCount, Is.EqualTo(1));
            Assert.That(result1.HasResult, Is.False);
            Assert.That(result1.LoginRequired, Is.True);
            Assert.That(result1.RestResultError, Is.Null);
            Assert.That(result2.HasResult, Is.False);
            Assert.That(result2.LoginRequired, Is.True);
            Assert.That(result2.RestResultError, Is.Null);
        }

    }
}
