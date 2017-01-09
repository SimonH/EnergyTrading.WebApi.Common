using System;

namespace EnergyTrading.WebApi.Common.Contracts
{
    public class ReviewablePeriod
    {
        ApplicablePeriod ApplicablePeriod { get; set; }
        DateTime NextReview { get; set; }
    }
}