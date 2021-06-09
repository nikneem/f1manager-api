using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class SaoPaulo : CircuitEntity
    {
        public SaoPaulo()
        {
            Id = Guid.Parse("154e4b28-8317-4c07-8e7b-2420eead6bd4");
            Name = "Autódromo José Carlos Pace";
            FirstGrandPrix = 1973;
            NumberOfLaps = 71;
            Length = 4.309M;
            RaceDistance = 305.909M;
            LapRecord = "1:10.540 (Valtteri Bottas 2018)";
        }
    }
}
