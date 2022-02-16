using System;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;

namespace F1Manager.Leagues.DomainModels
{
    public class LeagueMember : ValueObject
    {
        public Guid TeamId { get;  }
        public DateTimeOffset CreatedOn { get; }
        public bool IsMaintainer { get; private set; }

        public void SetMaintainer(bool value)
        {
            if (!Equals(IsMaintainer, value))
            {
                IsMaintainer = value;
                SetState(TrackingState.Modified);
            }
        }

        public void Delete()
        {
            SetState( TrackingState.Deleted);
        }

        public LeagueMember( Guid teamId, DateTimeOffset created)
        {
            TeamId=teamId;
            CreatedOn = created;
        }
        private LeagueMember( Guid teamId) : base( TrackingState.New)
        {
            TeamId = teamId;
            CreatedOn = DateTimeOffset.UtcNow;
        }

        public static LeagueMember Create( Guid teamId)
        {
            return new LeagueMember( teamId);
        }
    }
}