using System;
using Bogus;
using F1Manager.Users.Domain;

namespace F1Manager.Users.UnitTests.Factories
{
    public class UsersFactory
    {

        private static readonly Faker<User> UsersGenerator;

        public static User Random()
        {
            return UsersGenerator.Generate();
        }

        static UsersFactory()
        {
            UsersGenerator = new Faker<User>()
                .CustomInstantiator(f => new User(
                    f.Random.Guid(),
                    f.Person.FullName,
                    f.Person.Email,
                    f.Random.String(8, 16),
                    f.Person.Email,
                    f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddYears(-2), DateTimeOffset.UtcNow.AddYears(-1)),
                    f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddYears(-2), DateTimeOffset.UtcNow.AddYears(-1)),
                    null,
                    false,
                    f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddYears(-2), DateTimeOffset.UtcNow.AddYears(-1)),
                    f.Date.BetweenOffset(DateTimeOffset.UtcNow.AddYears(-1), DateTimeOffset.UtcNow)
                ));
        }

    }

}
