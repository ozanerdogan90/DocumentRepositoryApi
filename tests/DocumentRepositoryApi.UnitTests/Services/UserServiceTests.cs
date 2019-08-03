using AutoMapper;
using DocumentRepositoryApi.DataAccess.Entities;
using DocumentRepositoryApi.DataAccess.Repositories;
using DocumentRepositoryApi.Services;
using DocumentRepositoryApi.Services.Helpers;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DocumentRepositoryApi.UnitTests.Services
{
    public class UserServiceTests
    {
        private readonly IMapper _mapper;
        public UserServiceTests()
        {
            var mappingConfig = new MapperConfiguration(mc =>
            {
                mc.AddProfile(new AutoMapperProfile());
            });

            _mapper = mappingConfig.CreateMapper();
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Register_WhenSuccess_ThenReturnsTrue(bool actual)
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.Add(It.IsAny<User>())).ReturnsAsync(actual);
            encMock.Setup(x => x.Encrypt(It.IsAny<string>())).Returns(string.Empty);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);
            var expected = await sut.Register(new Models.User());

            expected.Should().Be(actual);
        }

        [Fact]
        public async Task Get_WhenFailure_ThenReturnsNull()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(default(User));
            encMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);
            var expected = await sut.Get(string.Empty);

            expected.Should().BeNull();
        }

        [Fact]
        public async Task Get_WhenSuccess_ThenReturnsUser()
        {
            var encMock = new Mock<IEncryptionService>();
            var repositoryMock = new Mock<IUserRepository>();
            repositoryMock.Setup(x => x.Get(It.IsAny<string>())).ReturnsAsync(new User());
            encMock.Setup(x => x.Decrypt(It.IsAny<string>())).Returns(string.Empty);

            var sut = new UserService(encMock.Object, repositoryMock.Object, _mapper);
            var expected = await sut.Get(string.Empty);

            expected.Should().NotBeNull();
        }
    }
}
