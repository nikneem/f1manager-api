using System;
using F1Manager.Leagues.DomainModels;
using F1Manager.Leagues.Exceptions;
using Xunit;

namespace F1Manager.Leagues.Tests.Invitations;

public class LeagueInvitationTests
{
    [Fact]
    public void WhenInvitationWasSent_TheLeagueAndTeamMatch()
    {
        var leagueId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var leagueInvitation = new LeagueInvitation(leagueId, teamId);
        Assert.Equal(leagueInvitation.LeagueId, leagueId);
        Assert.Equal(leagueInvitation.TeamId, teamId);
        Assert.InRange(leagueInvitation.CreatedOn, DateTimeOffset.UtcNow.AddSeconds(-1), DateTimeOffset.UtcNow);
        Assert.InRange(leagueInvitation.ExpiresOn, DateTimeOffset.UtcNow.AddDays(7).AddSeconds(-1), DateTimeOffset.UtcNow.AddDays(7));
    }
    [Fact]
    public void WhenInvitationWasSent_ItCanBeAccepted()
    {
        var domainModel = new LeagueInvitation(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Accept();
        Assert.NotNull(domainModel.AcceptedOn);
    }
    [Fact]
    public void WhenInvitationWasSent_ItCanBeDeclined()
    {
        var domainModel = new LeagueInvitation(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Decline();
        Assert.NotNull(domainModel.DeclinedOn);
    }
    [Fact]
    public void WhenInvitationWasAccepted_IsCannotBeAccepted()
    {
        var domainModel = new LeagueInvitation(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Accept();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Accept());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.InvitationAlreadyAccepted);
    }
    [Fact]
    public void WhenInvitationWasAccepted_IsCannotBeDeclined()
    {
        var domainModel = new LeagueInvitation(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Accept();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Decline());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.InvitationAlreadyAccepted);
    }
    [Fact]
    public void WhenInvitationWasDeclined_IsCannotBeAccepted()
    {
        var domainModel = new LeagueInvitation(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Decline();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Accept());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.InvitationAlreadyDeclined);
    }
    [Fact]
    public void WhenInvitationWasDeclined_IsCannotBeDeclined()
    {
        var domainModel = new LeagueInvitation(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Decline();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Decline());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.InvitationAlreadyDeclined);
    }
}