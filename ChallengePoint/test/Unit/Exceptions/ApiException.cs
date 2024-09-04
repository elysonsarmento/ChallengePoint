using ChallengePoint.Exceptions;
using Xunit;

namespace ChallengePoint.Tests.Exceptions
{
    public class ApiExceptionTests
    {
        [Fact]
        public void ApiException_DefaultConstructor_ShouldSetDefaultMessageAndStatusCode()
        {
            // Act
            var exception = new ApiException();

            // Assert
            Assert.Equal("An error occurred in the application.", exception.Message);
            Assert.Equal(500, exception.StatusCode);
        }

        [Fact]
        public void ApiException_MessageConstructor_ShouldSetMessageAndDefaultStatusCode()
        {
            // Arrange
            var message = "Custom error message";

            // Act
            var exception = new ApiException(message);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(500, exception.StatusCode);
        }

        [Fact]
        public void ApiException_MessageAndStatusCodeConstructor_ShouldSetMessageAndStatusCode()
        {
            // Arrange
            var message = "Custom error message";
            var statusCode = 404;

            // Act
            var exception = new ApiException(message, statusCode);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
        }

        [Fact]
        public void ApiException_MessageAndInnerExceptionConstructor_ShouldSetMessageInnerExceptionAndDefaultStatusCode()
        {
            // Arrange
            var message = "Custom error message";
            var innerException = new Exception("Inner exception message");

            // Act
            var exception = new ApiException(message, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(innerException, exception.InnerException);
            Assert.Equal(500, exception.StatusCode);
        }

        [Fact]
        public void ApiException_MessageStatusCodeAndInnerExceptionConstructor_ShouldSetMessageStatusCodeAndInnerException()
        {
            // Arrange
            var message = "Custom error message";
            var statusCode = 400;
            var innerException = new Exception("Inner exception message");

            // Act
            var exception = new ApiException(message, statusCode, innerException);

            // Assert
            Assert.Equal(message, exception.Message);
            Assert.Equal(statusCode, exception.StatusCode);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}
