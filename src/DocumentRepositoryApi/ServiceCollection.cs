using AutoMapper;
using CorrelationId;
using Hellang.Middleware.ProblemDetails;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

namespace DocumentRepositoryApi
{
    public static class ServiceCollection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
        {
            services.AddDataProtectionServices()
                       .AddSingleton(config)
                       .AddCorrelationIdServices()
                       .AddAutoMapperServices()
                       .AddSwaggerServices()
                       .AddResponseCompression()
                       .AddProblemDetailServices();
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
            return services;
        }

        private static IServiceCollection AddCorrelationIdServices(this IServiceCollection services)
        {
            services.AddCorrelationId();
            return services;
        }

        private static IServiceCollection AddProblemDetailServices(this IServiceCollection services)
        {
            services.AddProblemDetails();
            return services;
        }

        private static IServiceCollection AddDataProtectionServices(this IServiceCollection services)
        {
            services.AddDataProtection()
                       .UseCryptographicAlgorithms(
                       new AuthenticatedEncryptorConfiguration()
                       {
                           EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                           ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                       });

            return services;
        }

        private static IServiceCollection AddAutoMapperServices(this IServiceCollection services)
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            IMapper mapper = mappingConfig.CreateMapper();
            services.AddSingleton(mapper);
            return services;
        }
        private static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            return services.AddSwaggerGen(c =>
             {
                 c.SwaggerDoc("v1", new Info
                 {
                     Version = "v1",
                     Title = "Document Repository Api",
                     Description = "A simple api to store your documents",
                 });
                 var security = new Dictionary<string, IEnumerable<string>>
                 {
                    {"Bearer", new string[] { }},
                 };

                 c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                 {
                     Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                     Name = "Authorization",
                     In = "header",
                     Type = "apiKey"
                 });
                 c.AddSecurityRequirement(security);
                 var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                 var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                 c.IncludeXmlComments(xmlPath);
             });
        }
    }
}
