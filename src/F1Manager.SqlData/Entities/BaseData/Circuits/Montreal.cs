using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Montreal : CircuitEntity
    {
        public Montreal()
        {
            Id = Guid.Parse("73571b1e-cfc4-44ba-899c-41f438732444");
            Name = "Circuit Gilles-Villeneuve";
            FirstGrandPrix = 1978;
            NumberOfLaps = 70;
            Length = 4.361M;
            RaceDistance = 305.27M;
            LapRecord = "1:13.078 (Valtteri Bottas 2019)";
        }
    }
}
