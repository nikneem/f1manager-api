using System;
using Microsoft.Azure.Cosmos.Table;

namespace F1Manager.Admin.Drivers.Entities
{
    public class DriverEntity : TableEntity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public decimal Value { get; set; }
        public string PictureUrl { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveUntil { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsDeleted { get; set; }
    }
}
