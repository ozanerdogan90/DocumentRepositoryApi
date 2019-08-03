using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using FluentAssertions;
using Microsoft.Extensions.Configuration;
using Moq;
using System.Threading.Tasks;
using Xunit;

namespace DocumentRepositoryApi.UnitTests.Services
{
    public class AuthServiceTests
    {

        [Fact]
        public async Task GenerateToken_WhenEmptyUser_ReturnsNull()
        {
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(default(User));

            var sut = new AuthService(null, serviceMock.Object);
            var expected = await sut.GenerateToken(string.Empty, string.Empty);

            expected.Should().BeNullOrEmpty();
        }
        [Fact]
        public async Task GenerateToken_WhenDifferentPasswords_ReturnsNull()
        {
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new User() { Password = "1123" });

            var sut = new AuthService(null, serviceMock.Object);
            var expected = await sut.GenerateToken(string.Empty, "112244");

            expected.Should().BeNullOrEmpty();
        }

        [Fact]
        public async Task GenerateToken_WhenSuccess_ReturnsToken()
        {
            var serviceMock = new Mock<IUserService>();
            var configMock = new Mock<IConfiguration>();
            var secretSectionMock = new Mock<IConfigurationSection>();
            var daySectionMock = new Mock<IConfigurationSection>();
            secretSectionMock.Setup(a => a.Value).Returns("this is my custom Secret key for authnetication");
            configMock.Setup(c => c.GetSection("Jwt:Secret")).Returns(secretSectionMock.Object);
            daySectionMock.Setup(a => a.Value).Returns("1");
            configMock.Setup(c => c.GetSection("Jwt:ExpiresDay")).Returns(daySectionMock.Object);

            serviceMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new User() { Password = "1123" });

            var sut = new AuthService(configMock.Object, serviceMock.Object);
            var expected = await sut.GenerateToken(string.Empty, "1123");

            expected.Should().NotBeNullOrEmpty();
        }
    }
}
