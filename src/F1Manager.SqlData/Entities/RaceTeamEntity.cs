using System;
using F1Manager.SqlData.Base;

namespace F1Manager.SqlData.Entities
{
    public class RaceTeamEntity : Entity<Guid>
    {
        public string DisplayName { get; set; }
        public string Base { get; set; }
        public string TeamChief { get; set; }
        public string TechnicalChief { get; set; }
        public int YearFirstEntry { get; set; }
        public Guid FirstDriverId { get; set; }
        public Guid SecondDriverId { get; set; }
        public Guid EngineId { get; set; }
        public Guid ChassisId { get; set; }
        public string PictureUrl { get; set; }
        public string OfficialWebsiteUrl { get; set; }

    }
}