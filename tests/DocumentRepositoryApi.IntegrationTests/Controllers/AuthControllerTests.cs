using DocumentRepositoryApi.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocumentRepositoryApi.IntegrationTests.Controllers
{

    public class AuthControllerTests : IDisposable
    {
        private readonly HttpClient httpClient;
        private readonly TestServer server;
        public AuthControllerTests()
        {
            server = new TestServer(new WebHostBuilder()
                     .UseStartup<TestStartup>());

            httpClient = server.CreateClient();
        }
        public void Dispose()
        {
            AutoMapper.Mapper.Reset();
        }

        [Fact]
        public async Task Register_WhenRegistered_ReturnsOk()
        {
            var expected = await httpClient.PostAsJsonAsync<User>("/register", new User() { Email = "aa@bb.com", Password = "112233" });
            expected.IsSuccessStatusCode.Should().BeTrue();
        }

        [Fact]
        public async Task GenerateToken_WhenInvalidPassword_ReturnsBadRequest()
        {
            await httpClient.PostAsJsonAsync<User>("/register", new User() { Email = "aa@bb.com", Password = "112233" });

            var expected = await httpClient.PostAsJsonAsync<Login>("/token", new Login() { Email = "aa@bb.com", Password = "332211" });
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GenerateToken_WhenPasswordMatches_ReturnsToken()
        {
            await httpClient.PostAsJsonAsync<User>("/register", new User() { Email = "aa@bb.com", Password = "112233" });

            var expected = await httpClient.PostAsJsonAsync<Login>("/token", new Login() { Email = "aa@bb.com", Password = "112233" });
            expected.IsSuccessStatusCode.Should().BeTrue();
            var token = await expected.Content.ReadAsStringAsync();
            token.Should().NotBeEmpty();
        }
    }
}
