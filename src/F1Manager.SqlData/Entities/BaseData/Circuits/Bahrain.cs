using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Bahrain : CircuitEntity
    {
        public Bahrain()
        {
            Id = Guid.Parse("899ad336-b0f0-4102-95bc-ae9dcc8c33c4");
            Name = "Bahrain International Circuit";
            FirstGrandPrix = 2004;
            NumberOfLaps = 57;
            Length = 5.412M;
            RaceDistance = 308.238M;
            LapRecord = "1:31.447 (Pedro de la Rosa 2005)";
        }
    }
}
