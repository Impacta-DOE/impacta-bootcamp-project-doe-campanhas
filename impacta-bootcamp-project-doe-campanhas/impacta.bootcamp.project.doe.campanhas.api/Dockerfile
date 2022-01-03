#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["impacta.bootcamp.project.doe.campanhas.api/impacta.bootcamp.project.doe.campanhas.api.csproj", "impacta.bootcamp.project.doe.campanhas.api/"]
COPY ["impacta.bootcamp.project-doe.campanhas.ioc/impacta.bootcamp.project-doe.campanhas.ioc.csproj", "impacta.bootcamp.project-doe.campanhas.ioc/"]
COPY ["impacta.bootcamp.project-doe.campanhas.core/impacta.bootcamp.project-doe.campanhas.core.csproj", "impacta.bootcamp.project-doe.campanhas.core/"]
COPY ["impacta.bootcamp.project.doe.campanhas.application/impacta.bootcamp.project.doe.campanhas.application.csproj", "impacta.bootcamp.project.doe.campanhas.application/"]
COPY ["impacta.bootcamp.project-doe.campanhas.shared/impacta.bootcamp.project-doe.campanhas.shared.csproj", "impacta.bootcamp.project-doe.campanhas.shared/"]
COPY ["impacta.bootcamp.project-doe.campanhas.infra.data/impacta.bootcamp.project-doe.campanhas.infra.data.csproj", "impacta.bootcamp.project-doe.campanhas.infra.data/"]
RUN dotnet restore "impacta.bootcamp.project.doe.campanhas.api/impacta.bootcamp.project.doe.campanhas.api.csproj"
COPY . .
WORKDIR "/src/impacta.bootcamp.project.doe.campanhas.api"
RUN dotnet build "impacta.bootcamp.project.doe.campanhas.api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "impacta.bootcamp.project.doe.campanhas.api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "impacta.bootcamp.project.doe.campanhas.api.dll"]
