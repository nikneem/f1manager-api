using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class America : CircuitEntity
    {
        public America()
        {
            Id = Guid.Parse("c76a4298-c5d5-4dcd-b869-47e9624cb80b");
            Name = "Circuit of The Americas";
            FirstGrandPrix = 2012;
            NumberOfLaps = 56;
            Length = 5.513M;
            RaceDistance = 308.405M;
            LapRecord = "1:36.169 (Charles Leclerc 2019)";
        }
    }
}
