using Common.Models;

namespace Domain.DanishEnergyPrices
{
    public class DanishEnergyPriceResponse : BaseResponse
    {
        public DanishEnergyPriceArea PriceArea { get; set; }
        public List<DanishEnergyPriceRecord> Records { get; set; }
        public List<DanishEnergyPriceRecord> RecordsWithPriceData => Records?.Where(_ => _.HasPriceData)?.ToList();

        public int HourSpan { get; set; }
    }

    public enum DanishEnergyPriceArea
    {
        Unknown, DK1, DK2
    }
}
