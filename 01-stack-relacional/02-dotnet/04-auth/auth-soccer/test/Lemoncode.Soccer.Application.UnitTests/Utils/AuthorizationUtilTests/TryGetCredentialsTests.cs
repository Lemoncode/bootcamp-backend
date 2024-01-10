using FluentAssertions;
using Lemoncode.Soccer.Application.Utils;
using Lemoncode.TestSupport;
using Xunit;

namespace Lemoncode.Soccer.Application.UnitTests.Utils.AuthorizationUtilTests
{
    public static class TryGetCredentialsTests
    {
        public class Given_A_Basic_Authorization_Header_With_Valid_Base64_Encoded_Credentials_When_Getting_Values
            : Given_When_Then_Test
        {
            private string _authHeader;
            private string _username;
            private string _password;
            private bool _result;
            private string _expectedUsername;
            private string _expectedPassword;

            protected override void Given()
            {
                _authHeader = "Basic YWRtaW46TGVtMG5Db2RlIQ=="; // Basic admin:Lem0nCode! in base64

                _expectedUsername = "admin";
                _expectedPassword = "Lem0nCode!";
            }

            protected override void When()
            {
                _result = AuthorizationUtil.TryGetCredentials(_authHeader, out _username, out _password);
            }

            [Fact]
            public void Then_It_Return_True()
            {
                _result.Should().BeTrue();
            }

            [Fact]
            public void Then_It_Should_Return_The_Username_Decoded()
            {
                _username.Should().Be(_expectedUsername);
            }

            [Fact]
            public void Then_It_Should_Return_The_Password_Decoded()
            {
                _password.Should().Be(_expectedPassword);
            }
        }

        public class Given_A_Bearer_Authorization_Header_Getting_Values
            : Given_When_Then_Test
        {
            private string _authHeader;
            private string _username;
            private string _password;
            private bool _result;
            private string _expectedUsername;
            private string _expectedPassword;

            protected override void Given()
            {
                _authHeader = "Bearer foo";

                _expectedUsername = string.Empty;
                _expectedPassword = string.Empty;
            }

            protected override void When()
            {
                _result = AuthorizationUtil.TryGetCredentials(_authHeader, out _username, out _password);
            }

            [Fact]
            public void Then_It_Return_False()
            {
                _result.Should().BeFalse();
            }

            [Fact]
            public void Then_It_Should_Return_The_Username_Decoded()
            {
                _username.Should().Be(_expectedUsername);
            }

            [Fact]
            public void Then_It_Should_Return_The_Password_Decoded()
            {
                _password.Should().Be(_expectedPassword);
            }
        }
    }
}