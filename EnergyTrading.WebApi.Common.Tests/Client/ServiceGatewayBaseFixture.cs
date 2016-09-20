using EnergyTrading.WebApi.Common.Client;
using NUnit.Framework;

namespace EnergyTrading.WebApi.Common.Tests.Client
{
    [TestFixture]
    public class ServiceGatewayBaseFixture
    {
        private class TestServiceGateway : ServiceGatewayBase
        {
            public TestServiceGateway(string baseUri, string path) : base(baseUri, path)
            {
            }

            public string ExposedServiceUri => ServiceUri;
        }

        [Test]
        [TestCase("http://basevalue", "", "http://basevalue")]
        [TestCase("http://basevalue", null, "http://basevalue")]
        [TestCase("http://basevalue", "   ", "http://basevalue")]
        [TestCase("http://basevalue/", "", "http://basevalue")]
        [TestCase("http://basevalue/", null, "http://basevalue")]
        [TestCase("http://basevalue/", "   ", "http://basevalue")]
        [TestCase("http://basevalue", "/", "http://basevalue")]
        [TestCase("http://basevalue/", "/", "http://basevalue")]
        [TestCase("http://basevalue", "/some/path/value", "http://basevalue/some/path/value")]
        [TestCase("http://basevalue/", "/some/path/value", "http://basevalue/some/path/value")]
        [TestCase("http://basevalue", "some/path/value", "http://basevalue/some/path/value")]
        [TestCase("http://basevalue/", "some/path/value", "http://basevalue/some/path/value")]
        public void ConstructedServiceUri(string baseUri, string path, string expected)
        {
            Assert.That(new TestServiceGateway(baseUri, path).ExposedServiceUri, Is.EqualTo(expected));
        } 
    }
}