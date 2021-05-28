#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["F1Manager.Api/F1Manager.Api.csproj", "F1Manager.Api/"]
RUN dotnet restore "F1Manager.Api/F1Manager.Api.csproj"
COPY . .
WORKDIR "/src/F1Manager.Api"
RUN dotnet build "F1Manager.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "F1Manager.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "F1Manager.Api.dll"]