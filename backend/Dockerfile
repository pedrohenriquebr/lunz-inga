# syntax=docker/dockerfile:1
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /app
# Copy csproj and restore as distinct layers
COPY ./*.sln .
COPY ./LuzInga.Api/*.csproj ./LuzInga.Api/
COPY ./LuzInga.Application/*.csproj ./LuzInga.Application/
COPY ./LuzInga.Domain/*.csproj ./LuzInga.Domain/
COPY ./LuzInga.Infra/*.csproj ./LuzInga.Infra/
COPY ./LuzInga.UnitTests/*.csproj ./LuzInga.UnitTests/
COPY ./LuzInga.HangFire/*.csproj ./LuzInga.HangFire/
RUN dotnet restore

# Copy everything else and build
COPY ./LuzInga.Api/. ./LuzInga.Api/
COPY ./LuzInga.Application/. ./LuzInga.Application/
COPY ./LuzInga.Domain/. ./LuzInga.Domain/
COPY ./LuzInga.Infra/. ./LuzInga.Infra/
COPY ./LuzInga.UnitTests/. ./LuzInga.UnitTests/
COPY ./LuzInga.HangFire/. ./LuzInga.HangFire/
WORKDIR /app/LuzInga.Api
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build /app/LuzInga.Api/out ./
ENTRYPOINT ["dotnet", "LuzInga.Api.dll"]
