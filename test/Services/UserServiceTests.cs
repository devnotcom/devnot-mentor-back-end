using AutoFixture.Xunit2;
using AutoMapper;
using DevnotMentor.Configuration.Context;
using DevnotMentor.Common.Requests.User;
using DevnotMentor.Data.Entities;
using DevnotMentor.Data.Interfaces;
using DevnotMentor.Business.Services;
using DevnotMentor.Business.Services.Interfaces;
using DevnotMentor.Test.Base;
using DevnotMentor.Business.Utilities.Email;
using DevnotMentor.Business.Utilities.File;
using DevnotMentor.Business.Utilities.Security.Hash;
using DevnotMentor.Business.Utilities.Security.Token;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DevnotMentor.Test.Services
{
    public class UserServiceTests
    {
        protected static MockRepository MockRepository { get; } = new MockRepository(MockBehavior.Loose);

        protected Mock<IMapper> mockMapper = MockRepository.Create<IMapper>();
        protected Mock<ITokenService> mockTokenService = MockRepository.Create<ITokenService>();
        protected Mock<IHashService> mockHashService = MockRepository.Create<IHashService>();
        protected Mock<IMailService> mockMailService = MockRepository.Create<IMailService>();
        protected Mock<IUserRepository> mockUserRepository = MockRepository.Create<IUserRepository>();
        protected Mock<ILogRepository> mockLoggerRepository = MockRepository.Create<ILogRepository>();
        protected Mock<IFileService> mockFileService = MockRepository.Create<IFileService>();
        protected Mock<IDevnotConfigurationContext> mockConfigurationContext = MockRepository.Create<IDevnotConfigurationContext>();

        public UserServiceTests()
        {

        }

        protected IUserService CreateUserService()
        {
            return new UserService(
                mockMapper.Object,
                mockTokenService.Object,
                mockHashService.Object,
                mockMailService.Object,
                mockUserRepository.Object,
                mockLoggerRepository.Object,
                mockFileService.Object,
                mockConfigurationContext.Object);
        }

        [Theory, AutoMoqData]
        public async Task RegisterAsync_ShouldBeSucceeded_WhenFileUploadSucceeded(RegisterUserRequest request)
        {
            // Arrange
            var userService = CreateUserService();

            mockFileService
                .Setup(x => x.InsertProfileImageAsync(It.IsAny<IFormFile>()))
                .Returns(() => Task.FromResult(new FileResult(true, string.Empty, "file.png", "/file.png")));

            // Act
            var result = await userService.RegisterAsync(request);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public async Task RegisterAsync_ShouldBeFailed_WhenFileUploadFailed(RegisterUserRequest request)
        {
            // Arrange
            var userService = CreateUserService();

            var message = "File couldn't be uploaded!";

            mockFileService
                .Setup(x => x.InsertProfileImageAsync(It.IsAny<IFormFile>()))
                .Returns(() => Task.FromResult(new FileResult(false, message, string.Empty, string.Empty)));

            // Act
            var result = await userService.RegisterAsync(request);

            // Assert
            result.Message.Should().Be(message);
            result.Success.Should().BeFalse();
        }

        [Theory, AutoMoqData]
        public async Task LoginAsync_ShouldBeSucceeded_WhenPasswordIsCorrect([Frozen] User user, [Frozen] TokenInfo tokenInfo)
        {
            // Arrange
            var userService = CreateUserService();

            var request = new UserLoginRequest
            {
                UserName = user.UserName,
                Password = "SomeStr0ngPassw0rd"
            };

            mockHashService.Setup(x => x.CreateHash(request.Password)).Returns(user.Password);
            mockUserRepository.Setup(x => x.GetAsync(request.UserName, user.Password)).Returns(Task.FromResult(user));
            mockTokenService.Setup(x => x.CreateToken(user.Id, user.UserName)).Returns(tokenInfo);

            // Act
            var result = await userService.LoginAsync(request);

            // Assert
            result.Success.Should().BeTrue();
        }

        [Theory, AutoMoqData]
        public async Task LoginAsync_ShouldBeFailed_WhenPasswordIsIncorrect([Frozen] User user)
        {
            // Arrange
            var userService = CreateUserService();

            var hashedPassword = "hashed_1231231231";
            var request = new UserLoginRequest
            {
                UserName = user.UserName,
                Password = "SomeStr0ngPassw0rd"
            };

            mockHashService.Setup(x => x.CreateHash(request.Password)).Returns(hashedPassword);
            mockUserRepository.Setup(x => x.GetAsync(request.UserName, user.Password)).Returns(Task.FromResult(user));

            // Act
            var result = await userService.LoginAsync(request);

            // Assert
            result.Success.Should().BeFalse();
        }
    }
}
