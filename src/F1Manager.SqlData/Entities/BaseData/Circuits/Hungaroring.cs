using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Hungaroring : CircuitEntity
    {
        public Hungaroring()
        {
            Id = Guid.Parse("7dc16a3c-2f88-41da-9ec8-4a95ea31eae1");
            Name = "Hungaroring";
            FirstGrandPrix = 1986;
            NumberOfLaps = 70;
            Length = 4.381M;
            RaceDistance = 306.63M;
            LapRecord = "";
        }
    }
}
