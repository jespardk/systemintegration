using Common.Models;

namespace DomainServices.DanishEnergyPrice
{
    public class DanishEnergyPriceResponse : BaseResponse
    {
        public string PriceArea { get; set; }
        public List<DanishEnergyPriceRecordResponse> Records { get; set; }
        public int HourSpan { get; set; }
    }

    public class DanishEnergyPriceRecordResponse
    {
        public DateTime HourDk { get; set; }
        public double SpotPriceMegawattInDKK { get; set; }
        public double SpotPriceMegawattInEUR { get; set; }
        public double SpotPriceKilowattInDKK { get; internal set; }
        public double SpotPriceKilowattInEUR { get; internal set; }
    }
}
