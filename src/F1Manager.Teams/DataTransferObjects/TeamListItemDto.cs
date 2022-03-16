using System;

namespace F1Manager.Teams.DataTransferObjects
{
    public class TeamListItemDto
    {
        public Guid TeamId { get; set; }
        public string Name { get; set; }
        public int Points { get; set; }
        public decimal Money { get; set; }
    }
}
