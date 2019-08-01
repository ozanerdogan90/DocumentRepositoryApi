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
        public async Task Get_WhenEmptyDocumentIdSent_ThenReturnsBadRequest()
        {
            var expected = await httpClient.GetAsync($"/documents/{Guid.Empty}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Get_WhenInvalidDocumentIdSent_ThenReturnsNotFound()
        {
            var expected = await httpClient.GetAsync($"/documents/{Guid.NewGuid()}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task Get_WhenIdExist_ThenReturnsDocument()
        {
            var id = await AddNewDocument();
            var expected = await httpClient.GetAsync($"/documents/{id}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var entity = await expected.Content.ReadAsAsync<Document>();
            entity.Should().NotBeNull();
            entity.Name.Should().NotBeEmpty();
            entity.Title.Should().NotBeEmpty();
            entity.Version.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Post_WhenEmptyPayloadSent_ThenReturnsBadRequest()
        {
            var expected = await httpClient.PostAsJsonAsync<Document>("/documents", null);
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Post_WhenSuccessfulyInserted_ThenCreatesEntity()
        {
            Guid expected = await AddNewDocument();
            expected.Should().NotBe(Guid.Empty);
        }

        [Fact]
        public async Task Put_WhenEmptyPayloadSent_ThenReturnsBadRequest()
        {
            var expected = await httpClient.PutAsJsonAsync<Document>($"/documents/{Guid.NewGuid()}", null);
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }


        [Fact]
        public async Task Put_WhenEmptyDocumentIdSent_ThenReturnsBadRequest()
        {
            var expected = await httpClient.PutAsJsonAsync<Document>($"/documents/{Guid.Empty}", DocumentHelper.ValidDocument);
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Put_WhenIdExist_ThenUpdatesDocument()
        {
            var id = await AddNewDocument();
            var expected = await httpClient.PutAsJsonAsync<Document>($"/documents/{id}", DocumentHelper.ValidDocumentUpdate);
            expected.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var entity = await expected.Content.ReadAsAsync<Document>();
            entity.Should().NotBeNull();
            entity.Name.Should().Be(DocumentHelper.ValidDocumentUpdate.Name);
            entity.Version.Should().NotBeEmpty();
        }

        [Fact]
        public async Task Delete_WhenEmptyDocumentIdSent_ThenReturnsBadRequest()
        {
            var expected = await httpClient.DeleteAsync($"/documents/{Guid.Empty}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task Delete_WhenExecutedSuccessfuly_ThenDeletesFile()
        {
            var id = await AddNewDocument();
            var expected = await httpClient.DeleteAsync($"/documents/{id}");
            expected.StatusCode.Should().Be((int)HttpStatusCode.OK);

            var doc = await httpClient.GetAsync($"/documents/{id}");
            doc.StatusCode.Should().Be((int)HttpStatusCode.NotFound);
        }


        private async Task<Guid> AddNewDocument()
        {
            var response = await httpClient.PostAsJsonAsync<Document>("/documents", DocumentHelper.ValidDocument);
            response.StatusCode.Should().Be((int)HttpStatusCode.Created);

            return await response.Content.ReadAsAsync<Guid>();
        }

    }
}
