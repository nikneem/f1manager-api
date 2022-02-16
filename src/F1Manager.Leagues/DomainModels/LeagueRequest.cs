using System;
using F1Manager.Leagues.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Enums;

namespace F1Manager.Leagues.DomainModels;

public class LeagueRequest : ValueObject
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
        AssertNotExpired();
        DeclinedOn = DateTimeOffset.UtcNow;
        ExpiresOn = DateTimeOffset.UtcNow;
        SetState(TrackingState.Modified);
    }

    private void AssertNotAccepted()
    {
        if (AcceptedOn.HasValue)
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.RequestAlreadyAccepted,
                "This join request has already been accepted");
        }
    }

    private void AssertNotDeclined()
    {
        if (DeclinedOn.HasValue)
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.RequestAlreadyDeclined,
                "This join request has already been declined");
        }
    }
    private void AssertNotExpired()
    {
        if (ExpiresOn.CompareTo(DateTimeOffset.UtcNow) < 0)
        {
            throw new F1ManagerLeaguesException(LeagueErrorCode.RequestExpired,
                "The request has expired");
        }
    }

    public LeagueRequest(
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

    public LeagueRequest(
        Guid league,
        Guid team) : base(TrackingState.New)
    {
        LeagueId = league;
        TeamId = team;
        CreatedOn = DateTimeOffset.UtcNow;
        ExpiresOn = DateTimeOffset.UtcNow.AddDays(7);
    }
}