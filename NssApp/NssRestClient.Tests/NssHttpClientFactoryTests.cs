using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NssRestClient.Tests
{
    [TestFixture]
    public class NssHttpClientFactoryTests
    {
        [TestCase("")]
        [TestCase(null)]
        [TestCase("  ")]
        [TestCase("Not A Url")]
        public void Create_Throws_WhenBaseUrlIsNotValid(string url)
        {
            //Act
            void act()
            {
                new NssHttpClientFactory().Create(url);
            }

            //Assert
            Assert.That(act, Throws.Exception.TypeOf<ArgumentException>().With.Message.Contain("A valid url is required"));
        }

        [Test]
        public void Create_ReturnsHttpClient_WhenBaseUrlIsValid()
        {
            //Act
            var result = new NssHttpClientFactory().Create("http://biomni.com/nss");

            //Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.BaseAddress.OriginalString, Is.EqualTo("http://biomni.com/nss"));
        }
        
        [Test]
        public void Create_ReturnsTheSameHttpClientInstance_WhenBaseUrlIsTheSame()
        {
            //Act
            var result1 = new NssHttpClientFactory().Create("http://biomni.com/nss");
            var result2 = new NssHttpClientFactory().Create("http://biomni.com/nss");
            var result3 = new NssHttpClientFactory().Create("http://biomni.com/nss/other");

            //Assert
            Assert.That(result1, Is.SameAs(result2));
            Assert.That(result1, Is.Not.SameAs(result3));
            Assert.That(result2, Is.Not.SameAs(result3));
        }

    }
}
