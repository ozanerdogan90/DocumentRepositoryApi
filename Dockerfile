FROM mcr.microsoft.com/dotnet/core/sdk:2.2.300 AS build-env
WORKDIR /app
 
# Copy csproj and restore as distinct layers
COPY src/DocumentRepositoryApi/DocumentRepositoryApi.csproj ./
RUN dotnet restore
 
# Copy everything else
COPY . ./

# build and run tests
WORKDIR /app/src/DocumentRepositoryApi
RUN dotnet build


# publish
WORKDIR /app/src/DocumentRepositoryApi

RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/core/aspnet:2.2.5 AS runtime
WORKDIR /app
COPY --from=build-env /app/src/DocumentRepositoryApi/out .

RUN mkdir -p /log
EXPOSE 5001/tcp
ENV ASPNETCORE_URLS http://*:5001

CMD ["dotnet", "DocumentRepositoryApi.dll"]
