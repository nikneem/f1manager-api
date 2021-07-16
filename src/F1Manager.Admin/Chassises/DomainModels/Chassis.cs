using System;
using System.Text.RegularExpressions;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ValueObjects;

namespace F1Manager.Admin.Chassises.DomainModels
{
    public class Chassis : DomainModel<Guid>
    {
        private const decimal ObjectMinValue = 15000000M;
        private const decimal ObjectMaxValue = 50000000M;
        public string Name { get; private set; }
        public string Manufacturer { get; private set; }
        public string Model { get; private set; }
        public decimal Value { get; private set; }
        public decimal WeeklyWearOffPercentage { get; private set; }
        public decimal MaxWearOffPercentage { get; private set; }
        public string PictureUrl { get; private set; }
        public DateTimeOffset ActiveFrom { get; private set; }
        public DateTimeOffset? ActiveUntil { get; private set; }
        public bool IsAvailable { get; private set; }
        public bool IsDeleted { get; private set; }


        public void SetName(string value)
        {
            AssertDeleted();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.NameIsNullOrEmpty,
                    "The name of the driver cannot be null or empty");
            }

            if (!Equals(Name, value))
            {
                Name = value;
                SetState(TrackingState.Modified);
            }
        }
        public void SetCurrentValue(decimal value)
        {
            AssertDeleted();


            if (value < ObjectMinValue || value > ObjectMaxValue)
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ValueOutOfRange,
                    $"The minimum value for an engine driver is 15 Mln., the max is 50 Mln, current price is {value} ");
            }

            if (!Equals(Value, value))
            {
                Value = value;
                SetState(TrackingState.Modified);
            }
        }
        public void SetActiveDates(DateRange value)
        {
            AssertDeleted();


            if (value == null || !value.IsValid)
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ObjectActiveRangeInvalid,
                    "The date / time range the driver is active is invalid");
            }

            if (!Equals(ActiveFrom, value.From))
            {
                ActiveFrom = value.From;
                SetState(TrackingState.Modified);
            }

            if (!Equals(ActiveUntil, value.To))
            {
                ActiveUntil = value.To;
                SetState(TrackingState.Modified);
            }

        }
        public void SetAvailable(bool value)
        {
            AssertDeleted();


            if (!Equals(IsAvailable, value))
            {
                IsAvailable = value;
                SetState(TrackingState.Modified);
            }
        }
        public void SetPictureUrl(string value)
        {
            AssertDeleted();


            if (string.IsNullOrWhiteSpace(value) ||
                !Regex.IsMatch(value, RegularExpressions.UniqueResourceLocation))
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.PictureUrlInvalid,
                    "The Picture URL cannot be null or empty");
            }


            if (!Equals(PictureUrl, value))
            {
                PictureUrl = value;
                SetState(TrackingState.Modified);
            }
        }

        public void SetManufacturer(string value)
        {
            AssertDeleted();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ManufacturerNullOrEmpty,
                    "The manufacturer of the driver cannot be null or empty");
            }

            if (!Equals(Manufacturer, value))
            {
                Manufacturer = value;
                SetState(TrackingState.Modified);
            }
        }        public void SetModel(string value)
        {
            AssertDeleted();

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ModelNullOrEmpty,
                    "The model of the driver cannot be null or empty");
            }

            if (!Equals(Model, value))
            {
                Model = value;
                SetState(TrackingState.Modified);
            }
        }

        public void SetWeeklyWearOff(decimal value)
        {
            AssertDeleted();

            if (value < 0 || value > 5)
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.WeeklyWearOffInvalid,
                    "The weekly wear off value must be between 0 and 5 percent");
            }

            if (!Equals(WeeklyWearOffPercentage, value))
            {
                WeeklyWearOffPercentage = value;
                SetState(TrackingState.Modified);
            }
        }
        public void SetMaxWearOff(decimal value)
        {
            AssertDeleted();

            if (value < 0 || value > 25)
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.MaxWearOffInvalid,
                    "The maximum wear off value must be between 0 and 25 percent");
            }

            if (!Equals(MaxWearOffPercentage, value))
            {
                MaxWearOffPercentage = value;
                SetState(TrackingState.Modified);
            }
        }


        public void Delete()
        {
            if (!IsDeleted)
            {
                IsDeleted = true;
                SetState(TrackingState.Modified);
            }
        }
        public void Undelete()
        {
            if (IsDeleted)
            {
                IsDeleted = false;
                SetState(TrackingState.Modified);
            }
        }
        private void AssertDeleted()
        {
            if (IsDeleted)
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.ObjectIsDeleted,
                    "Cannot change this driver's properties, the driver is marked as deleted");
            }
        }

        public Chassis() : base(Guid.NewGuid(), TrackingState.New)
        {
        }

        public Chassis(Guid id,
            string name,
            string manufacturer,
            string model,
            string pictureUrl,
            decimal value,
            decimal weeklyWearOff,
            decimal maxWearOff,
            DateTimeOffset activeFrom,
            DateTimeOffset? activeUntil,
            bool isAvailable,
            bool isDeleted) : base(id, TrackingState.Pristine)
        {
            Name = name;
            Manufacturer = manufacturer;
            Model = model;
            PictureUrl = pictureUrl;
            Value = value;
            WeeklyWearOffPercentage = weeklyWearOff;
            MaxWearOffPercentage = maxWearOff;
            ActiveFrom = activeFrom;
            ActiveUntil = activeUntil;
            IsAvailable = isAvailable;
            IsDeleted = isDeleted;
        }
    }
}