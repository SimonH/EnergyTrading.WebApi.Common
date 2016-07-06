using EnergyTrading.serialization;

namespace EnergyTrading.WebApi.Common.Faults
{
    public class FaultCategory : StringEnumBase<FaultCategory>
    {
        private FaultCategory(string category)
            : base(category)
        {
        }

        public static readonly FaultCategory Web = Create("Web");
        public static readonly FaultCategory Concurrency = Create("Concurrency");
        public static readonly FaultCategory Unknown = Create("Unknown");

        private static FaultCategory Create(string category)
        {
            return new FaultCategory(category);
        }


        public static implicit operator string(FaultCategory fault)
        {
            return fault?.Value;
        }
    }
}