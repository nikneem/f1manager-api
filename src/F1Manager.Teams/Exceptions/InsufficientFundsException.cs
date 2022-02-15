using System;
using F1Manager.Teams.Enums;

namespace F1Manager.Teams.Exceptions;

[Serializable]
public class InsufficientFundsException : F1ManagerTeamException
{
    public TeamComponent Component { get; }
    public Guid ComponentId { get; }
    public string ComponentName { get; }
    public decimal Price { get; }
    public decimal Money { get; }

    internal InsufficientFundsException(TeamComponent component, Guid componentId, string componentName, decimal price, decimal money, Exception ex = null)
        : base(TeamErrorCode.NotEnoughMoney, $"Could not but component '{component}' with ID '{componentId} because the team has insufficient funds (required {price}, owned {money})",
            ex)
    {
        Component = component;
        ComponentId = componentId;
        ComponentName = componentName;
        Price = price;
        Money = money;
    }
}