﻿using System;
using System.Threading.Tasks;
using F1Manager.Shared.DataTransferObjects;
using F1Manager.Shared.Helpers;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.DataTransferObjects;
using F1Manager.Teams.Domain;
using F1Manager.Teams.Exceptions;
using Microsoft.Extensions.Logging;
using Microsoft.OData.Edm;

namespace F1Manager.Teams.Services
{
    public class TeamsService
    {
        private readonly ITeamsRepository _teamsRepository;
        private readonly ITeamsDomainService _domainService;
        private readonly ILogger<TeamsService> _logger;

        public Task<CollectionResult<TeamListItemDto>> List( TeamsListFilterDto filter)
        {
            return _teamsRepository.GetList(SeasonsHelper.GetSeasonId(), filter);
        }
        public async Task<TeamDetailsDto> Create(Guid userId, TeamCreateDto dto)
        {
            var season = SeasonsHelper.GetSeasonId();
            if (await _teamsRepository.GetUserHasTeam(SeasonsHelper.GetSeasonId(), userId))
            {
                throw new F1ManagerTeamException(TeamErrorCode.UserAlreadyHasTeam,
                    $"Cannot create a new team for user {userId}, this player already has a team in season {season}");
            }

            var team =  await Team.Create(season, userId, dto.Name, _domainService);

            if (await _teamsRepository.Create(team))
            {
                return new TeamDetailsDto
                {
                    Id = team.Id,
                    Name = team.Name,
                    Points = team.Points,
                    Money = team.Money
                };
            }

            throw new F1ManagerTeamException(TeamErrorCode.PersistenceFailed, $"Failed to create new team {dto.Name}");
        }
        public async Task<bool> IsUniqueName( Guid userId, string name)
        {
            var teamId = await _teamsRepository.GetTeamId(SeasonsHelper.GetSeasonId(), userId);
            return await _teamsRepository.IsUniqueName(teamId.GetValueOrDefault(), name);
        }
        public async Task<TeamDetailsDto> GetByPlayer( Guid userId)
        {
            var teamId = await GetTeamId(SeasonsHelper.GetSeasonId(), userId);
            return await GetById(teamId.GetValueOrDefault(), userId);
        }
        public async Task<TeamDetailsDto> GetById(Guid teamId, Guid userId)
        {
            var team = await _teamsRepository.Get(teamId);
            return DomainModelToDto(team, userId);
        }
        public async Task<TeamDetailsDto> Update(Guid userId, Guid id, TeamUpdateDto dto)
        {
            var team = await _teamsRepository.Get(id);
            if (!team.OwnerId.Equals(userId))
            {
                throw new F1ManagerTeamException(TeamErrorCode.NotYourTeam,
                    $"Team {id} is not your team, you cannot change properties of this team");
            }

            if (!string.IsNullOrWhiteSpace(dto.Name))
            {
                await team.SetName(dto.Name, _domainService);
            }

            //if (dto.IsPublic.HasValue)
            //{
            //    team.SetPublic(dto.IsPublic.Value);
            //}

            if (await _teamsRepository.Update(team))
            {
                return DomainModelToDto(team, userId);
            }

            throw new F1ManagerTeamException(TeamErrorCode.PersistenceFailed, "Team was updated, but persisting changes failed");
        }

        #region [ Team Driver Transactions ]

        public async Task<TeamDriverDetailsDto> GetFirstDriver(Guid teamId)
        {
            _logger.LogInformation("Getting details for team first driver {teamId}", teamId);
            var team = await _teamsRepository.Get(teamId);

            if (team?.FirstDriver != null)
            {
                var driver = await _domainService.GetDriverById(team.FirstDriver.DriverId);
                _logger.LogInformation("Team first driver was found, returning detailed information");

                return new TeamDriverDetailsDto
                {
                    Id = team.Id,
                    Name = team.FirstDriver.Name,
                    PictureUrl = team.Engine.PictureUrl,
                    BoughtOn = team.Engine.BoughtOn,
                    BoughtFor = team.Engine.BoughtFor,
                    CurrentPrice = driver.Value,
                    PointsGained = 0, 
                    IsFirstDriver=true
                };
            }

            _logger.LogInformation("No first driver found for team {teamId}", teamId);
            return null;
        }
        public async Task<TeamDriverDetailsDto> GetSecondDriver(Guid teamId)
        {
            _logger.LogInformation("Getting details for team second driver {teamId}", teamId);
            var team = await _teamsRepository.Get(teamId);

            if (team?.SecondDriver != null)
            {
                var driver = await _domainService.GetDriverById(team.FirstDriver.DriverId);
                _logger.LogInformation("Team second driver was found, returning detailed information");

                return new TeamDriverDetailsDto
                {
                    Id = team.Id,
                    Name = team.SecondDriver.Name,
                    PictureUrl = team.SecondDriver.PictureUrl,
                    BoughtOn = team.SecondDriver.BoughtOn,
                    BoughtFor = team.SecondDriver.BoughtFor,
                    CurrentPrice = driver.Value,
                    PointsGained = 0,
                    IsFirstDriver = false
                };
            }

            _logger.LogInformation("No second driver found for team {teamId}", teamId);
            return null;
        }
        public async Task<TeamDriverDetailsDto> BuyFirstDriver(Guid userId, Guid driverId)
        {
            _logger.LogInformation("Purchasing driver for player '{userId}'", userId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team != null)
            {
                await team.BuyFirstDriver(driverId, _domainService);
                if (await _teamsRepository.Update(team))
                {
                    _logger.LogInformation("Team {id}, ({name}) succesfully bought driver {driverId} (driverName)",
                        team.Id,
                        team.Name,
                        team.FirstDriver.DriverId,
                        team.FirstDriver.Name);

                    return new TeamDriverDetailsDto
                    {
                        Id = team.FirstDriver.Id,
                        Name = team.FirstDriver.Name,
                        PictureUrl = team.FirstDriver.PictureUrl,
                        BoughtOn = team.FirstDriver.BoughtOn,
                        BoughtFor = team.FirstDriver.BoughtFor,
                        CurrentPrice = team.FirstDriver.BoughtFor,
                        PointsGained = 0,
                    };
                }
            }

            return null;
        }
        public async Task<TeamDriverDetailsDto> BuySecondDriver(Guid userId, Guid driverId)
        {
            _logger.LogInformation("Purchasing driver for player '{userId}'", userId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team != null)
            {
                await team.BuySecondDriver(driverId, _domainService);
                if (await _teamsRepository.Update(team))
                {
                    _logger.LogInformation("Team {id}, ({name}) succesfully bought driver {driverId} (driverName)",
                        team.Id,
                        team.Name,
                        team.FirstDriver.DriverId,
                        team.FirstDriver.Name);

                    return new TeamDriverDetailsDto
                    {
                        Id = team.SecondDriver.Id,
                        Name = team.SecondDriver.Name,
                        PictureUrl = team.SecondDriver.PictureUrl,
                        BoughtOn = team.SecondDriver.BoughtOn,
                        BoughtFor = team.SecondDriver.BoughtFor,
                        CurrentPrice = team.SecondDriver.BoughtFor,
                        PointsGained = 0,
                    };
                }
            }

            return null;
        }
        public async Task<SellConfirmationDto> SellDriverConfirmation(Guid userId, Guid teamDriverId)
        {
            _logger.LogInformation($"Gathering information about the driver to sell for player {userId}", userId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team?.FirstDriver != null || team?.SecondDriver != null)
            {
                var teamDriver = team.FirstDriver?.Id == teamDriverId ? team.FirstDriver : team.SecondDriver;
                if (teamDriver.Id != teamDriverId)
                {
                    _logger.LogWarning(
                        "Incoming request to sell driver {expectedTeamDriverId}, but team doesn't own a driver with that ID",
                        teamDriverId);
                    throw new F1ManagerTeamException(TeamErrorCode.ComponentNotInPosession,
                        $"Team tries to sell driver {teamDriver} but this driver is not in the team");
                }

                var driver = await _domainService.GetDriverById(teamDriver.DriverId);

                _logger.LogInformation("Team driver '{driverName}' gathering information to sell", teamDriver.Name);
                return new SellConfirmationDto
                {
                    Id = teamDriver.Id,
                    Name = teamDriver.Name,
                    CurrentValue = driver.Value,
                    WearOffPercentage = 0,
                    SellingPrice = driver.Value
                };
            }
            _logger.LogInformation("Team not found or team does not own this driver, skipping selling proposal");
            return null;
        }
        public async Task<bool> SellDriver(Guid userId, Guid teamDriverId)
        {
            _logger.LogInformation("Player '{userId}' is going to sell driver {teamDriverId}", userId, teamDriverId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team?.FirstDriver != null || team?.SecondDriver != null)
            {
                var firstDriver = team.FirstDriver?.Id == teamDriverId;
                var teamDriver = firstDriver ? team.FirstDriver : team.SecondDriver;
                var driver = await _domainService.GetDriverById(teamDriver.DriverId);
                if (teamDriver.Id != teamDriverId)
                {
                    _logger.LogWarning(
                        "Incoming request to sell driver {expectedTeamDriverId}, but team doesn't own a driver with that ID",
                        teamDriverId);
                    throw new F1ManagerTeamException(TeamErrorCode.ComponentNotInPosession,
                        $"Team tries to sell driver {teamDriver} but this driver is not in the team");
                }

                _logger.LogInformation(
                    "Selling driver {driverName} with value {currentValue} for '{sellingPrice}'",
                    teamDriver.Name,
                    driver.Value,
                    driver.Value);
                if (firstDriver)
                {
                    await team.SellFirstDriver(_domainService);
                }
                else
                {
                    await team.SellSecondDriver(_domainService);
                }
                var soldSuccessfully = await _teamsRepository.Update(team);
                _logger.LogInformation("Driver sold successfully: {successfully}", soldSuccessfully);
                return soldSuccessfully;
            }
            throw new F1ManagerTeamException(TeamErrorCode.PersistenceFailed, $"Failed to sell team driver with ID {teamDriverId}");
        }

        #endregion

        #region [ Team engine transactions ]

        public async Task<TeamEngineDetailsDto> GetEngine(Guid teamId)
        {
            _logger.LogInformation("Getting details for team engine {teamId}", teamId);
            var team = await _teamsRepository.Get(teamId);

            if (team?.Engine != null)
            {
                var engine = await _domainService.GetEngineById(team.Engine.EngineId);
                _logger.LogInformation("Team engine was found, returning detailed information");

                return new TeamEngineDetailsDto
                {
                    Id = team.Id,
                    Name = team.Engine.Name,
                    Manufacturer = engine.Manufacturer,
                    Model = engine.Model,
                    PictureUrl = team.Engine.PictureUrl,
                    BoughtOn = team.Engine.BoughtOn,
                    BoughtFor = team.Engine.BoughtFor,
                    CurrentPrice = engine.Value,
                    MaxWearOff = engine.MaximumWearOff,
                    PointsGained = 0,
                    WearOff = team.Engine.WarnOffPercentage
                };
            }

            _logger.LogInformation("No engine found for team {teamId}", teamId);
            return null;
        }
        public async Task<TeamEngineDetailsDto> PurchaseEngine(Guid userId, Guid engineId)
        {
            _logger.LogInformation("Purchasing engine for player '{userId}'", userId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team != null)
            {
                var boughtEngine = await team.BuyEngine(engineId,_domainService);
                if (await _teamsRepository.Update(team))
                {
                    _logger.LogInformation("Engine '{engineId}' ({engineName}) successfully bought for team '{teamName}'", engineId, team.Engine.Name, team.Name);

                    return new TeamEngineDetailsDto
                    {
                        Id = team.Engine.Id,
                        Name = team.Engine.Name,
                        Manufacturer = boughtEngine.Manufacturer,
                        Model = boughtEngine.Model,
                        PictureUrl = boughtEngine.PictureUrl,
                        BoughtOn = team.Engine.BoughtOn,
                        BoughtFor = team.Engine.BoughtFor,
                        CurrentPrice = boughtEngine.Value,
                        MaxWearOff = boughtEngine.MaximumWearOff,
                        PointsGained = 0,
                        WearOff = 0
                    };
                }
            }

            return null;
        }
        public async Task<SellConfirmationDto> SellEngineConfirmation(Guid userId, Guid teamEngineId)
        {
            _logger.LogInformation($"Gathering information about the engine to sell for player {userId}", userId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team?.Engine != null)
            {
                if (team.Engine.Id != teamEngineId)
                {
                    _logger.LogWarning(
                        "Incoming request to sell engine {expectedTeamEngineId}, but team owns team engine ID {actualTeamEngineId}",
                        teamEngineId, team.Engine.Id);
                    throw new F1ManagerTeamException(TeamErrorCode.ComponentNotInPosession,
                        $"Team tries to sell engine {teamEngineId} but this engine is not in the team");
                }

                var engine = await _domainService.GetEngineById(team.Engine.EngineId);
                _logger.LogInformation("Team is engine '{engineName}' gathering information to sell", team.Engine.Name);
                return new SellConfirmationDto
                {
                    Id = team.Engine.Id,
                    Name = team.Engine.Name,
                    CurrentValue = engine.Value,
                    WearOffPercentage = team.Engine.WarnOffPercentage,
                    SellingPrice = team.Engine.GetSellingPrice(engine.Value)
                };
            }
            _logger.LogInformation("Team not found or team does not own an engine, skipping selling proposal");
            return null;
        }
        public async Task<bool> SellEngine(Guid userId, Guid teamEngineId)
        {
            _logger.LogInformation("Player '{userId}' is going to sell engine {teamEngineId}", userId,
                teamEngineId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team?.Engine != null)
            {
                if (team.Engine.Id != teamEngineId)
                {
                    _logger.LogWarning(
                        "Incoming request to sell engine {expectedTeamEngineId}, but team owns team engine ID {actualTeamEngineId}",
                        teamEngineId, team.Engine.Id);
                    throw new F1ManagerTeamException(TeamErrorCode.ComponentNotInPosession,
                        $"Team tries to sell engine {teamEngineId} but this engine is not in the team");
                }
                
                var engine = await _domainService.GetEngineById(team.Engine.EngineId);
                _logger.LogInformation(
                    "Selling engine {engineName} with value {currentValue} warn off for '{warnOffPercentage}' for '{sellingPrice}'",
                    team.Engine.Name,
                    engine.Value,
                    team.Engine.WarnOffPercentage,
                    team.Engine.GetSellingPrice(engine.Value));
                
                await team.SellEngine(_domainService);
                var soldSuccessfully = await _teamsRepository.Update(team);
                _logger.LogInformation("Engine sold successfully: {successfully}", soldSuccessfully);
                return soldSuccessfully;
            }

            throw new F1ManagerTeamException(TeamErrorCode.PersistenceFailed, $"Failed to sell engine with ID {teamEngineId}");
        }

        #endregion

        #region [ Team chassis transactions ]

        public async Task<TeamChassisDetailsDto> GetChassis(Guid teamId)
        {
            _logger.LogInformation("Getting chassis details for team {teamId}", teamId);
            var team = await _teamsRepository.Get(teamId);

            if (team?.Chassis != null)
            {
                _logger.LogInformation("Team chassis was found, returning detailed information");
                var chassis = await _domainService.GetChassisById(team.Chassis.ChassisId);
                return new TeamChassisDetailsDto
                {
                    Id = team.Id,
                    Name = team.Chassis.Name,
                    PictureUrl = team.Chassis.PictureUrl,
                    BoughtOn = team.Chassis.BoughtOn,
                    BoughtFor = team.Chassis.BoughtFor,
                    CurrentPrice = chassis.Value,
                    MaxWearOff = chassis.MaximumWearOff,
                    PointsGained = 0,
                    WearOff = team.Chassis.WarnOffPercentage
                };
            }

            _logger.LogInformation("No chassis found for team {teamId}", teamId);
            return null;
        }

        public async Task<TeamChassisDetailsDto> PurchaseChassis(Guid userId,  Guid chassisId)
        {
            _logger.LogInformation("Purchasing engine for player '{userId}'", userId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team != null)
            {
                var boughtChassis = await team.BuyChassis(chassisId, _domainService);
                if (await _teamsRepository.Update(team))
                {
                    _logger.LogInformation("Chassis '{chassisId}' ({chassisName}) successfully bought for team '{teamName}'", chassisId, team.Chassis.Name, team.Name);

                    return new TeamChassisDetailsDto
                    {
                        Id = team.Chassis.Id,
                        Name = team.Chassis.Name,
                        Manufacturer = boughtChassis.Manufacturer,
                        Model = boughtChassis.Model,
                        PictureUrl = boughtChassis.PictureUrl,
                        BoughtOn = team.Chassis.BoughtOn,
                        BoughtFor = team.Chassis.BoughtFor,
                        CurrentPrice = boughtChassis.Value,
                        MaxWearOff = boughtChassis.MaximumWearOff,
                        PointsGained = 0,
                        WearOff = 0
                    };
                }
            }

            return null;
        }

        public async Task<SellConfirmationDto> SellChassisConfirmation(Guid userId, Guid teamChassisId)
        {
            _logger.LogInformation($"Gathering information about the chassis to sell for user {userId}", userId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team?.Chassis != null)
            {
                if (team.Chassis.Id != teamChassisId)
                {
                    _logger.LogWarning(
                        "Incoming request to sell chassis {expectedTeamChassisId}, but team owns team chassis ID {actualTeamEngineId}",
                        teamChassisId, team.Chassis.Id);
                    throw new F1ManagerTeamException(TeamErrorCode.ComponentNotInPosession,
                        $"Team tries to sell chassis {teamChassisId} but this chassis is not in the team");
                }

                var chassis = await _domainService.GetChassisById(team.Chassis.ChassisId);
                _logger.LogInformation("Team is selling chassis '{chassisName}' gathering information to sell", team.Chassis.Name);
                return new SellConfirmationDto
                {
                    Id = team.Chassis.Id,
                    Name = team.Chassis.Name,
                    CurrentValue = chassis.Value,
                    WearOffPercentage = team.Chassis.WarnOffPercentage,
                    SellingPrice = team.Chassis.GetSellingPrice(chassis.Value)
                };
            }
            _logger.LogInformation("Team not found or team does not own an engine, skipping selling proposal");
            return null;
        }

        public async Task<bool> SellChassis(Guid userId, Guid teamChassisId)
        {
            _logger.LogInformation("Player '{userId}' is going to sell chassis {teamChassisId}", userId,
                teamChassisId);
            var team = await _teamsRepository.GetByUserId(SeasonsHelper.GetSeasonId(), userId);
            if (team?.Chassis != null)
            {
                if (team.Chassis.Id != teamChassisId)
                {
                    _logger.LogWarning(
                        "Incoming request to sell chassis {expectedTeamEngineId}, but team owns team chassis ID {teamChassisId}",
                        teamChassisId, team.Engine.Id);
                    throw new F1ManagerTeamException(TeamErrorCode.ComponentNotInPosession,
                        $"Team tries to sell chassis {teamChassisId} but this chassis is not in the team");
                }

                var chassis = await _domainService.GetChassisById(team.Chassis.ChassisId);
                _logger.LogInformation(
                    "Selling chassis {chassisName} with value {currentValue} warn off for '{warnOffPercentage}' for '{sellingPrice}'",
                    team.Chassis.Name,
                    chassis.Value,
                    team.Chassis.WarnOffPercentage,
                    team.Chassis.GetSellingPrice(chassis.Value));

                await team.SellEngine(_domainService);
                var soldSuccessfully = await _teamsRepository.Update(team);
                _logger.LogInformation("Chassis sold successfully: {successfully}", soldSuccessfully);
                return soldSuccessfully;
            }

            throw new F1ManagerTeamException(TeamErrorCode.PersistenceFailed, $"Failed to sell chassis with ID {teamChassisId}");
        }

        #endregion


        private static TeamDetailsDto DomainModelToDto(Team team, Guid playerId)
        {
            return new TeamDetailsDto
            {
                Id = team.Id,
                Name = team.Name,
                Points = team.Points,
                Money = team.Money,
                FirstDriverId = team.FirstDriver?.Id,
                SecondDriverId = team.SecondDriver?.Id,
                EngineId = team.Engine?.Id,
                ChassisId = team.Chassis?.Id,
                IsTeamOwner = team.OwnerId.Equals(playerId),
                IsPublic = team.IsPublic
            };
        }
        private Task<Guid?> GetTeamId(int seasonId, Guid userId)
        {
            return _teamsRepository.GetTeamId(seasonId, userId);
        }

        public TeamsService(ITeamsRepository teamsRepository, 
            ITeamsDomainService domainService,
            ILogger<TeamsService> logger)
        {
            _teamsRepository = teamsRepository;
            _domainService = domainService;
            _logger = logger;
        }

    }
}