﻿using System;

namespace F1Manager.Admin.Drivers.DataTransferObjects
{
    public class DriverDetailsDto
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public DateTimeOffset DateOfBirth { get; set; }
        public string Country { get; set; }
        public decimal Value { get; set; }
        public DateTimeOffset ActiveFrom { get; set; }
        public DateTimeOffset? ActiveUntil { get; set; }
        public bool IsAvailable { get; set; }
        public string PictureUrl { get; set; }
        public bool IsDeleted { get; set; }
    }
}
