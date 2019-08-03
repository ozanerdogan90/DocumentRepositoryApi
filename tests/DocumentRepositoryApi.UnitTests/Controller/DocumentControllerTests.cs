using DocumentRepositoryApi.Controllers;
using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using FluentAssertions;
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
    public class DocumentControllerTests
    {
        [Fact]
        public async Task Get_WhenEmptyResult_ThenReturnsNotFound()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(default(Document));

            var sut = new DocumentController(serviceMock.Object);
            var expected = await sut.Get(Guid.Empty);

            expected.Should().BeOfType<NotFoundResult>();
        }

        [Fact]
        public async Task Get_WhenSucceeds_ThenReturnsOkWithObject()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new Document());

            var sut = new DocumentController(serviceMock.Object);
            var expected = await sut.Get(Guid.Empty);

            expected.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Post_WhenSucceeds_ThenReturnsCreatedAt()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Add(It.IsAny<Document>())).ReturnsAsync(Guid.NewGuid());

            var sut = new DocumentController(serviceMock.Object);
            var expected = await sut.Post(new Document());

            expected.Should().BeOfType<CreatedAtActionResult>();
        }

        [Fact]
        public async Task Put_WhenEmptyDocument_ThenReturnsBadRequest()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(default(Document));

            var sut = new DocumentController(serviceMock.Object);
            var expected = await sut.Put(Guid.Empty, new Document());

            expected.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task Put_WhenFailure_ThenReturnsInternalServerError()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new Document());
            serviceMock.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Document>())).ReturnsAsync(false);

            var sut = new DocumentController(serviceMock.Object);
            var expected = await sut.Put(Guid.Empty, new Document());

            expected.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }

        [Fact]
        public async Task Put_WhenSuccess_ThenReturnsOk()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Get(It.IsAny<Guid>())).ReturnsAsync(new Document());
            serviceMock.Setup(x => x.Update(It.IsAny<Guid>(), It.IsAny<Document>())).ReturnsAsync(true);

            var sut = new DocumentController(serviceMock.Object);
            var expected = await sut.Put(Guid.Empty, new Document());

            expected.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Delete_WhenSuccess_ThenReturnsOk()
        {
            var serviceMock = new Mock<IDocumentService>();
            serviceMock.Setup(x => x.Delete(It.IsAny<Guid>())).ReturnsAsync(true);

            var sut = new DocumentController(serviceMock.Object);
            var expected = await sut.Delete(Guid.Empty);

            expected.Should().BeOfType<OkResult>();
        }
    }
}
