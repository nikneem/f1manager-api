﻿using System;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;
using F1Manager.Teams.Exceptions;

namespace F1Manager.Teams.Domain
{
    public sealed class TeamDriver : DomainModel<Guid>
    {
        public Guid DriverId { get; }
        public decimal BoughtFor { get; }
        public bool IsFirstDriver { get; }
        public decimal? SoldFor { get; private set; }
        public int TotalPointsGained { get; private set; }
        public DateTimeOffset BoughtOn { get; }
        public DateTimeOffset? SoldOn { get; private set; }


        internal decimal Sell(decimal price)
        {
            if (SoldFor.HasValue || SoldOn.HasValue)
            {
                throw new F1ManagerTeamException(TeamErrorCode.InvalidTransfer, "Team component is already sold");
            }

            SoldFor = price;
            SoldOn = DateTimeOffset.UtcNow;
            SetState(TrackingState.Modified);
            return price;
        }

        public TeamDriver(Guid id,
            Guid driverId, 
            decimal boughtFor, 
            decimal? soldFor, 
            int totalPointsGained,
            bool isFirstDriver,
            DateTimeOffset boughtOn, 
            DateTimeOffset? soldOn) : base(id)
        {
            DriverId = driverId;
            BoughtFor = boughtFor;
            SoldFor = soldFor;
            TotalPointsGained = totalPointsGained;
            BoughtOn = boughtOn;
            SoldOn = soldOn;
            IsFirstDriver = isFirstDriver;
        }

        public TeamDriver(
            Guid driverId,
            decimal boughtFor, bool isFirstDriver) : base(Guid.NewGuid(), TrackingState.New)
        {
            DriverId = driverId;
            BoughtFor = boughtFor;
            IsFirstDriver = isFirstDriver;
            TotalPointsGained = 0;
            BoughtOn = DateTimeOffset.UtcNow;
        }

    }
}