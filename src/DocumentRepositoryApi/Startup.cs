using DocumentRepositoryApi.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using DocumentRepositoryApi.DataAccess;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using CorrelationId;
using Microsoft.Extensions.Logging;
using DocumentRepositoryApi.Middlewares;
using Hellang.Middleware.ProblemDetails;
using System;

namespace DocumentRepositoryApi
{
    public class Startup
    {
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
         .SetBasePath(env.ContentRootPath)
         .AddJsonFile("appsettings.json");

            builder.AddUserSecrets<Startup>();

            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            ConfigureDatabase(services, Configuration);
            ConfigureAuth(services);
            services.AddBusinessServices()
                    .AddRepositories(Configuration)
                    .AddApplicationServices(Configuration);
        }


        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory builder)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            builder.AddConsole();
            app.UseCors(x => x
                            .AllowAnyOrigin()
                            .AllowAnyMethod()
                            .AllowAnyHeader())
                .UseAuthentication()
                .UseMiddleware<ApiLoggingMiddleware>()
                .UseCorrelationId(new CorrelationIdOptions
                {
                    Header = "X-Correlation-ID",
                    UseGuidForCorrelationId = true,
                    UpdateTraceIdentifier = true,
                    IncludeInResponse = true
                })
                .UseSwagger()
                .UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Document Repository v1");
                c.DocumentTitle = "Document Repository Swagger Ui";
            })
                .UseResponseCompression()
                .UseProblemDetails()
                .UseMvc();
        }

        public virtual void ConfigureDatabase(IServiceCollection services, IConfiguration config)
        {
            var dbName = config.GetValue<string>("Database:InMemoryDb:Name", "DocumentRepository");
            services.AddDbContext<DocumentContext>(options =>
             options.UseInMemoryDatabase(dbName));
        }

        public virtual void ConfigureAuth(IServiceCollection services)
        {
            var key = Configuration["Jwt:Secret"] ?? throw new ArgumentNullException("JwtSecret");
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
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(key)),
             ValidateIssuer = false,
             ValidateAudience = false
         };
     });
        }
    }
}
