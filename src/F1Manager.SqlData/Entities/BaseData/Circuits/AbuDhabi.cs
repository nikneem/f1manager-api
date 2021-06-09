using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class AbuDhabi : CircuitEntity
    {
        public AbuDhabi()
        {
            Id = Guid.Parse("f54bce76-5e6c-4e1c-bb3e-ef5c81a38e70");
            Name = "Yas Marina Circuit";
            FirstGrandPrix = 2009;
            NumberOfLaps = 55;
            Length = 5.554M;
            RaceDistance = 305.355M;
            LapRecord = "1:39.283 (Lewis Hamilton 2019)";
        }
    }
}
