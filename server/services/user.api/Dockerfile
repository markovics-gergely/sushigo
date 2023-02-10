#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["services/user.api/user.api.csproj", "services/user.api/"]
COPY ["services/user.bll/user.bll.csproj", "services/user.bll/"]
COPY ["services/user.dal/user.dal.csproj", "services/user.dal/"]
RUN dotnet restore "services/user.api/user.api.csproj"
COPY . .
WORKDIR "/src/services/user.api"
RUN dotnet build "user.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "user.api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "user.api.dll"]