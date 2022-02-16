using System;
using F1Manager.Leagues.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;

namespace F1Manager.Leagues.DomainModels;

/// <summary>
/// A league invitation is an invitation by one of the league administrators to a
/// not-yet member, asking to join their league
/// </summary>
public class LeagueInvitation : ValueObject
{

    public Guid LeagueId { get; }
    public Guid TeamId { get; }
    public DateTimeOffset? AcceptedOn { get; private set; }
    public DateTimeOffset? DeclinedOn { get; private set; }
    public DateTimeOffset CreatedOn { get; }
    public DateTimeOffset ExpiresOn { get; private set; }

    public void Accept()
    {
        AssertNotAccepted();
        AssertNotDeclined();
        AssertNotExpired();
        AcceptedOn = DateTimeOffset.UtcNow;
        ExpiresOn = DateTimeOffset.UtcNow;
        SetState(TrackingState.Modified);
    }

    public void Decline()
    {
        AssertNotAccepted();
        AssertNotDeclined();
        DeclinedOn = DateTimeOffset.UtcNow;
        ExpiresOn = DateTimeOffset.UtcNow;
        SetState(TrackingState.Modified);
    }

    private void AssertNotAccepted()
    {
        if (AcceptedOn.HasValue)
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.InvitationAlreadyAccepted,
                "This invitation has already been accepted");
        }
    }

    private void AssertNotDeclined()
    {
        if (DeclinedOn.HasValue)
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.InvitationAlreadyDeclined,
                "This invitation has already been declined");
        }
    }

    private void AssertNotExpired()
    {
        if (ExpiresOn.CompareTo(DateTimeOffset.UtcNow)<0)
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.InvitationExpired,
                "The invitation has expired");
        }
    }

    public LeagueInvitation(
        Guid league,
        Guid team,
        DateTimeOffset? accepted,
        DateTimeOffset? declined,
        DateTimeOffset created,
        DateTimeOffset expires)
    {
        LeagueId = league;
        TeamId = team;
        AcceptedOn = accepted;
        DeclinedOn = declined;
        CreatedOn = created;
        ExpiresOn = expires;
    }

    public LeagueInvitation(
        Guid league,
        Guid team) : base(TrackingState.New)
    {
        LeagueId = league;
        TeamId = team;
        CreatedOn = DateTimeOffset.UtcNow;
        ExpiresOn = DateTimeOffset.UtcNow.AddDays(7);
    }
}