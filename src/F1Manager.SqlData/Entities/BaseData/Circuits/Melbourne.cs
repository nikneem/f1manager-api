using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Melbourne : CircuitEntity
    {
        public Melbourne()
        {
            Id = Guid.Parse("4d7be8b6-4f61-4031-8d48-8551015b6986");
            Name = "Melbourne Grand Prix Circuit";
            FirstGrandPrix = 1996;
            NumberOfLaps = 58;
            Length = 5.303M;
            RaceDistance = 307.574M;
            LapRecord = "1:24.125 (Michael Schumacher 2004)";
        }
    }
}
