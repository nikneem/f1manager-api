using System;
using F1Manager.Teams.Enums;

namespace F1Manager.Teams.Exceptions;

[Serializable]
public class TeamComponentNotFoundException : F1ManagerTeamException
{
    public Guid ComponentId { get; }

    internal TeamComponentNotFoundException(TeamComponent component, Guid componentId, Exception ex = null)
        : base(TeamErrorCode.ComponentNotFound, $"The component '{component}' with ID '{componentId}' could not be found",
            ex)
    {
        ComponentId = componentId;
    }
}