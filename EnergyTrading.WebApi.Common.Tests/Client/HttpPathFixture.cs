using System;
using EnergyTrading.WebApi.Common.Client;
using NUnit.Framework;

namespace EnergyTrading.WebApi.Common.Tests.Client
{
    [TestFixture]
    public class HttpPathFixture
    {
        [Test]
        [TestCase(null, "some/path", typeof(ArgumentException))]
        [TestCase("", "some/path", typeof(ArgumentException))]
        [TestCase("     ", "some/path", typeof(ArgumentException))]
        [TestCase("notavaliduri", "some/path", typeof(ArgumentException))]
        public void CombineInvalid(string baseUri, string path, Type exceptionType)
        {
            Assert.That(() => HttpPath.Combine(baseUri, path), Throws.TypeOf(exceptionType));
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
        public void CombineValid(string baseUri, string path, string expected)
        {
            Assert.That(HttpPath.Combine(baseUri, path), Is.EqualTo(expected));
        }
    }
}