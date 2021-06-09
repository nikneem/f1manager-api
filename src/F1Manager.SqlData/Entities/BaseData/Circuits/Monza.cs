using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Monza : CircuitEntity
    {
        public Monza()
        {
            Id = Guid.Parse("9a068c4a-b406-4d23-bf69-4609ca2132f6");
            Name = "Autodromo Nazionale Monza";
            FirstGrandPrix = 1950;
            NumberOfLaps = 53;
            Length = 5.793M;
            RaceDistance = 306.72M;
            LapRecord = "1:21.046 (Rubens Barrichello 2004)";
        }
    }
}
