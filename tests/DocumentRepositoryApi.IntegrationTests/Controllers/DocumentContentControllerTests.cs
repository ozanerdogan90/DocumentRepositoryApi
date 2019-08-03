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
    public class DocumentContentControllerTests
    {
        private readonly HttpClient httpClient;
        private readonly TestServer server;
        public DocumentContentControllerTests()
        {
            server = new TestServer(new WebHostBuilder()
                     .UseStartup<TestStartup>());

            httpClient = server.CreateClient();
        }

        [Fact]
        public async Task Get_WhenDocumentDoesntExist_ThenReturnsNotFound()
        {
            var expected = await httpClient.GetAsync($"/documents/{Guid.NewGuid()}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_WhenDocumentExist_ThenReturnsContent()
        {
            var id = await AddNewDocument();
            await AddNewContent(id);
            var expected = await httpClient.GetAsync($"/documents/{id}/contents");
            expected.StatusCode.Should().Be((int)HttpStatusCode.OK);
        }

        [Fact]
        public async Task Post_WhenDocumentDoesntExist_ThenReturnsBadRequest()
        {
            var expected = await httpClient.PostAsync($"/documents/{Guid.NewGuid()}/contents", FormDataHelper.FormData);
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_WhenNewFilePosted_ThenSavesToRepository()
        {
            var id = await AddNewDocument();
            await AddNewContent(id);
        }

        private async Task AddNewContent(Guid documentId)
        {
            var expected = await httpClient.PostAsync($"/documents/{documentId}/contents", FormDataHelper.FormData);
            expected.IsSuccessStatusCode.Should().Be(true);
        }

        private async Task<Guid> AddNewDocument()
        {
            var response = await httpClient.PostAsJsonAsync<Document>("/documents", DocumentHelper.ValidDocument);
            response.StatusCode.Should().Be((int)HttpStatusCode.Created);

            return await response.Content.ReadAsAsync<Guid>();
        }
    }
}
