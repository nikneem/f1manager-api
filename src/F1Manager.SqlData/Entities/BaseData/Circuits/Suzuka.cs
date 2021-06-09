using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Suzuka : CircuitEntity
    {
        public Suzuka()
        {
            Id = Guid.Parse("75b106d1-ccee-43b2-aa75-7667901f22a1");
            Name = "Suzuka International Racing Course";
            FirstGrandPrix = 1987;
            NumberOfLaps = 53;
            Length = 5.807M;
            RaceDistance = 307.471M;
            LapRecord = "1:30.983 (Lewis Hamilton 2019)";
        }
    }
}
