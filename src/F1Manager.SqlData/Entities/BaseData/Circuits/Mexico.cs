using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Mexico : CircuitEntity
    {
        public Mexico()
        {
            Id = Guid.Parse("02cc85e9-0ed5-47dc-9f5f-bcf6949c781b");
            Name = "Autódromo Hermanos Rodríguez";
            FirstGrandPrix = 1963;
            NumberOfLaps = 71;
            Length = 4.304M;
            RaceDistance = 305.354M;
            LapRecord = "1:18.741 (Valtteri Bottas 2018)";
        }
    }
}
