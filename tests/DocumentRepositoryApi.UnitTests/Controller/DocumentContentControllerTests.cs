using DocumentRepositoryApi.Controllers;
using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocumentRepositoryApi.UnitTests.Controller
{
    public class DocumentContentControllerTests
    {
        [Fact]
        public async Task Get_WhenDocumentIsMissing_ThenReturnsBadRequest()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(default(Document));

            var sut = new DocumentContentController(serviceMock.Object, null);
            var expected = await sut.Get(Guid.Empty);

            expected.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Get_WhenContentIsMissing_ThenReturnsInternalServerError()
        {
            var serviceMock = new Mock<IDocumentService>();
            var contentServiceMock = new Mock<IDocumentContentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new Document());
            contentServiceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(default(DocumentContent));

            var sut = new DocumentContentController(serviceMock.Object, contentServiceMock.Object);
            var expected = await sut.Get(Guid.Empty);

            expected.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }


        [Fact]
        public async Task Get_WhenSuccess_ThenReturnsFile()
        {
            var serviceMock = new Mock<IDocumentService>();
            var contentServiceMock = new Mock<IDocumentContentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new Document());
            contentServiceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new DocumentContent() { Content = new byte[1], Name = "aa.text", ContentType = "text/plain" });

            var sut = new DocumentContentController(serviceMock.Object, contentServiceMock.Object);
            var expected = await sut.Get(Guid.Empty);

            expected.Should().BeOfType<FileContentResult>();
        }

        [Fact]
        public async Task Post_WhenDocumentIsMissing_ThenReturnsBadRequest()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(default(Document));

            var sut = new DocumentContentController(serviceMock.Object, null);
            var expected = await sut.Post(Guid.Empty, null);

            expected.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public async Task Post_WhenFailure_ThenReturnsInternalServerError()
        {
            var serviceMock = new Mock<IDocumentService>();
            var contentServiceMock = new Mock<IDocumentContentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new Document());
            contentServiceMock.Setup(x => x.Store(It.IsAny<Guid>(), It.IsAny<IFormFile>())).ReturnsAsync(false);

            var sut = new DocumentContentController(serviceMock.Object, contentServiceMock.Object);
            var expected = await sut.Post(Guid.Empty, null);

            expected.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Get_WhenFailure_ThenReturnsInternalServerError()
        {
            var serviceMock = new Mock<IDocumentService>();
            var contentServiceMock = new Mock<IDocumentContentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new Document());
            contentServiceMock.Setup(x => x.Store(It.IsAny<Guid>(), It.IsAny<IFormFile>())).ReturnsAsync(true);

            var sut = new DocumentContentController(serviceMock.Object, contentServiceMock.Object);
            var expected = await sut.Post(Guid.Empty, null);

            expected.Should().BeOfType<OkResult>();
        }

    }
}
