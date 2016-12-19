using System;
using EnergyTrading.WebApi.Common.Contracts;

namespace EnergyTrading.WebApi.Common.Extensions
{
    public static class UniqueContractExtensions
    {
        public static void EnsureIdIsSet(this UniqueContract contract)
        {
            if (contract.UniqueReference == null)
            {
                throw new ArgumentException("UniqueReference must be set");
            }
        }

        public static void EnsureIdIsNotSet(this UniqueContract contract)
        {
            if (contract.UniqueReference != null)
            {
                throw new ArgumentException("UniqueReference must not be set");
            }
        }
    }
}