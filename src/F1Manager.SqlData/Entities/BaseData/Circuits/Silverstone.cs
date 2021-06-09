using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Silverstone : CircuitEntity
    {
        public Silverstone()
        {
            Id = Guid.Parse("e2d56321-9d50-4010-a40d-4696267f5f5e");
            Name = "Silverstone Circuit";
            FirstGrandPrix = 1950;
            NumberOfLaps = 52;
            Length = 5.891M;
            RaceDistance = 306.198M;
            LapRecord = "1:27.097 (Max Verstappen 2020)";
        }
    }
}
