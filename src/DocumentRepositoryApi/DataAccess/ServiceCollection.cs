using Amazon;
using Amazon.Runtime;
using Amazon.S3;
using DocumentRepositoryApi.DataAccess.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace DocumentRepositoryApi.DataAccess
{
    public static class ServiceCollection
    {
        private const string amazons3 = "amazons3storage";
        private const string inmemoryStorage = "inmemorystorage";
        private const string fileStorage = "filestorage";

        public static IServiceCollection AddRepositories(this IServiceCollection services, IConfiguration config)
        {
            return services.RegisterContentRepository(config)
                        .AddScoped<IUserRepository, UserRepository>()
                        .AddScoped<IDocumentRepository, DocumentRepository>();
        }

        private static IServiceCollection RegisterContentRepository(this IServiceCollection services, IConfiguration configuration)
        {
            var repositoryProvider = configuration["Repository:Provider"] ?? throw new ArgumentNullException("Repository:Provider");
            switch (repositoryProvider.ToLowerInvariant())
            {
                case amazons3:
                    {
                        RegisterAmazonS3Services(services, configuration);
                        services.AddTransient<IDocumentContentRepository, AmazonS3FileRepository>();
                        return services;
                    }
                case inmemoryStorage:
                    {
                        services.AddSingleton<IDocumentContentRepository, InMemoryStorageRepository>();
                        return services;
                    }
                case fileStorage:
                    {
                        services.AddTransient<IDocumentContentRepository, DiskFileRepository>();
                        return services;
                    }
                default:
                    throw new ArgumentNullException(nameof(repositoryProvider));
            }

        }

        private static void RegisterAmazonS3Services(IServiceCollection services, IConfiguration configuration)
        {
            var accessKey = configuration["AWS:Credentials:AccessKey"] ?? throw new ArgumentNullException("AwsAccessKey");
            var secretKey = configuration["AWS:Credentials:SecretKey"] ?? throw new ArgumentNullException("AWSSecretKey");
            var region = configuration["AWS:Region"] ?? throw new ArgumentNullException("AWSRegion");

            var credential = new BasicAWSCredentials(accessKey, secretKey);
            var client = new AmazonS3Client(credential, RegionEndpoint.GetBySystemName(region));
            services.AddSingleton<IAmazonS3>(client);
        }

    }
}
