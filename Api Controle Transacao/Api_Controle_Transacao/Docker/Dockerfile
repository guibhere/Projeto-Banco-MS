FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 90
ENV ASPNETCORE_URLS=http://*:90
    
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["*.csproj", "Api_Controle_Transacao/"]
RUN dotnet restore "Api_Controle_Transacao/Api_Controle_Transacao.csproj"
COPY . ./Api_Controle_Transacao
WORKDIR "/src/Api_Controle_Transacao"
RUN dotnet build "Api_Controle_Transacao.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api_Controle_Transacao.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN useradd -m myappuser
USER myappuser

ENTRYPOINT ["dotnet","Api_Controle_Transacao.dll"]    