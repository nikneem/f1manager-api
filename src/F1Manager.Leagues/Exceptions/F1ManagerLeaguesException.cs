using System;
using F1Manager.Shared.Base;

namespace F1Manager.Leagues.Exceptions
{
    public class F1ManagerLeaguesException : F1ManagerException
    {
        public  F1ManagerLeaguesException(LeagueErrorCode code, string message, Exception ex = null) : base(code, message, ex)
        {
        }
    }
}
