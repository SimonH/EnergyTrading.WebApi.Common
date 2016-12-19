using System.Collections.Generic;

namespace EnergyTrading.WebApi.Common.Contracts
{
    public abstract class MappedContract : VersionedUniqueContract
    {
        protected MappedContract()
        {
            Mappings = new List<MappingId>();
        }

        public List<MappingId> Mappings { get; set; }
    }
}