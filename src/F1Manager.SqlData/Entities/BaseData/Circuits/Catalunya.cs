using System;

namespace F1Manager.SqlData.Entities.BaseData.Circuits
{
    public class Catalunya : CircuitEntity
    {

        public Catalunya()
        {
            Id = Guid.Parse("a7cc32a4-1b87-418b-aec2-98852d0428dd");
            Name = "Circuit de Barcelona-Catalunya";
            FirstGrandPrix = 1991;
            NumberOfLaps = 66;
            Length = 4.655M;
            RaceDistance = 307.104M;
            LapRecord = "1:18.183 (Valtteri Bottas 2020)";
        }

    }
}
