using DocumentRepositoryApi.DataAccess.Entities;
using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Services;
using DocumentRepositoryApi.Services.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocumentRepositoryApi.UnitTests.Services
{
    public class DocumentContentServiceTests
    {

        [Fact]
        public async Task Get_WhenEmptyContent_ReturnsNull()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IDocumentContentRepository>();
            repositoryMock.Setup(x => x.Fetch(It.IsAny<Guid>())).ReturnsAsync(default(DocumentContent));
            encMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var sut = new DocumentContentService(new CompressionService(), encMock.Object, repositoryMock.Object);

            var expected = await sut.Get(Guid.Empty);
            expected.Should().BeNull();
        }

        [Fact]
        public async Task Get_WhenSuccess_ReturnsContent()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IDocumentContentRepository>();
            repositoryMock.Setup(x => x.Fetch(It.IsAny<Guid>())).ReturnsAsync(new DocumentContent());
            encMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var sut = new DocumentContentService(new CompressionService(), encMock.Object, repositoryMock.Object);

            var expected = await sut.Get(Guid.Empty);
            expected.Should().NotBeNull();
        }

        [Fact]
        public async Task Store_WhenSuccess_ReturnsContent()
        {
            var formFile = new Mock<IFormFile>();
            var encMock = new Mock<IEncryptionService>();
            var compMock = new Mock<ICompressionService>();
            var repositoryMock = new Mock<IDocumentContentRepository>();
            repositoryMock.Setup(x => x.Store(It.IsAny<DocumentContent>())).ReturnsAsync(true);
            encMock.Setup(x => x.Encrypt(It.IsAny<byte[]>())).Returns(new byte[1]);
            compMock.Setup(x => x.Compress(It.IsAny<IFormFile>())).Returns(new byte[1]);
            formFile.Setup(x => x.FileName).Returns("aa.txt");

            var sut = new DocumentContentService(compMock.Object, encMock.Object, repositoryMock.Object);

            var expected = await sut.Store(Guid.Empty, formFile.Object);
            expected.Should().BeTrue();
        }
    }
}
