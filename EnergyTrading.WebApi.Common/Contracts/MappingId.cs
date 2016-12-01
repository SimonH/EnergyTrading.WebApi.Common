namespace EnergyTrading.WebApi.Common.Contracts
{
    public class MappingId : UniqueContract
    {
        public string SourceName { get; set; }
        public string Identifier { get; set; }
        public bool IsOriginal { get; set; }
    }
}