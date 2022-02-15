using System;
using F1Manager.Shared.Base;

namespace F1Manager.Teams.Exceptions;

[Serializable]
public class F1ManagerTeamException : F1ManagerException
{
    internal F1ManagerTeamException(TeamErrorCode code, string message, Exception ex = null) : base(code, message, ex)
    {
    }
}