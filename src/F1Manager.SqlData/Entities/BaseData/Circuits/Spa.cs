using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Spa : CircuitEntity
    {
        public Spa()
        {
            Id = Guid.Parse("8daf99b5-cac5-4d82-a497-db1148b7a1f5");
            Name = "Circuit de Spa-Francorchamps";
            FirstGrandPrix = 1950;
            NumberOfLaps = 44;
            Length = 7.004M;
            RaceDistance = 308.052M;
            LapRecord = "1:46.286 (Valtteri Bottas 2018)";
        }
    }
}
