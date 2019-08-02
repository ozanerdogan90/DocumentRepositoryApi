FROM microsoft/dotnet:2.1-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 80

FROM microsoft/dotnet:2.1-sdk AS build
WORKDIR /src
COPY ["DocumentRepositoryApi/DocumentRepositoryApi.csproj", "DocumentRepositoryApi/"]
RUN dotnet restore "DocumentRepositoryApi/DocumentRepositoryApi.csproj"
COPY . .
WORKDIR "/src/DocumentRepositoryApi"
RUN dotnet build "DocumentRepositoryApi.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "DocumentRepositoryApi.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "DocumentRepositoryApi.dll"]