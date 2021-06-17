using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using F1Manager.SqlData;
using F1Manager.SqlData.Entities;
using F1Manager.SqlData.Entities.BaseData.Circuits;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.CommandLineUtils;

namespace F1Manager.Migrations
{
    class Program
    {
        public static int Main(string[] args)
        {
            var app = new CommandLineApplication
            {
                Name = "F1 Manager - Database Migrator",
                Description = "Entity Framework Migrations Runner"
            };

            var environment = Environment.GetEnvironmentVariable("debugging") ?? "false";
            var debugging = bool.Parse(environment);

            app.HelpOption("-?|-h|--help");

            app.Command("migrate", command =>
            {
                command.HelpOption("-?|-h|--help");

                var databaseNameOption = command.Option("-n | --databaseName", "The name of the sql database.",
                    CommandOptionType.SingleValue);
                var serverNameOption = command.Option("-s | --databaseServerName", "The name of the sql server.",
                    CommandOptionType.SingleValue);
                var userNameOption = command.Option("-u | --user", "The name of database user.",
                    CommandOptionType.SingleValue);
                var passwordOption = command.Option("-p | --password", "The password of the database user.",
                    CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    try
                    {
                        if (!databaseNameOption.HasValue() || !serverNameOption.HasValue() ||
                            !userNameOption.HasValue() || !passwordOption.HasValue())
                        {
                            app.ShowHint();
                            return 1;
                        }

                        Console.WriteLine($"Updating database {databaseNameOption.Value()} on server {serverNameOption.Value()} ...");

                        var builder = new SqlConnectionStringBuilder
                        {
                            ["Data Source"] = debugging ? serverNameOption.Value() : $"tcp:{serverNameOption.Value()}.database.windows.net,1433",
                            ["Initial Catalog"] = databaseNameOption.Value(),
                            ["User Id"] = debugging ? userNameOption.Value() : $"{userNameOption.Value()}@{serverNameOption.Value()}",
                            ["Password"] = passwordOption.Value()
                        };
                        var optionsBuilder = new DbContextOptionsBuilder<F1ManagerDbContext>();
                        optionsBuilder.UseSqlServer(builder.ConnectionString,
                            x => x.MigrationsAssembly(typeof(F1ManagerDbContext).Assembly.GetName().Name));
                        optionsBuilder.EnableDetailedErrors();

                        var dataContextInstance = new F1ManagerDbContext(optionsBuilder.Options);
                        var pendingMigrations = dataContextInstance.Database
                            .GetPendingMigrations()
                            .ToArray();

                        if (pendingMigrations.Any())
                        {
                            Console.WriteLine("Migration to be applied: {0}", string.Join(",", pendingMigrations));
                            dataContextInstance.Database.Migrate();
                            Console.WriteLine("Done. Database updated successfully");
                        }

                        Console.ReadKey();
                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Update database failed! {0}", ex);
                        Console.ReadKey();
                        return -1;
                    }

                });
            });
            app.Command("seed", command =>
            {
                command.HelpOption("-?|-h|--help");

                var databaseNameOption = command.Option("-n | --databaseName", "The name of the sql database.",
                    CommandOptionType.SingleValue);
                var serverNameOption = command.Option("-s | --databaseServerName", "The name of the sql server.",
                    CommandOptionType.SingleValue);
                var userNameOption = command.Option("-u | --user", "The name of database user.",
                    CommandOptionType.SingleValue);
                var passwordOption = command.Option("-p | --password", "The password of the database user.",
                    CommandOptionType.SingleValue);

                command.OnExecute(() =>
                {
                    try
                    {
                        if (!databaseNameOption.HasValue() || !serverNameOption.HasValue() ||
                            !userNameOption.HasValue() || !passwordOption.HasValue())
                        {
                            app.ShowHint();
                            return 1;
                        }

                        Console.WriteLine(
                            $"Seeding database {databaseNameOption.Value()} on server {serverNameOption.Value()} ...");

                        var builder = new SqlConnectionStringBuilder
                        {
                            ["Data Source"] = debugging ? serverNameOption.Value() : $"tcp:{serverNameOption.Value()}.database.windows.net,1433",
                            ["Initial Catalog"] = databaseNameOption.Value(),
                            ["User Id"] = debugging ? userNameOption.Value() : $"{userNameOption.Value()}@{serverNameOption.Value()}",
                            ["Password"] = passwordOption.Value()
                        };

                        // DATA SOURCE=tcp:confoo-test-sqlsvr.database.windows.net,1433;User ID=confoo-test-user;Password=DyYBletzXnvUQsYz8NxT;Initial Catalog=confoo-test-sqldb;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;

                        var optionsBuilder = new DbContextOptionsBuilder<F1ManagerDbContext>();
                        optionsBuilder.UseSqlServer(builder.ConnectionString,
                            x => x.MigrationsAssembly(typeof(F1ManagerDbContext).Assembly.GetName().Name));
                        optionsBuilder.EnableDetailedErrors();

                        var svcApiDbContext = new F1ManagerDbContext(optionsBuilder.Options);
                        //SeedDatabase(svcApiDbContext);


                        return 0;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Seeding database failed! {0}", ex);
                        return -1;
                    }
                });
            });

            app.OnExecute(() =>
            {
                app.ShowHelp();
                return 1;
            });

            return app.Execute(args);

        }

        //private static void SeedDatabase(F1ManagerDbContext ddc)
        //{
        //    SeedSeasons(ddc);
        //    SeedCircuits(ddc);
        //    SeedCalendar(ddc);
        //    SeedDriverPointsTable(ddc);
        //    SeedDrivers(ddc);
        //    SeedEngines(ddc);
        //    SeedChassis(ddc);
        //}

        //private static void SeedSeasons(F1ManagerDbContext ddc)
        //{
        //    var startDate = new DateTime(2021, 2, 1);
        //    var season21 = ddc.Seasons.FirstOrDefault(x => x.ActiveFrom < startDate && x.ActiveUntil > startDate);
        //    if (season21 == null)
        //    {

        //        season21 = new SeasonEntity
        //        {
        //            Id = Guid.NewGuid(),
        //            Name = "Formula 1 Schedule 2021",
        //            ActiveFrom = new DateTimeOffset(new DateTime(2021, 1, 1)),
        //            ActiveUntil = new DateTimeOffset(new DateTime(2021, 12, 31))
        //        };
        //        ddc.Add(season21);
        //        Console.WriteLine($"Seeding season {season21.ActiveFrom.Year}...");
        //        ddc.SaveChanges();
        //    }
        //}
        //private static void SeedCircuits(F1ManagerDbContext ddc)
        //{
        //    var circuits = new List<CircuitEntity>
        //    {
        //        new AbuDhabi(),
        //        new America(),
        //        new Bahrain(),
        //        new Baku(),
        //        new Catalunya(),
        //        new Hungaroring(),
        //        new Imola(),
        //        new Melbourne(),
        //        new Mexico(),
        //        new Monaco(),
        //        new Montreal(),
        //        new Monza(),
        //        new PaulRicard(),
        //        new SaoPaulo(),
        //        new Silverstone(),
        //        new Singapore(),
        //        new Sochi(),
        //        new Spa(),
        //        new Spielberg(),
        //        new Suzuka(),
        //        new Zandvoort()
        //    };

        //    foreach (var ci in circuits)
        //    {
        //        var count = ddc.Circuits.Count(x => x.Id == ci.Id);
        //        if (count == 0)
        //        {
        //            Console.WriteLine($"Seeding circuit {ci.Name}...");
        //            ddc.Add(ci);
        //        }
        //    }

        //    ddc.SaveChanges();
        //}
        //private static void SeedCalendar(F1ManagerDbContext ddc)
        //{
        //    var calendar = new List<Calendar>();
        //    calendar.Add(new Calendar("2021-03-27 15", "2021-03-28 15", new Bahrain().Id, "Formula 1 Gulf Air Bahrain Grand Prix 2021"));

        //    foreach (var c in calendar)
        //    {
        //        var s = ddc.Seasons.FirstOrDefault(x => x.ActiveFrom < c.RaceStart && x.ActiveUntil > c.RaceStart);
        //        if (s != null)
        //        {
        //            var race = ddc.Races.FirstOrDefault(x => x.SeasonId == s.Id && x.Race == c.RaceStart);
        //            if (race == null)
        //            {
        //                Console.WriteLine($"Seeding race {c.Name}...");
        //                race = new RaceEntity
        //                {
        //                    Name = c.Name,
        //                    CircuitId = c.CircuitId,
        //                    Qualifying = c.QualiStart,
        //                    Race = c.RaceStart,
        //                    SeasonId = s.Id,
        //                    TeamLockStartsOn = c.QualiStart,
        //                    TeamLockEndsOn = c.RaceStart.AddHours(5)
        //                };
        //                ddc.Add(race);
        //                ddc.SaveChanges();
        //            }
        //        }
        //    }
        //}
        //private static void SeedDriverPointsTable(F1ManagerDbContext ddc)
        //{
        //    var pointsTableEntities = ddc.DriverPointsTable.ToList();
        //    foreach (var dp in DriverPointsTable.GetDriverPointsTable())
        //    {
        //        var p = pointsTableEntities.FirstOrDefault(x => x.Id == dp.Id);
        //        if (p == null)
        //        {
        //            p = new DriverPointsTableEntity(dp.Qualification, dp.Race);
        //            ddc.Add(p);
        //        }
        //        else
        //        {
        //            p.Qualification = dp.Qualification;
        //            p.Race = dp.Race;
        //            ddc.Entry(p).State = EntityState.Modified;
        //        }
        //    }

        //    ddc.SaveChanges();
        //}
        //private static void SeedEngines(F1ManagerDbContext ddc)
        //{
        //    Console.WriteLine($"Seeding engines");

        //    var engines = ddc.Engines.ToList();
        //    foreach (var dp in Engines.GetEngines())
        //    {
        //        var engineEntity = engines.FirstOrDefault(x => x.Id == dp.Id);
        //        if (engineEntity == null)
        //        {
        //            ddc.Add(dp);
        //        }
        //    }

        //    var affected = ddc.SaveChanges();
        //    Console.WriteLine($"{affected} Engines have been inserted");
        //}
        //private static void SeedChassis(F1ManagerDbContext ddc)
        //{
        //    Console.WriteLine($"Seeding chassis");

        //    var chassis = ddc.Chassis.ToList();
        //    foreach (var dp in Chassis.GetChassis())
        //    {
        //        var chassisEntity = chassis.FirstOrDefault(x => x.Id == dp.Id);
        //        if (chassisEntity == null)
        //        {
        //            ddc.Add(dp);
        //        }
        //    }

        //    var affected = ddc.SaveChanges();
        //    Console.WriteLine($"{affected} Chassis have been inserted");
        //}
        //private static void SeedDrivers(F1ManagerDbContext ddc)
        //{
        //    Console.WriteLine($"Seeding drivers");

        //    var drivers = ddc.Drivers.ToList();
        //    foreach (var dp in Drivers.GetDrivers())
        //    {
        //        var driverEntity = drivers.FirstOrDefault(x => x.Id == dp.Id);
        //        if (driverEntity == null)
        //        {
        //            ddc.Add(dp);
        //        }
        //    }

        //    var affected = ddc.SaveChanges();
        //    Console.WriteLine($"{affected} Drivers have been inserted");
        //}

    }
}