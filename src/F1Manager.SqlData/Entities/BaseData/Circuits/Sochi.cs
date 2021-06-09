using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Sochi : CircuitEntity
    {
        public Sochi()
        {
            Id = Guid.Parse("589199ad-6383-48f5-b515-0dea2c2996e6");
            Name = "Sochi Autodrom";
            FirstGrandPrix = 2014;
            NumberOfLaps = 53;
            Length = 5.848M;
            RaceDistance = 309.745M;
            LapRecord = "1:35.761 (Lewis Hamilton 2019)";
        }
    }
}
