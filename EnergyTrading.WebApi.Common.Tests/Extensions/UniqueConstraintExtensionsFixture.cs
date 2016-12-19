using System;
using EnergyTrading.WebApi.Common.Contracts;
using EnergyTrading.WebApi.Common.Extensions;
using NUnit.Framework;

namespace EnergyTrading.WebApi.Common.Tests.Extensions
{
    [TestFixture]
    public class UniqueConstraintExtensionsFixture
    {

        public class TestUniqueContract : UniqueContract
        {
        }

        [Test]
        public void EnsureIdIsSet()
        {
            Assert.That(() => new TestUniqueContract().EnsureIdIsSet(), Throws.TypeOf<ArgumentException>());
            new TestUniqueContract { UniqueReference = "id" }.EnsureIdIsSet();
        }

        [Test]
        public void EnsureIdIsNotSet()
        {
            Assert.That(() => new TestUniqueContract { UniqueReference = "id" }.EnsureIdIsNotSet(), Throws.TypeOf<ArgumentException>());
            new TestUniqueContract().EnsureIdIsNotSet();
        }
    }
}