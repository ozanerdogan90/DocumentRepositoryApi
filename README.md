# Document Repository Api

> .Net Core 2.2, PostgreSql, Amazon S3, Xunit, Docker  

## Description
This project aims to create a document repository to save file on Amazon S3 or shared disk space or inmemory

## Features
##### Framework
- .Net Core
##### Storage
- PostgreSql
##### Authentication
- Jwt authentication
##### Integration testing
- Xunit
- Fluent Assertions
- Moq

## Requirements

- .Net Core >= 2.2
- Docker

## Running the API
### Development
To start the application in development mode, run:

```cmd
dotnet build
cd src\DocumentRepositoryApi
dotnet run
```
Application will be served on route: 
http://localhost:5000

To start the application in docker container:
```cmd
docker-compose up
```
Docker will spin up application, postgreSql container and PgAdmin to manage database

## Swagger
Swagger documentation will be available on route: 
```bash
http://localhost:5000/swagger
```

### Testing
To run integration tests: 
```bash
dotnet test ./tests/DocumentRepositoryApi.IntegrationTests/DocumentRepositoryApi.IntegrationTests.csproj
```
To run unit tests: 
```bash
dotnet test ./tests/DocumentRepositoryApi.UnitTests/DocumentRepositoryApi.UnitTests.csproj
```

To run unit and integration tests with script: 
```bash
scripts/tests.sh
```

## Set up environment
Keys and the secrets are defined in user secret file. More information can be found [.net core user secrets](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-2.2&tabs=windows)
This application uses "8c43a081-db6b-43eb-8376-df1651b2d72a" as secret key id.

Application uses PostgreSql as database. Connection string needs to be defined in user secret
```bash
ConnectionStrings": {
    "PostgreSql": "Your connection string"
  },
```
#### To configure Amazon S3 as your file storage repository:
AmazonS3 should be selected as datasource in appsettings:
```bash
 "Repository": {
    "Provider": "amazons3storage"
  }
```

Amazon S3 credential informations should be defined in user secret
```bash
 "AWS": {
    "BucketName": "Your Bucket Name",
    "Region": "Region Name",
	"Credentials":{
	"AccessKey" :"XXXX",
	"SecretKey": "XXXX"
	}
  }
```

#### To configure InMemory storage:
```bash
 "Repository": {
    "Provider": "inmemorystorage"
  }
```

#### To configure Disk file storage:

appsettings:
```bash
 "Repository": {
    "Provider": "filestorage"
  }
```

user secret:
```bash
 "DocumentApi": {
    "ContentPath": "shared file storage path"
  }
```

#### To configure Compression key hash:
user secret:
```bash
 "Encryption": {
    "Key": "Base64 key"
  }

#### To configure Jwt secret:
user secret:
```bash
 "Jwt": {
    "Secret": "this is my custom Secret key for authnetication"
  }