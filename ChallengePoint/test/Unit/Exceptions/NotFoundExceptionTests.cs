using ChallengePoint.Exceptions;
using Xunit;

namespace ChallengePoint.Tests.Exceptions
{
    public class NotFoundExceptionTests
    {
        [Fact]
        public void Constructor_WithoutParameters_SetsCorrectMessageAndStatusCode()
        {
            var exception = new NotFoundException();

            Assert.Equal("The requested resource was not found.", exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void Constructor_WithMessage_SetsCorrectMessageAndStatusCode()
        {
            var message = "Custom not found message";
            var exception = new NotFoundException(message);

            Assert.Equal(message, exception.Message);
            Assert.Equal(404, exception.StatusCode);
        }

        [Fact]
        public void Constructor_WithMessageAndInnerException_SetsCorrectMessageAndStatusCode()
        {
            var message = "Custom not found message";
            var innerException = new Exception("Inner exception message");
            var exception = new NotFoundException(message, innerException);

            Assert.Equal(message, exception.Message);
            Assert.Equal(404, exception.StatusCode);
            Assert.Equal(innerException, exception.InnerException);
        }
    }
}