using DocumentRepositoryApi.IntegrationTests.Helpers.Models;
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
    public class DocumentControllerTests
    {
        private readonly HttpClient httpClient;
        private readonly TestServer server;
        public DocumentControllerTests()
        {
            server = new TestServer(new WebHostBuilder()
                     .UseStartup<TestStartup>());

            httpClient = server.CreateClient();
        }

        [Fact]
        public async Task Get_WhenEmptyDocumentIdSent_ThenThrowsError()
        {
            var expected = await httpClient.GetAsync($"/documents/{Guid.Empty}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_WhenEmptyPayloadSent_ThenThrowsError()
        {
            var expected = await httpClient.PostAsJsonAsync<Document>("/documents", null);
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_WhenEmptyPayloadSent_ThenThrowsError()
        {
            var expected = await httpClient.PutAsJsonAsync<Document>($"/documents/{Guid.NewGuid()}", null);
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Put_WhenEmptyDocumentIdSent_ThenThrowsError()
        {
            var expected = await httpClient.PutAsJsonAsync<Document>($"/documents/{Guid.Empty}", DocumentHelper.ValidDocument);
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WhenEmptyDocumentIdSent_ThenThrowsError()
        {
            var expected = await httpClient.DeleteAsync($"/documents/{Guid.Empty}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }
    }
}
