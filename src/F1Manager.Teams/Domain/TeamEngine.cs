using System;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;
using F1Manager.Teams.Exceptions;

namespace F1Manager.Teams.Domain
{
    public sealed class TeamEngine : DomainModel<Guid>
    {
        public Guid EngineId { get; }
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

            SoldFor = GetSellingPrice(price);
            SoldOn = DateTimeOffset.UtcNow;
            SetState(TrackingState.Modified);
            return SoldFor.GetValueOrDefault();
        }

        public decimal GetSellingPrice(decimal currentValue)
        {
            return currentValue - currentValue * WarnOffPercentage / 100;
        }

        public TeamEngine(Guid id,
            Guid engineId,
            decimal boughtFor,
            decimal? soldFor,
            int totalPointsGained,
            decimal warnOffPercentage,
            DateTimeOffset boughtOn,
            DateTimeOffset? soldOn) : base(id)
        {
            EngineId = engineId;
            BoughtFor = boughtFor;
            SoldFor = soldFor;
            TotalPointsGained = totalPointsGained;
            WarnOffPercentage = warnOffPercentage;
            BoughtOn = boughtOn;
            SoldOn = soldOn;
        }

        public TeamEngine(Guid engineId,
            decimal boughtFor) : base(Guid.NewGuid(), TrackingState.New)
        {
            EngineId = engineId;
            BoughtFor = boughtFor;
            TotalPointsGained = 0;
            WarnOffPercentage = 0M;
            BoughtOn = DateTimeOffset.UtcNow;
        }


    }
}
