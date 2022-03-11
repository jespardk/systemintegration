using Case.SoapService.Models;

namespace Case.SoapService.Services
{
    public class CurrencyService : ICurrencyService
    {
        public Amount ConvertToDkk(Amount amount)
        {
            Amount result = new();
            result.Currency = "DKK";
            if (amount.Currency == "USD")
            {
                result.Value = amount.Value * 6.53;
                return result;
            }
            else
            {
                return amount;
            }

        }
    }
}
