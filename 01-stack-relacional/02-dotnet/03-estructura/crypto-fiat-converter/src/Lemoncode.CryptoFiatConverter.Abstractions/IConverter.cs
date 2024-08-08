namespace Lemoncode.CryptoFiatConverter.Abstractions
{
    public interface IConverter
    {
        ConversionResult ConvertToEur(string cryptoCode, decimal amount);
    }
}
