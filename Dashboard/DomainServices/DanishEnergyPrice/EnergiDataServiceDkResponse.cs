namespace DomainServices.DanishEnergyPrice
{
    public class Rootobject
    {
        public string help { get; set; }
        public bool success { get; set; }
        public Result result { get; set; }
    }

    public class Result
    {
        public Record[] records { get; set; }
        public Field[] fields { get; set; }
        public string sql { get; set; }
    }

    public class Record
    {
        public DateTime HourDK { get; set; }
        public object SpotPriceDKK { get; set; }
    }

    public class Field
    {
        public string type { get; set; }
        public string id { get; set; }
    }

}
