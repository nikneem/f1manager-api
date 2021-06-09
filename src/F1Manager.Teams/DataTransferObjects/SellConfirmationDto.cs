using System;
using System.Collections.Generic;
using System.Text;

namespace F1Manager.Teams.DataTransferObjects
{
    public class SellConfirmationDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int NumberOfRacesDriven { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal WearOffPercentage { get; set; }
        public decimal SellingPrice { get; set; }

    }
}
