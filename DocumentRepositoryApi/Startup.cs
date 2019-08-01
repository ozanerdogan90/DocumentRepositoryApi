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

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            InitializeMapper();

            services.AddTransient<IDocumentService, DocumentService>();
            services.AddTransient<IDocumentContentService, DocumentContentService>();

            services.AddTransient<IDocumentRepository, DocumentRepository>();
            ConfigureDatabase(services, Configuration);
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseMvc();
        }

        public void InitializeMapper()
        {
            Mapper.Reset();
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            });

        }

        public virtual void ConfigureDatabase(IServiceCollection services, IConfigurationRoot config)
        {
            var dbName = config.GetValue<string>("Database:InMemoryDb:Name", "DocumentRepository");
            services.AddDbContext<DocumentContext>(options =>
             options.UseInMemoryDatabase(dbName));
        }
    }
}
