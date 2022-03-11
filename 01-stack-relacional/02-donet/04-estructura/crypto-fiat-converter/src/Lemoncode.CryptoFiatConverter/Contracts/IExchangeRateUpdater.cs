using System.Threading.Tasks;

namespace Lemoncode.CryptoFiatConverter.Contracts
{
public interface IExchangeRateUpdater
{
    Task Update();
}
}
