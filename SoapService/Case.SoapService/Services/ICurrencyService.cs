using Case.SoapService.Models;
using System.ServiceModel;

namespace Case.SoapService.Services
{
    [ServiceContract]
    public interface ICurrencyService
    {
        [OperationContract]
        public Amount ConvertToDkk(Amount amount);
    }
}
