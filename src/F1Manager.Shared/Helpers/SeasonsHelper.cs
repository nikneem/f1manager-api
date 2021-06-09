using System;

namespace F1Manager.Shared.Helpers
{
    public static class SeasonsHelper
    {
        public static int GetSeasonId()
        {
            return DateTimeOffset.UtcNow.Year;
        }
    }
}