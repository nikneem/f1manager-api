﻿using System;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;
using F1Manager.Teams.Exceptions;

namespace F1Manager.Teams.Domain
{
    public sealed class TeamChassis : DomainModel<Guid>
    {
        public Guid ChassisId { get; }
        public string Name { get; }
        public string PictureUrl { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public decimal BoughtFor { get; }
        public decimal? SoldFor { get; private set; }
        public int TotalPointsGained { get; private set; }
        public decimal WarnOffPercentage { get; private set; }
        public DateTimeOffset BoughtOn { get; }
        public DateTimeOffset? SoldOn { get; private set; }

        internal decimal Sell(decimal price)
        {
            if (SoldFor.HasValue || SoldOn.HasValue)
            {
                throw new F1ManagerTeamException(TeamErrorCode.InvalidTransfer, "Team component is already sold");
            }

            SoldFor = GetSellPrice(price);
            SoldOn = DateTimeOffset.UtcNow;
            SetState(TrackingState.Modified);
            return SoldFor.GetValueOrDefault();
        }

        public decimal GetSellPrice(decimal currentValue)
        {
            return currentValue - (currentValue * WarnOffPercentage / 100);
        }

        public TeamChassis(Guid id,
            Guid chassisId,
            string name,
            string pictureUrl,
            string manufacturer,
            string model,
            decimal boughtFor,
            decimal? soldFor,
            int totalPointsGained,
            decimal warnOffPercentage,
            DateTimeOffset boughtOn,
            DateTimeOffset? soldOn) : base(id)
        {
            ChassisId = chassisId;
            Name = name;
            PictureUrl = pictureUrl;
            Manufacturer = manufacturer;
            Model = model;
            BoughtFor = boughtFor;
            SoldFor = soldFor;
            TotalPointsGained = totalPointsGained;
            WarnOffPercentage = warnOffPercentage;
            BoughtOn = boughtOn;
            SoldOn = soldOn;
        }

        public TeamChassis(Guid chassisId,
            string name,
            decimal boughtFor) : base(Guid.NewGuid(), TrackingState.New)
        {
            ChassisId = chassisId;
            Name = name;
            BoughtFor = boughtFor;
            TotalPointsGained = 0;
            WarnOffPercentage = 0M;
            BoughtOn = DateTimeOffset.UtcNow;
        }

        public decimal GetSellingPrice(decimal currentValue)
        {
            return currentValue - currentValue * WarnOffPercentage / 100;
        }
    }
}