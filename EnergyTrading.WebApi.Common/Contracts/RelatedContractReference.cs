namespace EnergyTrading.WebApi.Common.Contracts
{
    public class RelatedContractReference
    {
        public EnumValueType Relationship { get; set; }
        public ContractReference Contract { get; set; }
    }
}