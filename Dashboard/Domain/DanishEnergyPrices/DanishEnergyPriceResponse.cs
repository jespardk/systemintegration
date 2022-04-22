using Common.Models;

namespace Domain.DanishEnergyPrices
{
    public class DanishEnergyPriceResponse : BaseResponse
    {
        public DanishEnergyPriceArea PriceArea { get; set; }
        public List<DanishEnergyPriceRecordResponse> Records { get; set; }
        public List<DanishEnergyPriceRecordResponse> RecordsWithPriceData => Records?.Where(_ => _.HasPriceData)?.ToList();

        public int HourSpan { get; set; }
    }

    public class DanishEnergyPriceRecordResponse
    {
        public bool HasPriceData { get; set; }
        public DateTime HourDk { get; set; }
        public double? SpotPriceMegawattInDKK { get; set; }
        public double? SpotPriceMegawattInEUR { get; set; }
        public double? SpotPriceKilowattInDKK { get; set; }
        public double? SpotPriceKilowattInEUR { get; set; }
    }

    public enum DanishEnergyPriceArea
    {
        Unknown, DK1, DK2
    }
}
