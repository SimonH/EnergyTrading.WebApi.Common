using System;

namespace EnergyTrading.WebApi.Common.Faults
{
    public class FaultException : Exception
    {
        public FaultException(Fault fault) : base(fault.ErrorMessage)
        {
            Fault = fault;
        }

        public Fault Fault { get; private set; }
    }
}