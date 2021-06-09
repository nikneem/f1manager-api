using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
   public  class Zandvoort: CircuitEntity
    {
        public Zandvoort()
        {
            Id = Guid.Parse("0b276708-76e4-407a-891f-22da2be6cfa5");
            Name = "Circuit Zandvoort";
            FirstGrandPrix = 1952;
            NumberOfLaps = 72;
            Length = 4.259M;
            RaceDistance = 306.648M;
            LapRecord = "";
        }
    }
}
