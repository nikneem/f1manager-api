using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
   public  class PaulRicard : CircuitEntity
    {
        public PaulRicard()
        {
            Id = Guid.Parse("26562424-7ae8-431e-8a21-b27e1b18b61f");
            Name = "Circuit Paul Ricard";
            FirstGrandPrix = 1971;
            NumberOfLaps = 53;
            Length = 5.842M;
            RaceDistance = 309.69M;
            LapRecord = "1:32.740 (Sebastian Vettel 2019)";
        }
    }
}
