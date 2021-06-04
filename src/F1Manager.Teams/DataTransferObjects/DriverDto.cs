using System;

namespace F1Manager.Teams.DataTransferObjects
{
    public sealed class DriverDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Country { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public decimal Value { get; set; }
        public string PictureUrl { get; set; }
    }
}
