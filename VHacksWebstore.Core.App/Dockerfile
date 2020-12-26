#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:5.0-buster-slim AS build
WORKDIR /src
COPY ["VHacksWebstore.Core.App/VHacksWebstore.Core.App.csproj", "VHacksWebstore.Core.App/"]
COPY ["App.Components/App.Components.csproj", "App.Components/"]
COPY ["VHacksWebstore.Core.Domain/VHacksWebstore.Core.Domain.csproj", "VHacksWebstore.Core.Domain/"]
COPY ["VHacksWebstore.Data/VHacksWebstore.Data.csproj", "VHacksWebstore.Data/"]
RUN dotnet restore "VHacksWebstore.Core.App/VHacksWebstore.Core.App.csproj"
COPY . .
WORKDIR "/src/VHacksWebstore.Core.App"
RUN dotnet build "VHacksWebstore.Core.App.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VHacksWebstore.Core.App.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "VHacksWebstore.Core.App.dll"]
