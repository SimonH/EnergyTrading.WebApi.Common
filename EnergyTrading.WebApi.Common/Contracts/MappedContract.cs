using System.Collections.Generic;

namespace EnergyTrading.WebApi.Common.Contracts
{
    public abstract class MappedContract : VersionedUniqueContract
    {
        public MappedContract()
        {
            Mappings = new List<MappingId>();
        }

        List<MappingId> Mappings { get; set; }
    }
}