﻿using F1Manager.Shared.Enums;
using F1Manager.Users.Abstractions;
using F1Manager.Users.Domain;
using F1Manager.Users.Exceptions;
using F1Manager.Users.UnitTests.Factories;
using Moq;
using Xunit;

namespace F1Manager.Users.UnitTests.Domain
{
    public class UserDomainTests : IClassFixture<UserDomainTests>
    {

        private readonly Mock<IUsersDomainService> _usersDomainServiceMock;


        [Fact]
        public void WhenDisplayNameIsValid_ItSucceeds()
        {
            var expectedName = "Peter Franssen";
            var user = WithRandomUser();
            user.SetDisplayName(expectedName);
            Assert.Equal(expectedName, user.DisplayName);
            Assert.Equal(TrackingState.Modified, user.TrackingState);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void WhenDisplayNameIsNullOrEmpty_ItThrowsF1ManagerUserException(string displayName)
        {
            var user = WithRandomUser();
            var exception = Assert.Throws<F1ManagerUserException>(() => user.SetDisplayName(displayName));
            Assert.Equal(exception.ErrorCode,UserErrorCode.DisplayNameNullOrEmpty);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }


        [Fact]
        public async void WhenUsernameIsValid_ItSucceeds()
        {
            var expectedUsername= "new@username.nl";
            var user = WithRandomUser();

            _usersDomainServiceMock.Setup(x => x.GetIsUsernameUnique(user.Id, expectedUsername))
                .ReturnsAsync(true);

            await user.SetUsername(expectedUsername, _usersDomainServiceMock.Object);
            Assert.Equal(expectedUsername, user.Username);
            Assert.Equal(TrackingState.Modified, user.TrackingState);
            _usersDomainServiceMock.Verify(x=> x.GetIsUsernameUnique(user.Id, expectedUsername), Times.Once);

        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public async void WhenUsernameIsNullOrEmpty_ItThrowsF1ManagerUserException(string username)
        {
            var user = WithRandomUser();
            var exception = await Assert.ThrowsAsync<F1ManagerUserException>(() => user.SetUsername(username, _usersDomainServiceMock.Object));
            Assert.Equal(exception.ErrorCode, UserErrorCode.UsernameNullOrEmpty);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }
        [Theory]
        [InlineData("tooshrt")]
        [InlineData("thisusernameistoolong")]
        [InlineData("__banana__")]
        public async void WhenUsernameIsInvalid_ItThrowsF1ManagerUserException(string username)
        {
            var user = WithRandomUser();
            var exception = await Assert.ThrowsAsync<F1ManagerUserException>(() => user.SetUsername(username, _usersDomainServiceMock.Object));
            Assert.Equal(exception.ErrorCode, UserErrorCode.InvalidUsername);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }
        [Fact]
        public async void WhenUsernameIsAlreadyTaken_ItThrowsF1ManagerUserException()
        {
            var username = "alreadytaken";
            var user = WithRandomUser();
            _usersDomainServiceMock.Setup(x => x.GetIsUsernameUnique(user.Id, username))
                .ReturnsAsync(false);
            var exception = await Assert.ThrowsAsync<F1ManagerUserException>(() => user.SetUsername(username, _usersDomainServiceMock.Object));
            _usersDomainServiceMock.Verify(x => x.GetIsUsernameUnique(user.Id, username), Times.Once);
            Assert.Equal(exception.ErrorCode, UserErrorCode.UsernameNotUnique);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }



        [Fact]
        public void WhenPasswordIsValid_ItSucceeds()
        {
            var expectedPassword = "SecretPassword01";
            var user = WithRandomUser();

             user.SetPassword(expectedPassword);
            Assert.True(user.Password.Validate(expectedPassword));
            Assert.Equal(TrackingState.Modified, user.TrackingState);
        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void WhenPasswordIsNullOrEmpty_ItThrowsF1ManagerUserException(string password)
        {
            var user = WithRandomUser();
            var exception = Assert.Throws<F1ManagerUserException>(() => user.SetPassword(password));
            Assert.Equal(exception.ErrorCode, UserErrorCode.PasswordNullOrEmpty);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }
        [Theory]
        [InlineData("tooshrt")]
        [InlineData("noNumbersInPassword")]
        [InlineData("nocapitals01")]
        [InlineData("NOLOWERCASES01")]
        public void WhenPasswordIsInvalid_ItThrowsF1ManagerUserException(string password)
        {
            var user = WithRandomUser();
            var exception = Assert.Throws<F1ManagerUserException>(() => user.SetPassword(password));
            Assert.Equal(exception.ErrorCode, UserErrorCode.InvalidPassword);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }


        [Fact]
        public void WhenEmailAddressIsValid_ItSucceeds()
        {
            var expectedEmailAddress = "new@email-address.nl";
            var user = WithRandomUser();

            user.SetEmailAddress(expectedEmailAddress);
            Assert.Equal(expectedEmailAddress, user.EmailAddress);
            Assert.Equal(TrackingState.Modified, user.TrackingState);

        }
        [Theory]
        [InlineData(null)]
        [InlineData("")]
        [InlineData(" ")]
        public void WhenEmailAddressIsNullOrEmpty_ItThrowsF1ManagerUserException(string emailAddress)
        {
            var user = WithRandomUser();
            var exception = Assert.Throws<F1ManagerUserException>(() => user.SetEmailAddress(emailAddress));
            Assert.Equal(exception.ErrorCode, UserErrorCode.EmailNullOrEmpty);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }
        [Theory]
        [InlineData("email@address")]
        [InlineData("@email-adress.nl")]
        [InlineData("email-address.nl")]
        [InlineData("email@addres nl")]
        public void WhenEmailAddressIsInvalid_ItThrowsF1ManagerUserException(string emailAddress)
        {
            var user = WithRandomUser();
            var exception = Assert.Throws<F1ManagerUserException>(() => user.SetEmailAddress(emailAddress));
            Assert.Equal(exception.ErrorCode, UserErrorCode.InvalidEmail);
            Assert.Equal(TrackingState.Pristine, user.TrackingState);
        }


        private User WithRandomUser()
        {
            return UsersFactory.Random();
        }

        public UserDomainTests()
        {
            _usersDomainServiceMock = new Mock<IUsersDomainService>();
        }

    }
}