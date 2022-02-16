using System;
using F1Manager.Shared.Enums;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.DataTransferObjects;
using F1Manager.Teams.Domain;
using F1Manager.Teams.Exceptions;
using F1Manager.Teams.UnitTests.Factories;
using Moq;
using Xunit;

namespace F1Manager.Teams.UnitTests.Domain
{
    public class TeamDomainTests
    {

        private readonly Mock<ITeamsDomainService> _teamsDomainServiceMock;

        [Fact]
        public async void WhenTeamNameIsValid_ItSucceeds()
        {
            var expectedTeamName = "Any Valid Random F1 Team Name";
            var team = WithRandomTeam();
            WhenTeamNameIsUnique(team.Id, expectedTeamName);
            await team.SetName(expectedTeamName, _teamsDomainServiceMock.Object);
            Assert.Equal(expectedTeamName, team.Name);
            Assert.Equal(TrackingState.Modified, team.TrackingState);
        }

        [Theory]
        [InlineData("")]
        [InlineData(" ")]
        [InlineData(null)]
        public async void WhenTeamNameIsNullOrEmpty_ItThrowsF1ManagerTeamException(string name)
        {
            var team = WithRandomTeam();
            var exception = await Assert.ThrowsAsync<F1ManagerTeamException>(() =>
                team.SetName(name, _teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.NameNullOrEmpty, exception.ErrorCode);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }

        [Theory]
        [InlineData("Ban$nas")]
        [InlineData("Fa%wakas")]
        [InlineData("Dream;Team")]
        public async void WhenTeamNameIsInvalid_ItThrowsF1ManagerTeamException(string name)
        {
            var team = WithRandomTeam();
            var exception = await Assert.ThrowsAsync<F1ManagerTeamException>(() =>
                team.SetName(name, _teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.InvalidName, exception.ErrorCode);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamNameIsAlreadyTaken_ItThrowsF1ManagerTeamException()
        {
            var expectedTeamName = "Any Valid Random F1 Team Name";
            var team = WithRandomTeam();
            WhenTeamAlreadyExists(team.Id, expectedTeamName);
            var exception = await Assert.ThrowsAsync<F1ManagerTeamException>(() =>
                team.SetName(expectedTeamName, _teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.NameAlreadyTaken, exception.ErrorCode);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }



        [Fact]
        public async void WhenTeamBuysFirstDriver_ItSucceeds()
        {
            var team = WithRandomTeam();
            var driver = WithDriver();
            var expectedMoney = team.Money - driver.Value;
            await team.BuyFirstDriver(driver.Id, _teamsDomainServiceMock.Object);

            Assert.NotNull(team.FirstDriver);
            Assert.Equal(team.FirstDriver.BoughtFor, driver.Value);
            Assert.Equal(expectedMoney, team.Money);
            Assert.Equal(TrackingState.Modified, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamBuysSecondDriver_ItSucceeds()
        {
            var team = WithRandomTeam();
            var driver = WithDriver();
            var expectedMoney = team.Money - driver.Value;
            await team.BuySecondDriver(driver.Id, _teamsDomainServiceMock.Object);

            Assert.NotNull(team.SecondDriver);
            Assert.Equal(driver.Value, team.SecondDriver.BoughtFor);
            Assert.Equal(expectedMoney, team.Money);
            Assert.Equal(TrackingState.Modified, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamBuysTwoFirstDrivers_ItThrowsComponentAlreadyOwnedException()
        {
            var team = WithRandomTeam();
            var firstDriver = WithDriver();
            var secondDriver = WithDriver();

            await team.BuyFirstDriver(firstDriver.Id, _teamsDomainServiceMock.Object);
            var exception = await Assert.ThrowsAsync<ComponentAlreadyOwnedException>(() =>
                team.BuyFirstDriver(secondDriver.Id, _teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.ComponentAlreadyFilled, exception.ErrorCode);
        }

        [Fact]
        public async void WhenTeamBuysTwoSecondDrivers_ItThrowsComponentAlreadyOwnedException()
        {
            var team = WithRandomTeam();
            var firstDriver = WithDriver();
            var secondDriver = WithDriver();

            await team.BuySecondDriver(firstDriver.Id, _teamsDomainServiceMock.Object);
            var exception = await Assert.ThrowsAsync<ComponentAlreadyOwnedException>(() =>
                team.BuySecondDriver(secondDriver.Id, _teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.ComponentAlreadyFilled, exception.ErrorCode);
        }

        [Fact]
        public async void WhenTeamBuysTooExpensiveDriver_ItThrowsInsufficientFundsException()
        {
            var team = WithRandomTeam();
            var driver = WithDriver(120000001);
            var exception = await Assert.ThrowsAsync<InsufficientFundsException>(() =>
                team.BuyFirstDriver(driver.Id, _teamsDomainServiceMock.Object));
            Assert.Equal(team.Money, exception.Money);
            Assert.Equal(driver.Value, exception.Price);
            Assert.Equal(driver.Id, exception.ComponentId);
            Assert.Equal(driver.Name, exception.ComponentName);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamBuysUnknownDriver_ItThrowsInsufficientFundsException()
        {
            var team = WithRandomTeam();
            var driver = WithDriver();
            driver.Id = Guid.NewGuid();
            var exception = await Assert.ThrowsAsync<TeamComponentNotFoundException>(() =>
                team.BuyFirstDriver(driver.Id, _teamsDomainServiceMock.Object));
            Assert.Equal(driver.Id, exception.ComponentId);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamBuysEngine_ItSucceeds()
        {
            var team = WithRandomTeam();
            var engine = WithEngine();

            var expectedMoney = team.Money - engine.Value;
            await team.BuyEngine(engine.Id, _teamsDomainServiceMock.Object);

            Assert.NotNull(team.Engine);
            Assert.Equal(team.Engine.BoughtFor, engine.Value);
            Assert.Equal(expectedMoney, team.Money);
            Assert.Equal(TrackingState.Modified, team.TrackingState);
        }
        [Fact]
        public async void WhenTeamBuysChassis_ItSucceeds()
        {
            var team = WithRandomTeam();
            var chassis = WithChassis();

            var expectedMoney = team.Money - chassis.Value;
            await team.BuyChassis(chassis.Id, _teamsDomainServiceMock.Object);

            Assert.NotNull(team.Chassis);
            Assert.Equal(team.Chassis.BoughtFor, chassis.Value);
            Assert.Equal(expectedMoney, team.Money);
            Assert.Equal(TrackingState.Modified, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamBuysTooExpensiveEngine_ItSucceeds()
        {
            var team = WithRandomTeam();
            var engine = WithEngine(120000001);

            var exception = await Assert.ThrowsAsync<InsufficientFundsException>(() =>
                team.BuyEngine(engine.Id, _teamsDomainServiceMock.Object));
            Assert.Equal(team.Money, exception.Money);
            Assert.Equal(engine.Value, exception.Price);
            Assert.Equal(engine.Id, exception.ComponentId);
            Assert.Equal(engine.Name, exception.ComponentName);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamBuysTooExpensiveChassis_ItSucceeds()
        {
            var team = WithRandomTeam();
            var chassis = WithChassis(120000001);

            var exception = await Assert.ThrowsAsync<InsufficientFundsException>(() =>
                team.BuyChassis(chassis.Id, _teamsDomainServiceMock.Object));
            Assert.Equal(team.Money, exception.Money);
            Assert.Equal(chassis.Value, exception.Price);
            Assert.Equal(chassis.Id, exception.ComponentId);
            Assert.Equal(chassis.Name, exception.ComponentName);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }

        [Fact]
        public async void WhenTeamBuysTwoEngines_ItThrowsComponentAlreadyOwnedException()
        {
            var team = WithRandomTeam();
            var engine = WithEngine();
            var secondEngine = WithEngine();
            await team.BuyEngine(engine.Id, _teamsDomainServiceMock.Object);
            var exception = await Assert.ThrowsAsync<ComponentAlreadyOwnedException>(() =>
                team.BuyEngine(secondEngine.Id, _teamsDomainServiceMock.Object));
        }
        [Fact]
        public async void WhenTeamBuysTwoChassis_ItThrowsComponentAlreadyOwnedException()
        {
            var team = WithRandomTeam();
            var chassis = WithChassis();
            var secondChassis = WithChassis();
            await team.BuyChassis(chassis.Id, _teamsDomainServiceMock.Object);
            var exception = await Assert.ThrowsAsync<ComponentAlreadyOwnedException>(() =>
                team.BuyChassis(secondChassis.Id, _teamsDomainServiceMock.Object));
        }

        [Fact]
        public async void WhenTeamSellsUnposessedFirstDriver_ItThrowsF1ManagerTeamException()
        {
            var team = WithRandomTeam();
            var exception = await Assert.ThrowsAsync< F1ManagerTeamException >(() =>team.SellFirstDriver(_teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.InvalidTransfer, exception.ErrorCode);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }
        [Fact]
        public async void WhenTeamSellsUnposessedSecondDriver_ItThrowsF1ManagerTeamException()
        {
            var team = WithRandomTeam();
            var exception = await Assert.ThrowsAsync<F1ManagerTeamException>(() => team.SellSecondDriver(_teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.InvalidTransfer, exception.ErrorCode);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }
        [Fact]
        public async void WhenTeamSellsUnposessedEngine_ItThrowsF1ManagerTeamException()
        {
            var team = WithRandomTeam();
            var exception = await Assert.ThrowsAsync<F1ManagerTeamException>(() => team.SellEngine(_teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.InvalidTransfer, exception.ErrorCode);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }
        [Fact]
        public async void WhenTeamSellsUnposessedChassis_ItThrowsF1ManagerTeamException()
        {
            var team = WithRandomTeam();
            var exception = await Assert.ThrowsAsync<F1ManagerTeamException>(() => team.SellChassis(_teamsDomainServiceMock.Object));
            Assert.Equal(TeamErrorCode.InvalidTransfer, exception.ErrorCode);
            Assert.Equal(TrackingState.Pristine, team.TrackingState);
        }


        private Team WithRandomTeam()
        {
            return TeamsFactory.Random();
        }

        private void WhenTeamNameIsUnique(Guid id, string name)
        {
            _teamsDomainServiceMock.Setup(svc => svc.IsUniqueName(id, name))
                .ReturnsAsync(true);
        }

        private void WhenTeamAlreadyExists(Guid id, string name)
        {
            _teamsDomainServiceMock.Setup(svc => svc.IsUniqueName(id, name))
                .ReturnsAsync(false);
        }

        private DriverDto WithDriver(decimal? price = null)
        {
            var driver = TeamsFactory.RandomDriver();
            driver.Value = price ?? driver.Value;
            _teamsDomainServiceMock.Setup(x => x.GetDriverById(driver.Id))
                .ReturnsAsync(driver);
            return driver;
        }

        private EngineDto WithEngine(decimal? price = null)
        {
            var engine = TeamsFactory.RandomEngine();
            engine.Value = price ?? engine.Value;
            _teamsDomainServiceMock.Setup(x => x.GetEngineById(engine.Id))
                .ReturnsAsync(engine);
            return engine;
        }

        private ChassisDto WithChassis(decimal? price = null)
        {
            var chassis = TeamsFactory.RandomChassis();
            chassis.Value = price ?? chassis.Value;
            _teamsDomainServiceMock.Setup(x => x.GetChassisById(chassis.Id))
                .ReturnsAsync(chassis);
            return chassis;
        }


        public TeamDomainTests()
        {
            _teamsDomainServiceMock = new Mock<ITeamsDomainService>();
        }

    }
}