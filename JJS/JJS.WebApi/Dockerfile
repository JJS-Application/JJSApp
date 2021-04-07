#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["JJS/JJS.WebApi/JJS.WebApi.csproj", "JJS/JJS.WebApi/"]
COPY ["JJS/JJS.Infrastructure.Identity/JJS.Infrastructure.Identity.csproj", "JJS/JJS.Infrastructure.Identity/"]
COPY ["JJS/JJS.Application/JJS.Application.csproj", "JJS/JJS.Application/"]
COPY ["JJS/JJS.Domain/JJS.Domain.csproj", "JJS/JJS.Domain/"]
COPY ["JJS/JJS.Infrastructure.Shared/JJS.Infrastructure.Shared.csproj", "JJS/JJS.Infrastructure.Shared/"]
COPY ["JJS/JJS.Infrastructure.Persistence/JJS.Infrastructure.Persistence.csproj", "JJS/JJS.Infrastructure.Persistence/"]
RUN dotnet restore "JJS/JJS.WebApi/JJS.WebApi.csproj"
COPY . .
WORKDIR "/src/JJS/JJS.WebApi"
RUN dotnet build "JJS.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "JJS.WebApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "JJS.WebApi.dll"]