using EnergyTrading.Logging;

namespace EnergyTrading.WebApi.Common.Server
{
    public interface ICreateLoggerFactories
    {
        /// <summary>
        /// Creates an instance of ILoggerFactory or throws a FaultException containing the error information
        /// </summary>
        /// <returns>The created ILoggerFactory</returns>
        ILoggerFactory CreateInstance();
    }
}