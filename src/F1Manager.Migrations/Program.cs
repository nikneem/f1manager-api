using System;
using System.Linq;
using F1Manager.SqlData;
using F1Manager.SqlData.Entities;
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
                        SeedDatabase(svcApiDbContext);


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

        private static void SeedDatabase(F1ManagerDbContext ddc)
        {
            //SeedSeasons(ddc);
            //SeedCircuits(ddc);
            //SeedCalendar(ddc);
            SeedDriverPointsTable(ddc);
            SeedEnginePointsTable(ddc);
            SeedChassisPointsTable(ddc);
//            SeedDrivers(ddc);
            //SeedEngines(ddc);
            //SeedChassis(ddc);
        }

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
        private static void SeedDriverPointsTable(F1ManagerDbContext ddc)
        {
            var pointsTableEntities = ddc.DriverPoints.ToList();
            if (pointsTableEntities.Count == 0)
            {
                ddc.DriverPoints.Add(new DriverPointEntity(1, 25, 60, 10));
                ddc.DriverPoints.Add(new DriverPointEntity(2, 24, 54, 9));
                ddc.DriverPoints.Add(new DriverPointEntity(3, 23, 49, 8));
                ddc.DriverPoints.Add(new DriverPointEntity(4, 22, 44, 7));
                ddc.DriverPoints.Add(new DriverPointEntity(5, 21, 39, 6));
                ddc.DriverPoints.Add(new DriverPointEntity(6, 10, 35, 5));
                ddc.DriverPoints.Add(new DriverPointEntity(7, 19, 31, 4));
                ddc.DriverPoints.Add(new DriverPointEntity(8, 18, 28, 3));
                ddc.DriverPoints.Add(new DriverPointEntity(9, 17, 25, 2));
                ddc.DriverPoints.Add(new DriverPointEntity(10, 16, 22, 1));
                ddc.DriverPoints.Add(new DriverPointEntity(11, 15, 20, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(12, 14, 18, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(13, 13, 16, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(14, 12, 14, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(15, 11, 12, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(16, 10, 10, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(17, 09, 8, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(18, 08, 6, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(19, 06, 4, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(20, 04, 3, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(21, 02, 2, 0));
                ddc.DriverPoints.Add(new DriverPointEntity(22, 01, 1, 0));
            }

            ddc.SaveChanges();
        }
        private static void SeedEnginePointsTable(F1ManagerDbContext ddc)
        {
            var pointsTableEntities = ddc.EnginePoints.ToList();
            if (pointsTableEntities.Count == 0)
            {
                ddc.EnginePoints.Add(new EnginePointEntity(1, 25, 40, 5));
                ddc.EnginePoints.Add(new EnginePointEntity(2, 22, 34, 4));
                ddc.EnginePoints.Add(new EnginePointEntity(3, 20, 30, 3));
                ddc.EnginePoints.Add(new EnginePointEntity(4, 18, 26, 2));
                ddc.EnginePoints.Add(new EnginePointEntity(5, 16, 23, 1));
                ddc.EnginePoints.Add(new EnginePointEntity(6, 14, 20, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(7, 12, 18, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(8, 10, 16, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(9, 09, 14, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(10, 08, 12, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(11, 07, 10, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(12, 06, 9, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(13, 05, 7, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(14, 04, 7, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(15, 03, 6, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(16, 03, 5, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(17, 02, 4, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(18, 02, 3, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(19, 01, 2, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(20, 01, 2, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(21, 00, 1, 0));
                ddc.EnginePoints.Add(new EnginePointEntity(22, 00, 1, 0));
            }

            ddc.SaveChanges();
        }
        private static void SeedChassisPointsTable(F1ManagerDbContext ddc)
        {
            var pointsTableEntities = ddc.ChassisPoints.ToList();
            if (pointsTableEntities.Count == 0)
            {
                ddc.ChassisPoints.Add(new ChassisPointEntity(1, 25, 40, 5));
                ddc.ChassisPoints.Add(new ChassisPointEntity(2, 22, 34, 4));
                ddc.ChassisPoints.Add(new ChassisPointEntity(3, 20, 30, 3));
                ddc.ChassisPoints.Add(new ChassisPointEntity(4, 18, 26, 2));
                ddc.ChassisPoints.Add(new ChassisPointEntity(5, 16, 23, 1));
                ddc.ChassisPoints.Add(new ChassisPointEntity(6, 14, 20, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(7, 12, 18, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(8, 10, 16, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(9, 09, 14, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(10, 08, 12, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(11, 07, 10, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(12, 06, 9, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(13, 05, 7, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(14, 04, 7, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(15, 03, 6, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(16, 03, 5, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(17, 02, 4, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(18, 02, 3, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(19, 01, 2, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(20, 01, 2, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(21, 00, 1, 0));
                ddc.ChassisPoints.Add(new ChassisPointEntity(22, 00, 1, 0));
            }

            ddc.SaveChanges();
        }
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


    }
}