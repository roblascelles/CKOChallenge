#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CheckoutChallenge.WebAPI/CheckoutChallenge.WebAPI.csproj", "src/CheckoutChallenge.WebAPI/"]
COPY ["src/CheckoutChallenge.DataStores.InMemory/CheckoutChallenge.DataStores.InMemory.csproj", "src/CheckoutChallenge.DataStores.InMemory/"]
COPY ["src/CheckoutChallenge.Application/CheckoutChallenge.Application.csproj", "src/CheckoutChallenge.Application/"]
COPY ["src/CheckoutChallenge.Acquirers.Faked/CheckoutChallenge.Acquirers.Faked.csproj", "src/CheckoutChallenge.Acquirers.Faked/"]
RUN dotnet restore "src/CheckoutChallenge.WebAPI/CheckoutChallenge.WebAPI.csproj"
COPY . .
WORKDIR "/src/src/CheckoutChallenge.WebAPI"
RUN dotnet build "CheckoutChallenge.WebAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CheckoutChallenge.WebAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CheckoutChallenge.WebAPI.dll"]