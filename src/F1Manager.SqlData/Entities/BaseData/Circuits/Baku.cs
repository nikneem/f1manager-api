using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Baku : CircuitEntity
    {
        public Baku()
        {
            Id = Guid.Parse("cae36e91-9df2-4d7f-86ab-06dfe988048e");
            Name = "Baku City Circuit";
            FirstGrandPrix = 2016;
            NumberOfLaps = 51;
            Length = 6.003M;
            RaceDistance = 306.049M;
            LapRecord = "1:43.009 (Charles Leclerc 2019)";
        }
    }
}
