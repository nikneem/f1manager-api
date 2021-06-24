using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Teams.Entities
{
    public class ChassisEntity : TableEntity
    {
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public double Value { get; set; }
        public string PictureUrl { get; set; }
        public double WeeklyWearDown { get; set; }
        public double MaximumWearDown { get; set; }
    }
}
