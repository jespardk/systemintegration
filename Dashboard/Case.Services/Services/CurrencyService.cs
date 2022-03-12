namespace Case.Services
{
    public class CurrencyService
    {
        public void TryLocalService()
        {
            var service = new References.CurrencyService.CurrencyServiceClient();

            var request = new References.CurrencyService.Amount();
            request.Currency = "USD";
            request.Value = 102;

            var result = service.ConvertToDkkAsync(request).Result;
        }
    }
}