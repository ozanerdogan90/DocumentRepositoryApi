using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;

namespace DocumentRepositoryApi.IntegrationTests
{
    public class TestStartup : Startup
    {
        public TestStartup(IHostingEnvironment env) : base(env)
        {
            env.ApplicationName = "DocumentRepositoryApi";
        }
    }
}
