using System;
using Bogus;
using F1Manager.Teams.DataTransferObjects;
using F1Manager.Teams.Domain;

namespace F1Manager.Teams.UnitTests.Factories
{
    public class TeamsFactory
    {
        private static readonly Faker<Team> UsersGenerator;
        private static readonly Faker<DriverDto> DriverGenerator;
        private static readonly Faker<EngineDto> EngineGenerator;
        private static readonly Faker<ChassisDto> ChassisGenerator;

        public static Team Random()
        {
            return UsersGenerator.Generate();
        }

        public static DriverDto RandomDriver()
        {
            return DriverGenerator.Generate();
        }
        public static EngineDto RandomEngine()
        {
            return EngineGenerator.Generate();
        }
        public static ChassisDto RandomChassis()
        {
            return ChassisGenerator.Generate();
        }
        static TeamsFactory()
        {
            UsersGenerator = new Faker<Team>()
                .CustomInstantiator(f => new Team(
                    f.Random.Guid(),
                    f.Random.Int(),
                    f.Random.Guid(),
                    f.Person.FullName,
                    f.Random.Int(120, 600),
                    f.Random.Decimal(60000000, 120000000),
                    null, null, null, null));

            DriverGenerator = new Faker<DriverDto>()
                .RuleFor(x => x.Id, x => x.Random.Guid())
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .RuleFor(x => x.Country, x => x.Address.Country())
                .RuleFor(x => x.DateOfBirth,
                    x => x.Date.BetweenOffset(DateTimeOffset.UtcNow.AddYears(-30), DateTimeOffset.UtcNow.AddYears(-18)))
                .RuleFor(x => x.PictureUrl, x => x.Internet.Url())
                .RuleFor(x => x.Value, x => x.Random.Decimal(12000000, 60000000));

            EngineGenerator = new Faker<EngineDto>()
                .RuleFor(x => x.Id, x => x.Random.Guid())
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .RuleFor(x => x.Manufacturer, x => x.Company.CompanyName())
                .RuleFor(x => x.WeeklyWearOff, x => x.Random.Decimal(1M, 3M))
                .RuleFor(x => x.MaximumWearOff, x => x.Random.Decimal(10M, 12M))
                .RuleFor(x => x.PictureUrl, x => x.Internet.Url())
                .RuleFor(x => x.Value, x => x.Random.Decimal(12000000, 60000000));

            ChassisGenerator = new Faker<ChassisDto>()
                .RuleFor(x => x.Id, x => x.Random.Guid())
                .RuleFor(x => x.Name, x => x.Person.FullName)
                .RuleFor(x => x.Manufacturer, x => x.Company.CompanyName())
                .RuleFor(x => x.WeeklyWearOff, x => x.Random.Decimal(1M, 3M))
                .RuleFor(x => x.MaximumWearOff, x => x.Random.Decimal(10M, 12M))
                .RuleFor(x => x.PictureUrl, x => x.Internet.Url())
                .RuleFor(x => x.Value, x => x.Random.Decimal(12000000, 60000000));

        }
    }
}