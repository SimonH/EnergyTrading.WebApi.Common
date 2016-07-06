namespace EnergyTrading.WebApi.Common.Faults
{
    public class Fault
    {
        public Fault()
        {
            this.category = FaultCategory.Web;
        }

        public string ErrorMessage { get; set; }

        private FaultCategory category;

        public string Category
        {
            get
            {
                return this.category;
            }
            set
            {
                this.category = FaultCategory.FromString(value);
            }
        }
    }
}