using System;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;

namespace F1Manager.Leagues.DomainModels
{
    public class LeagueMember : DomainModel<Guid>
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; }
        public int TeamPoints { get; set; }

        public LeagueMember(Guid id, TrackingState initialState = null) : base(id, initialState)
        {
        }
    }
}