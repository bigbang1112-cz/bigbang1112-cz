FROM mcr.microsoft.com/dotnet/aspnet:6.0-alpine AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build
WORKDIR /src
COPY ["BigBang1112/BigBang1112.csproj", "BigBang1112/"]
COPY ["BigBang1112.BongoBot3/BigBang1112.BongoBot3.csproj", "BigBang1112.BongoBot3/"]
COPY ["BigBang1112.Essentials/BigBang1112.DiscordBot/BigBang1112.DiscordBot.csproj", "BigBang1112.Essentials/BigBang1112.DiscordBot/"]
COPY ["BigBang1112.Essentials/BigBang1112.Essentials/BigBang1112.Essentials.csproj", "BigBang1112.Essentials/BigBang1112.Essentials/"]
COPY ["BigBang1112.Essentials/BigBang1112.Essentials.Razor/BigBang1112.Essentials.Razor.csproj", "BigBang1112.Essentials/BigBang1112.Essentials.Razor/"]
COPY ["BigBang1112.UniReminder/BigBang1112.UniReminder.csproj", "BigBang1112.UniReminder/"]
RUN dotnet restore "BigBang1112/BigBang1112.csproj"
COPY . .
WORKDIR "/src/BigBang1112"
RUN dotnet build "BigBang1112.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BigBang1112.csproj" -c Release -o /app/publish -r linux-musl-x64 -p:PublishReadyToRun=true --self-contained

FROM mcr.microsoft.com/dotnet/runtime-deps:6.0-alpine
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["./BigBang1112"]
