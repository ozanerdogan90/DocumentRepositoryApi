using System;
using System.Collections.Generic;
using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DocumentRepositoryApi.DataAccess;
using AutoMapper;
using DocumentRepositoryApi.DataAccess.Repositories;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption.ConfigurationModel;
using Microsoft.AspNetCore.DataProtection.AuthenticatedEncryption;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace DocumentRepositoryApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
         .SetBasePath(env.ContentRootPath)
         .AddJsonFile("appsettings.json");

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {

            services.AddDataProtection()
                        .UseCryptographicAlgorithms(
                        new AuthenticatedEncryptorConfiguration()
                        {
                            EncryptionAlgorithm = EncryptionAlgorithm.AES_256_CBC,
                            ValidationAlgorithm = ValidationAlgorithm.HMACSHA256
                        });
            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDocumentContentService, DocumentContentService>();

            services.AddTransient<IDocumentRepository, DocumentRepository>();
            services.AddSingleton<IDocumentContentRepository, InMemoryStorageRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddTransient<ICompressionService, CompressionService>();
            services.AddTransient<IEncryptionService, EncryptionService>();

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IAuthService, AuthService>();
            ConfigureDatabase(services, Configuration);
            services.AddSingleton(Configuration);

            ConfigureAuth(services);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);

        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            var key = Encoding.ASCII.GetBytes(Configuration.GetValue<string>("Jwt:Secret"));
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
     .AddJwtBearer(x =>
     {
         x.RequireHttpsMetadata = false;
         x.SaveToken = true;
         x.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuerSigningKey = true,
             IssuerSigningKey = new SymmetricSecurityKey(key),
             ValidateIssuer = false,
             ValidateAudience = false
         };
     });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            InitializeMapper();
            app.UseCors(x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader());

            app.UseAuthentication();
            app.UseMvc();
        }

        public void InitializeMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

        }

        public virtual void ConfigureDatabase(IServiceCollection services, IConfiguration config)
        {
            var dbName = config.GetValue<string>("Database:InMemoryDb:Name", "DocumentRepository");
            services.AddDbContext<DocumentContext>(options =>
             options.UseInMemoryDatabase(dbName));
        }
    }
}
