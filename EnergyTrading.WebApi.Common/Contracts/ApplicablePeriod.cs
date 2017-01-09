using System;

namespace EnergyTrading.WebApi.Common.Contracts
{
    public class ApplicablePeriod
    {
        DateTime Start { get; set; }
        DateTime End { get; set; }
    }
}