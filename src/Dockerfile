FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["src/S4Capital.Api.csproj", "src/"]
RUN dotnet restore "src/S4Capital.Api.csproj"
COPY . .
WORKDIR "/src/src"
RUN dotnet build "S4Capital.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "S4Capital.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "S4Capital.Api.dll"]