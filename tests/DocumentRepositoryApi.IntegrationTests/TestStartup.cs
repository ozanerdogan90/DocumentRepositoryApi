using System;
using DocumentRepositoryApi.DataAccess;
using DocumentRepositoryApi.IntegrationTests.Helpers.Auth;
using DocumentRepositoryApi.IntegrationTests.Helpers.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DocumentRepositoryApi.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
            env.ApplicationName = "DocumentRepositoryApi";
        }

        public override void ConfigureDatabase(IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<DocumentContext>(options =>
             options.UseInMemoryDatabase(config.GetValue<string>("Database:InMemoryDb:Name", "DocumentRepositoryTest")));
        }

        public override void ConfigureAuth(IServiceCollection services)
        {
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = "Test Scheme";
                options.DefaultChallengeScheme = "Test Scheme";
            }).AddTestAuth(o => { });
        }
    }
}
