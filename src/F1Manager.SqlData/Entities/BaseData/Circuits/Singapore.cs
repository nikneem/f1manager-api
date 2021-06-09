using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Singapore : CircuitEntity
    {
        public Singapore()
        {
            Id = Guid.Parse("8e995cba-0e86-46df-b3ae-519c756cf0d9");
            Name = "Marina Bay Street Circuit";
            FirstGrandPrix = 2008;
            NumberOfLaps = 61;
            Length = 5.063M;
            RaceDistance = 308.706M;
            LapRecord = "1:41.905 (Kevin Magnussen 2018)";
        }
    }
}
