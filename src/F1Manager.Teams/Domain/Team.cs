using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.Enums;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.DataTransferObjects;
using F1Manager.Teams.Enums;
using F1Manager.Teams.Exceptions;

namespace F1Manager.Teams.Domain
{
    public sealed class Team : DomainModel<Guid>
    {
        public int SeasonId { get; }
        public Guid OwnerId { get; }
        public string Name { get; private set; }
        public int Points { get; private set; }
        public decimal Money { get; private set; }
        public bool IsPublic { get; private set; }
        public TeamDriver FirstDriver { get; private set; }
        public TeamDriver SecondDriver { get; private set; }
        public TeamEngine Engine { get; private set; }
        public TeamChassis Chassis { get; private set; }

        public async Task SetName(string value, ITeamsDomainService domainService)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new F1ManagerTeamException(TeamErrorCode.NameNullOrEmpty,
                    "The team name cannot be null or empty");
            }
            if (!Regex.IsMatch(value, RegularExpressions.Teamname))
            {
                throw new F1ManagerTeamException(TeamErrorCode.InvalidName,
                    $"Invalid name '{value}': The name can only contain alphanumeric characters and spaces and have a length between 1 and 100 characters");
            }
            if (!await domainService.IsUniqueName(Id, value))
            {
                throw new F1ManagerTeamException(TeamErrorCode.NameAlreadyTaken,
                    $"The name '{value}' is already taken");
            }

            if (!Equals(Name, value))
            {
                Name = value;
                SetState(TrackingState.Modified);
            }
        }

        public async Task BuyFirstDriver(Guid id, ITeamsDomainService domainService)
        {
            if (FirstDriver != null)
            {
                throw new ComponentAlreadyOwnedException(TeamComponent.FirstDriver);
            }

            var driver = await domainService.GetDriverById(id);
            if (driver == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.FirstDriver, id);
            }

            if (SecondDriver != null && SecondDriver.DriverId.Equals(id))
            {
                throw new F1ManagerTeamException(TeamErrorCode.DriverAlreadyInTeam, "You already own this driver");
            }

            if (driver.Value > Money)
            {
                throw new InsufficientFundsException(TeamComponent.FirstDriver,
                    driver.Id,
                    driver.Name,
                    driver.Value,
                    Money);
            }

            var teamDriver = new TeamDriver(driver.Id,  driver.Value, true);
            FirstDriver = teamDriver;
            Money -= driver.Value;
            SetState(TrackingState.Modified);
        }
        public async Task BuySecondDriver(Guid id, ITeamsDomainService domainService)
        {
            if (SecondDriver != null)
            {
                throw new ComponentAlreadyOwnedException(TeamComponent.SecondDriver);
            }

            var driver = await domainService.GetDriverById(id);
            if (driver == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.SecondDriver, id);
            }

            if (FirstDriver != null && FirstDriver.DriverId.Equals(id))
            {
                throw new F1ManagerTeamException(TeamErrorCode.DriverAlreadyInTeam, "You already own this driver");
            }


            if (driver.Value > Money)
            {
                throw new InsufficientFundsException(TeamComponent.SecondDriver,
                    driver.Id,
                    driver.Name,
                    driver.Value,
                    Money);
            }

            var teamDriver = new TeamDriver(driver.Id,  driver.Value, false);
            SecondDriver = teamDriver;
            Money -= driver.Value;
            SetState(TrackingState.Modified);
        }
        public async Task<EngineDto> BuyEngine(Guid id, ITeamsDomainService domainService)
        {
            if (Engine != null)
            {
                throw new ComponentAlreadyOwnedException(TeamComponent.Engine);
            }

            var engine = await domainService.GetEngineById(id);
            if (engine == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.Engine, id);
            }

            if (engine.Value > Money)
            {
                throw new InsufficientFundsException(TeamComponent.Engine,
                    engine.Id,
                    engine.Name,
                    engine.Value,
                    Money);
            }

            var teamEngine = new TeamEngine(engine.Id,  engine.Value);
            Engine = teamEngine;
            Money -= engine.Value;
            SetState(TrackingState.Modified);
            return engine;
        }
        public async Task<ChassisDto> BuyChassis(Guid id, ITeamsDomainService domainService)
        {
            if (Chassis != null)
            {
                throw new ComponentAlreadyOwnedException(TeamComponent.Chassis);
            }

            var chassis = await domainService.GetChassisById(id);
            if (chassis == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.Chassis, id);
            }

            if (chassis.Value > Money)
            {
                throw new InsufficientFundsException(TeamComponent.Chassis,
                    chassis.Id,
                    chassis.Name,
                    chassis.Value,
                    Money);
            }

            var teamChassis = new TeamChassis(chassis.Id,  chassis.Value);
            Chassis = teamChassis;
            Money -= chassis.Value;
            SetState(TrackingState.Modified);
            return chassis;
        }

        public async Task SellFirstDriver(ITeamsDomainService domainService)
        {
            if (FirstDriver == null)
            {
                throw new F1ManagerTeamException(TeamErrorCode.InvalidTransfer, "Team does not own this component");
            }

            var driver = await domainService.GetDriverById(FirstDriver.DriverId);
            if (driver == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.FirstDriver, FirstDriver.DriverId);
            }

            Money += FirstDriver.Sell(driver.Value);
            SetState(TrackingState.Modified);
        }
        public async Task SellSecondDriver(ITeamsDomainService domainService)
        {
            if (SecondDriver == null)
            {
                throw new F1ManagerTeamException(TeamErrorCode.InvalidTransfer, "Team does not own this component");
            }

            var driver = await domainService.GetDriverById(SecondDriver.DriverId);
            if (driver == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.SecondDriver, FirstDriver.DriverId);
            }

            Money += SecondDriver.Sell(driver.Value);
            SetState(TrackingState.Modified);
        }
        public async Task SellEngine(ITeamsDomainService domainService)
        {
            if (Engine == null)
            {
                throw new F1ManagerTeamException(TeamErrorCode.InvalidTransfer, "Team does not own this component");
            }

            var engine = await domainService.GetEngineById(Engine.EngineId);
            if (engine == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.Engine, Engine.EngineId);
            }

            Money += Engine.Sell(engine.Value);
            SetState(TrackingState.Modified);
        }
        public async Task SellChassis(ITeamsDomainService domainService)
        {
            if (Chassis == null)
            {
                throw new F1ManagerTeamException(TeamErrorCode.InvalidTransfer, "Team does not own this component");
            }

            var chassis = await domainService.GetEngineById(Chassis.ChassisId);
            if (chassis == null)
            {
                throw new TeamComponentNotFoundException(TeamComponent.Chassis, Chassis.ChassisId);
            }

            Money += Chassis.Sell(chassis.Value);
            SetState(TrackingState.Modified);
        }

        public Team(Guid id, 
            int seasonId, 
            Guid ownerId,
            string name, 
            int points,
            decimal money,
            TeamDriver firstDriver,
            TeamDriver secondDriver,
            TeamEngine engine,
            TeamChassis chassis
            ) : base(id)
        {
            SeasonId = seasonId;
            OwnerId = ownerId;
            Name = name;
            Points = points;
            Money = money;
            FirstDriver = firstDriver;
            SecondDriver = secondDriver;
            Engine = engine;
            Chassis = chassis;
        }
        private Team(int seasonId, Guid ownerId) : base(Guid.NewGuid(), TrackingState.New)
        {
            SeasonId = seasonId;
            OwnerId = ownerId;
            Points = 0;
            Money = 120000000M;
        }

        public static async Task<Team> Create(int seasonId, Guid ownerId, string name, ITeamsDomainService service) 
        {
            var team = new Team(seasonId, ownerId);
            await team.SetName(name, service);
            return team;
        }
    }
}