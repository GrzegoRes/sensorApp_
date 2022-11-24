FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
 WORKDIR /app
 EXPOSE 80
 FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
 WORKDIR /src
 COPY ["Generator/Generator/Generator.csproj", ""]
 RUN dotnet restore "./Generator.csproj"
 COPY . .
 WORKDIR "/src/."
 RUN dotnet build "Generator.csproj" -c Release -o /app/build
 FROM build AS publish
 RUN dotnet publish "Generator.csproj" -c Release -o /app/publish
 FROM base AS final
 WORKDIR /app
 COPY --from=publish /app/publish .
 ENTRYPOINT ["dotnet", "Generator.csproj.dll"]