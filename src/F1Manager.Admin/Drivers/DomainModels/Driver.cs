using System;
using System.Text.RegularExpressions;
using F1Manager.Admin.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ValueObjects;

namespace F1Manager.Admin.Drivers.DomainModels
{
    public class Driver : DomainModel<Guid>
    {
        private const decimal ObjectMinValue = 15000000M;
        private const decimal ObjectMaxValue = 60000000M;

        public string Name { get; private set; }
        public DateTimeOffset DateOfBirth { get; private set; }
        public string Country { get; private set; }
        public decimal Value { get; private set; }
        public DateTimeOffset ActiveFrom { get; private set; }
        public DateTimeOffset? ActiveUntil { get; private set; }
        public bool IsAvailable { get; private set; }
        public string PictureUrl { get; private set; }
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

        public void SetDateOfBirth(DateTimeOffset value)
        {
            AssertDeleted();

            if (!Equals(DateOfBirth, value))
            {
                DateOfBirth = value;
                SetState(TrackingState.Modified);
            }
        }

        public void SetCurrentValue(decimal value)
        {
            AssertDeleted();

            if (value < ObjectMinValue || value > ObjectMaxValue)
            {
                throw new F1ManagerObjectValueOutOfRangeException("Driver",
                    ObjectMinValue,
                    ObjectMaxValue,
                    value);
            }

            if (!Equals(Value, value))
            {
                Value = value;
                SetState(TrackingState.Modified);
            }
        }

        public void SetCountryOfOrigin(string value)
        {
            AssertDeleted();


            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerMaintenanceException(MaintenanceErrorCode.CountryIsNullOrEmpty,
                    "The country of origin for the driver cannot be null or empty");
            }

            if (!Equals(Country, value))
            {
                Country = value;
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

        public Driver() : base(Guid.NewGuid(), TrackingState.New)
        {
        }

        public Driver(Guid id,
            string name,
            DateTimeOffset dateOfBirth,
            string country,
            decimal value,
            string pictureUrl,
            DateTimeOffset activeFrom,
            DateTimeOffset? activeUntil,
            bool isAvailable,
            bool isDeleted) : base(id)
        {
            Name = name;
            DateOfBirth = dateOfBirth;
            Country = country;
            Value = value;
            PictureUrl = pictureUrl;
            ActiveFrom = activeFrom;
            ActiveUntil = activeUntil;
            IsAvailable = isAvailable;
            IsDeleted = isDeleted;
        }
    }
}
