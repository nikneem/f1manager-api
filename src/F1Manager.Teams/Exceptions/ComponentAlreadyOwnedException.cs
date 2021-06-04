using System;
using F1Manager.Teams.Enums;

namespace F1Manager.Teams.Exceptions
{
    public class ComponentAlreadyOwnedException : F1ManagerTeamException
    {
        internal ComponentAlreadyOwnedException(TeamComponent component, Exception ex = null)
            : base(TeamErrorCode.ComponentAlreadyFilled, $"Your team already contains a {component}",
                ex)
        {
        }
    }
}