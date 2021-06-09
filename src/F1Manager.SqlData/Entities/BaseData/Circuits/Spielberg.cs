using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Spielberg : CircuitEntity
    {
        public Spielberg()
        {
            Id = Guid.Parse("efcd11bd-77d9-46f7-8b80-0b37dfe958cb");
            Name = "Red Bull Ring";
            FirstGrandPrix = 1970;
            NumberOfLaps = 71;
            Length = 4.318M;
            RaceDistance = 306.452M;
            LapRecord = "1:05.619 (Carlos Sainz 2020)";
        }
    }
}
