using System;

namespace EnergyTrading.WebApi.Common.Contracts
{
    public abstract class UniqueContract
    {
        private string _uniqueReference;

        protected static void Guard<T>(T value, string propertyName) where T : class
        {
            if (value != null)
            {
                throw new InvalidOperationException($"{propertyName} cannot be altered once it has been set");
            }
        }

        public string UniqueReference
        {
            get
            {
                return _uniqueReference;
            }
            set
            {
                Guard(value, nameof(UniqueReference));
                _uniqueReference = value;
            }
        }
    }
}