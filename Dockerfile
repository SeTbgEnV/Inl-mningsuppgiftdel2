FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /app

COPY *.csproj ./

RUN dotnet restore

COPY . .

RUN dotnet publish -c Release -o Release

FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime

WORKDIR /app

COPY --from=build /app/Release ./

EXPOSE 8080

ENTRYPOINT ["dotnet", "MormorDagnysDel2.1.dll"]