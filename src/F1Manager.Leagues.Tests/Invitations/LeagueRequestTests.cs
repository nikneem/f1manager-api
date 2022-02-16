using System;
using F1Manager.Leagues.DomainModels;
using F1Manager.Leagues.Exceptions;
using Xunit;

namespace F1Manager.Leagues.Tests.Requests;

public class LeagueRequestTests
{
    [Fact]
    public void WhenRequestWasSent_TheLeagueAndTeamMatch()
    {
        var leagueId = Guid.NewGuid();
        var teamId = Guid.NewGuid();
        var leagueRequest = new LeagueRequest(leagueId, teamId);
        Assert.Equal(leagueRequest.LeagueId, leagueId);
        Assert.Equal(leagueRequest.TeamId, teamId);
        Assert.InRange(leagueRequest.CreatedOn, DateTimeOffset.UtcNow.AddSeconds(-1), DateTimeOffset.UtcNow);
        Assert.InRange(leagueRequest.ExpiresOn, DateTimeOffset.UtcNow.AddDays(7).AddSeconds(-1), DateTimeOffset.UtcNow.AddDays(7));
    }
    [Fact]
    public void WhenRequestWasSent_ItCanBeAccepted()
    {
        var domainModel = new LeagueRequest(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Accept();
        Assert.NotNull(domainModel.AcceptedOn);
    }
    [Fact]
    public void WhenRequestWasSent_ItCanBeDeclined()
    {
        var domainModel = new LeagueRequest(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Decline();
        Assert.NotNull(domainModel.DeclinedOn);
    }
    [Fact]
    public void WhenRequestWasAccepted_IsCannotBeAccepted()
    {
        var domainModel = new LeagueRequest(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Accept();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Accept());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.RequestAlreadyAccepted);
    }
    [Fact]
    public void WhenRequestWasAccepted_IsCannotBeDeclined()
    {
        var domainModel = new LeagueRequest(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Accept();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Decline());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.RequestAlreadyAccepted);
    }
    [Fact]
    public void WhenRequestWasDeclined_IsCannotBeAccepted()
    {
        var domainModel = new LeagueRequest(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Decline();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Accept());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.RequestAlreadyDeclined);
    }
    [Fact]
    public void WhenRequestWasDeclined_IsCannotBeDeclined()
    {
        var domainModel = new LeagueRequest(Guid.NewGuid(), Guid.NewGuid());
        domainModel.Decline();
        var exception = Assert.Throws<F1ManagerLeaguesException>(() => domainModel.Decline());
        Assert.Equal(exception.ErrorCode, LeagueErrorCode.RequestAlreadyDeclined);
    }

}