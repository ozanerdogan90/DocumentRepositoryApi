using DocumentRepositoryApi.Controllers;
using DocumentRepositoryApi.Models;
using DocumentRepositoryApi.Services;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace DocumentRepositoryApi.UnitTests.Controller
{
    public class AuthControllerTests
    {
        [Fact]
        public async Task GenerateToken_WhenUserServiceReturnsEmpty_ThenReturnsBadRequest()
        {
            var serviceMock = new Mock<IAuthService>();
            serviceMock.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(string.Empty);

            var sut = new AuthController(serviceMock.Object, null);

            var expected = await sut.GenerateToken(new Models.Login());

            expected.Should().BeOfType<BadRequestResult>();
        }

        [Fact]
        public async Task GenerateToken_WhenCredentialSucceeds_ThenReturnsOkWithToken()
        {
            var serviceMock = new Mock<IAuthService>();
            serviceMock.Setup(x => x.GenerateToken(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync("IAMTOKEN");

            var sut = new AuthController(serviceMock.Object, null);

            var expected = await sut.GenerateToken(new Models.Login());

            expected.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public async Task Register_WhenSucceeds_ThenReturnsOk()
        {
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(x => x.Register(It.IsAny<User>())).ReturnsAsync(true);

            var sut = new AuthController(null, serviceMock.Object);

            var expected = await sut.Register(new User());

            expected.Should().BeOfType<OkResult>();
        }

        [Fact]
        public async Task Register_WhenFailure_ThenReturnsInternalServerError()
        {
            var serviceMock = new Mock<IUserService>();
            serviceMock.Setup(x => x.Register(It.IsAny<User>())).ReturnsAsync(false);

            var sut = new AuthController(null, serviceMock.Object);

            var expected = await sut.Register(new User());

            expected.Should().BeOfType<StatusCodeResult>();
            expected.As<StatusCodeResult>().StatusCode.Should().Be((int)HttpStatusCode.InternalServerError);
        }
    }
}
