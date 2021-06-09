using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Monaco : CircuitEntity
    {
        public Monaco()
        {
            Id = Guid.Parse("876beb63-0d3a-4290-b260-af136cee6af4");
            Name = "Circuit de Monaco";
            FirstGrandPrix = 1950;
            NumberOfLaps = 78;
            Length = 3.337M;
            RaceDistance = 260.286M;
            LapRecord = "1:14.260 (Max Verstappen 2018)";
        }
    }
}
