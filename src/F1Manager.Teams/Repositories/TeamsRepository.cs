using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using F1Manager.Shared.DataTransferObjects;
using F1Manager.Shared.Enums;
using F1Manager.Shared.ExtensionMethods;
using F1Manager.Shared.Helpers;
using F1Manager.SqlData;
using F1Manager.SqlData.Entities;
using F1Manager.Teams.Abstractions;
using F1Manager.Teams.DataTransferObjects;
using F1Manager.Teams.Domain;
using F1Manager.Teams.Mappings;
using Microsoft.EntityFrameworkCore;

namespace F1Manager.Teams.Repositories
{
    public sealed class TeamsRepository : ITeamsRepository
    {

        private readonly DbContextOptions _dbContextOptions;


        public async Task<CollectionResult<TeamListItemDto>> GetList(int seasonId, TeamsListFilterDto filter)
        {
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            var query = context.Teams.AsQueryable().Where(ent => ent.SeasonId == seasonId);
            if (!string.IsNullOrEmpty(filter.Name))
            {
                query = query.Where(ent => ent.Name.Contains(filter.Name));
            }

            var totalEntries = await query.CountAsync();

            var list = await query
                .Skip(filter.Skip)
                .Take(filter.CalculatedPageSize)
                .Select(ent => new TeamListItemDto
                {
                    Id = ent.Id,
                    Name = ent.Name,
                    Points = ent.Points
                }).ToListAsync();

            return new CollectionResult<TeamListItemDto>
            {
                PageSize = filter.CalculatedPageSize,
                Page = filter.CalculatedPage,
                TotalEntries = totalEntries,
                TotalPages = filter.CalculatedPageSize.CalculatePages(totalEntries),
                Entities = list
            };
        }

        public async Task<Guid?> GetTeamId(int seasonId, Guid userId)
        {
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            var team = await context.Teams.FirstOrDefaultAsync(t => t.SeasonId == seasonId && t.UserId == userId);
            return team?.Id;
        }

        public async Task<Team> Get(Guid teamId)
        {
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            var team = await GetTeamEntity(teamId, context);

            var drivers = await GetDriverEntity(teamId, context);
            var firstDriver = drivers.FirstOrDefault(d => d.IsFirstDriver);
            var secondDriver = drivers.FirstOrDefault(d => !d.IsFirstDriver);
            
            var engine = await GetEngineEntity(teamId, context);
            var chassis = await GetChassisEntity(teamId, context);

            if (team != null)
            {
                return new Team(team.Id,
                    SeasonsHelper.GetSeasonId(),
                    team.UserId,
                    team.Name,
                    team.Points,
                    team.Money,
                    firstDriver.ToDomainModel(),
                    secondDriver.ToDomainModel(),
                    engine.ToDomainModel(),
                    chassis.ToDomainModel());
            }

            throw new Exception($"Team with ID {teamId} was not found");
        }

        public async Task<Team> GetByUserId(int seasonId, Guid userId)
        {
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            var teamEntity = await context.Teams
                .FirstOrDefaultAsync(t => t.SeasonId == seasonId && t.UserId == userId);

            var teamId = teamEntity?.Id;
            return teamId.HasValue ? await Get(teamId.Value) : null;
        }
        public async Task<bool> Create(Team team)
        {
            if (team.TrackingState == TrackingState.New)
            {
                await using var context = new F1ManagerDbContext(_dbContextOptions);
                var teamEntity = new TeamEntity
                {
                    Id = team.Id,
                    UserId = team.OwnerId,
                    SeasonId = team.SeasonId,
                    Name = team.Name,
                    Points = team.Points,
                    Money = team.Money
                };
                context.Add(teamEntity);
                return await context.SaveChangesAsync() > 0;
            }

            return false;
        }
        public async Task<bool> Update(Team domainModel)
        {
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            var team = await GetTeamEntity(domainModel.Id, context);
            var drivers = await GetDriverEntity(domainModel.Id, context);
            var engine = await GetEngineEntity(domainModel.Id, context);
            var chassis = await GetChassisEntity(domainModel.Id, context);
            var firstDriver = drivers.FirstOrDefault(x => x.IsFirstDriver);
            var secondDriver = drivers.FirstOrDefault(x => !x.IsFirstDriver);

            if (domainModel.TrackingState == TrackingState.Modified)
            {
                team = domainModel.ToEntity(team);
                context.Entry(team).State = EntityState.Modified;
            }

            UpdateTeamEngine(context, domainModel, engine);
            UpdateTeamChassis(context, domainModel, chassis);
            UpdateTeamDriver(context, domainModel, firstDriver, secondDriver);

            var affectedItems = await context.SaveChangesAsync();
            return affectedItems > 0;
        }

        public async Task<bool> GetUserHasTeam(int seasonId, Guid userId)
        {
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            return await context.Teams.AnyAsync(t=> t.UserId == userId && t.SeasonId == seasonId);
        }

        public async Task<bool> IsUniqueName(Guid id, string name)
        {
            await using var context = new F1ManagerDbContext(_dbContextOptions);
            var alreadyExists = await context.Teams.AnyAsync(x => !x.Id.Equals(id) && x.Name == name);
            return !alreadyExists;
        }


        private void UpdateTeamEngine(F1ManagerDbContext context, Team domainModel, TeamEngineEntity engine)
        {
            if (domainModel.Engine != null)
            {
                if (domainModel.Engine.TrackingState == TrackingState.Modified)
                {
                    engine = domainModel.Engine.ToEntity(domainModel.Id, engine);
                    context.Entry(engine).State = EntityState.Modified;
                }
                if (domainModel.Engine.TrackingState == TrackingState.New)
                {
                    engine = domainModel.Engine.ToEntity(domainModel.Id);
                    context.Attach(engine);
                    context.Entry(engine).State = EntityState.Added;
                }
            }
        }
        private void UpdateTeamChassis(F1ManagerDbContext context, Team domainModel, TeamChassisEntity chassis)
        {
            if (domainModel.Chassis != null)
            {
                if (domainModel.Chassis.TrackingState == TrackingState.Modified)
                {
                    chassis = domainModel.Chassis.ToEntity(domainModel.Id, chassis);
                    context.Entry(chassis).State = EntityState.Modified;
                }
                if (domainModel.Chassis.TrackingState == TrackingState.New)
                {
                    chassis = domainModel.Chassis.ToEntity(domainModel.Id);
                    context.Attach(chassis);
                    context.Entry(chassis).State = EntityState.Added;
                }
            }
        }
        private void UpdateTeamDriver(F1ManagerDbContext context, Team domainModel, TeamDriverEntity firstDriver, TeamDriverEntity secondDriver)
        {
            if (domainModel.FirstDriver != null)
            {
                if (domainModel.FirstDriver.TrackingState == TrackingState.Modified)
                {
                    firstDriver = domainModel.FirstDriver.ToEntity(domainModel.Id, firstDriver);
                    context.Entry(firstDriver).State = EntityState.Modified;
                }
                if (domainModel.FirstDriver.TrackingState == TrackingState.New)
                {
                    firstDriver = domainModel.FirstDriver.ToEntity(domainModel.Id);
                    context.Add(firstDriver);
                }
            }

            if (domainModel.SecondDriver != null)
            {
                if (domainModel.SecondDriver.TrackingState == TrackingState.Modified)
                {
                    secondDriver = domainModel.SecondDriver.ToEntity(domainModel.Id, secondDriver);
                    context.Entry(secondDriver).State = EntityState.Modified;
                }
                if (domainModel.SecondDriver.TrackingState == TrackingState.New)
                {
                    secondDriver = domainModel.SecondDriver.ToEntity(domainModel.Id);
                    context.Add(secondDriver);
                }
            }

        }


        private async Task<TeamEntity> GetTeamEntity(Guid id, F1ManagerDbContext context)
        {
            return await context.Teams.FirstOrDefaultAsync(ent => ent.Id == id);
        }
        private async Task<TeamEngineEntity> GetEngineEntity(Guid teamId, F1ManagerDbContext context)
        {
            return await context.TeamEngines
                .FirstOrDefaultAsync(engine => engine.TeamId == teamId && !engine.SoldOn.HasValue);
        }
        private async Task<TeamChassisEntity> GetChassisEntity(Guid teamId, F1ManagerDbContext context)
        {
            return await context.TeamChassis
                .FirstOrDefaultAsync(engine => engine.TeamId == teamId && !engine.SoldOn.HasValue);
        }
        private async Task<List<TeamDriverEntity>> GetDriverEntity(Guid teamId, F1ManagerDbContext context)
        {
            var drivers = await context.TeamDrivers
                .Where(driver => driver.TeamId == teamId && !driver.SoldOn.HasValue)
                .ToListAsync();
            if (drivers.Count > 2)
            {
                throw new Exception("Oops, more than two drivers in the team, this is unexpected behavior");
            }

            return drivers;
        }


        public TeamsRepository(DbContextOptions dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

    }
}