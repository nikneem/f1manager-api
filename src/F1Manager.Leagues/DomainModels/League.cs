using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using F1Manager.Leagues.Abstractions;
using F1Manager.Leagues.Exceptions;
using F1Manager.Shared.Base;
using F1Manager.Shared.Constants;
using F1Manager.Shared.Enums;

namespace F1Manager.Leagues.DomainModels
{
    public class League : DomainModel<Guid>

    {
        private List< LeagueMember> _members { get; set; }

        public Guid OwnerId { get; }
        public int SeasonId { get; }
        public string Name { get; set; }
        public DateTimeOffset CreatedOn { get;  }
        public IReadOnlyList<LeagueMember> Members => _members.AsReadOnly();

        public async Task SetName(string value, ILeaguesDomainService domainService)
        {
            if (string.IsNullOrEmpty(value))
            {
                throw new F1ManagerLeaguesException(LeagueErrorCode.NameNullOrEmpty,
                    "The league name cannot be null or empty");
            }

            if (!Regex.IsMatch(value, RegularExpressions.Leaguename))
            {
                throw new F1ManagerLeaguesException(LeagueErrorCode.NameInvalid,
                    "The league name is invalid. It cannot contain special characters, must be between 3 and 100 characters and unique");
            }

            if (!await domainService.IsUniqueName(value, SeasonId))
            {
                throw new F1ManagerLeaguesException(
                    LeagueErrorCode.NameAlreadyTaken,
                    "A league with this name already exists");
            }

            if (!Equals(Name, value))
            {
                Name = value;
                SetState(TrackingState.Modified);
            }
        }

        public void AddMember(Guid teamId)
        {
            if (_members.Any(m => m.TeamId == teamId))
            {
                throw new Exception();
            }

            var member = LeagueMember.Create(teamId);
            member.SetMaintainer(_members.Count==0);
            _members.Add(member);
            SetState(TrackingState.Touched);
        }


        public League(Guid id, Guid ownerId, string name, DateTimeOffset createdOn, List<LeagueMember> members): base(id)
        {
            OwnerId = ownerId;
            Name = name;
            CreatedOn = createdOn;
            _members = members ?? new List<LeagueMember>();
        }

        private League(Guid ownerId) : base(Guid.NewGuid(), TrackingState.New)
        {
            OwnerId = ownerId;
            SeasonId = DateTimeOffset.UtcNow.Year;
            CreatedOn = DateTimeOffset.UtcNow;
            _members = new List<LeagueMember>();
        }

        public static async Task<League> Create(Guid ownerId, string name, ILeaguesDomainService service)
        {
            var league = new League(ownerId);
            await league.SetName(name, service);
            return league;
        }

    }
}
