using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Imola : CircuitEntity
    {
        public Imola()
        {
            Id = Guid.Parse("1b78bf79-694d-4598-9ff6-7b791eec635d");
            Name = "Autodromo Enzo e Dino Ferrari";
            FirstGrandPrix = 1980;
            NumberOfLaps = 63;
            Length = 4.909M;
            RaceDistance = 309.049M;
            LapRecord = "1:15.484 (Lewis Hamilton 2020)";
        }
    }
}
