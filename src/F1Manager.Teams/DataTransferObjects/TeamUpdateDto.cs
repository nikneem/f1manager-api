using System;
using System.Collections.Generic;
using System.Text;

namespace F1Manager.Teams.DataTransferObjects
{
    public class TeamUpdateDto
    {
        public string Name { get; set; }
        public bool? IsPublic { get; set; }
    }
}
