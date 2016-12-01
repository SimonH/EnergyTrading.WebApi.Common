namespace EnergyTrading.WebApi.Common.Contracts
{
    public abstract class VersionedUniqueContract : UniqueContract
    {
        private string _version;

        public string Version
        {
            get
            {
                return _version;
            }
            set
            {
                Guard(value, nameof(Version));
                _version = value;
            }
        }
    }
}