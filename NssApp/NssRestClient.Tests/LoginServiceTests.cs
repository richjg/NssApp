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
    public class LoginServiceTests : RestApiHost
    {
        [Test]
        public async Task SignInAsync_ReturnsTrue_WhenAccessTokenReturn()
        {
            //Arrange
            base.AddRouteHandler("/auth/token", (req, res) => res.WriteJson(new { access_token = "123456" }));

            var loginService = new LoginService(new RestClient(new NssHttpClientFactory(), new InMemoryClientCredentialStore()));

            //Act
            var result = await loginService.SignInAsync(Url, "username", "password");

            //Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public async Task SignInAsync_SavesCredsToStore_WhenSigninOk()
        {
            //Arrange
            base.AddRouteHandler("/auth/token", (req, res) => res.WriteJson(new { access_token = "123456" }));
            var clientCredentialStore = new InMemoryClientCredentialStore();
            var loginService = new LoginService(new RestClient(new NssHttpClientFactory(), clientCredentialStore));

            //Act
            var result = await loginService.SignInAsync(Url, "username", "password");

            //Assert
            Assert.That(result, Is.True);
            Assert.That(clientCredentialStore.HasCredentialsAsync, Is.True);
            var creds = await clientCredentialStore.GetAsync();
            Assert.That(creds, Is.Not.Null);
            Assert.That(creds.AccessToken, Is.EqualTo("123456"));
            Assert.That(creds.BaseUrl, Is.EqualTo(Url));
            Assert.That(creds.Password, Is.EqualTo("password"));
            Assert.That(creds.RetryAuthenticateFailed, Is.False);
            Assert.That(creds.Username, Is.EqualTo("username"));
        }

        [Test]
        public async Task SignInAsync_ReturnsFalse_WhenAccessTokenReturn()
        {
            //Arrange
            base.AddRouteHandler("/auth/token", (req, res) => res.SetStatus(System.Net.HttpStatusCode.Unauthorized) );
            var loginService = new LoginService(new RestClient(new NssHttpClientFactory(), new InMemoryClientCredentialStore()));
            
            //Act
            var result = await loginService.SignInAsync(Url, "username", "password");

            //Assert
            Assert.That(result, Is.False);
        }
        
        [Test]
        public async Task SignInAsync_ReturnsFalse_WhenServerThrows()
        {
            //Arrange
            base.AddRouteHandler("/auth/token", (req, res) => throw new Exception("woop"));
            var loginService = new LoginService(new RestClient(new NssHttpClientFactory(), new InMemoryClientCredentialStore()));
            
            //Act
            var result = await loginService.SignInAsync(Url, "username", "password");
            
            //Assert
            Assert.That(result, Is.False);
        }
    }
}
