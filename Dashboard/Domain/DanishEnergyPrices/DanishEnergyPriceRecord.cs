namespace Domain.DanishEnergyPrices
{
    public class DanishEnergyPriceRecord
    {
        public DateTime HourDk { get; set; }
        public double? SpotPriceMegawattInDKK { get; set; }
        public double? SpotPriceMegawattInEUR { get; set; }
        public double? SpotPriceKilowattInDKK { get; set; }
        public double? SpotPriceKilowattInEUR { get; set; }
        public bool HasPriceData { get; set; }
    }
}
