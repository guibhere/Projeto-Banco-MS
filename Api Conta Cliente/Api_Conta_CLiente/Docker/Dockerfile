FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://*:80
    
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["*.csproj", "Api_Conta_Cliente/"]
RUN dotnet restore "Api_Conta_Cliente/Api_Conta_Cliente.csproj"
COPY . ./Api_Conta_Cliente
WORKDIR "/src/Api_Conta_Cliente"
RUN dotnet build "Api_Conta_Cliente.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Api_Conta_Cliente.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN useradd -m myappuser
USER myappuser

ENTRYPOINT ["dotnet","Api_Conta_Cliente.dll"]    