# F1 Manager Application

[![.NET](https://github.com/nikneem/f1manager-api/actions/workflows/dotnet.yml/badge.svg)](https://github.com/nikneem/f1manager-api/actions/workflows/dotnet.yml)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)
[![Bugs](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=bugs)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)
[![Code Smells](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=code_smells)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=coverage)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)
[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)
[![Technical Debt](https://sonarcloud.io/api/project_badges/measure?project=nikneem_f1manager-api&metric=sqale_index)](https://sonarcloud.io/summary/new_code?id=nikneem_f1manager-api)

This is the backend system of my Formula 1 Manager game. This game is free and open-source, hosted on Azure and made just to hobby with software development, the Azure Cloud and more. If you like to contribute, drop a line because that's awesome!

## The idea

The idea is to manage your own Formula 1 team. You will start with a limited budget that you can use to buy team components. You need two drivers, an engine and a chassis to complete your team. When real-world Formula 1 races take place, you will receive points and gain money to buy better team components. At the end of the season, the team with the most amount of points is the winner.

## Database migrations

To maintain database migrations, you need to have the `dotnet` CLI installed and have the Entity Framework CLI extension tools installed as well `dotnet tool install --global dotnet-ef`. Once everything is set, you can open a console window navigate to the `F1Manager.SqlData` folder in the project. There, you type the following command:

`dotnet ef migrations add <<name>> --startup-project ../F1Manager.Api/F1Manager.Api.csproj`

_Obviously, you replace <\<name>> with the name of the migration_
