using Xunit;
using ChallengePoint.Utils;

namespace ChallengePoint.Tests.Utils
{
    public class HourUtilTests
    {
        [Theory]
        [InlineData("2024-09-01T12:34:56Z", 2024, 9, 1, 12, 34, 56)]
        [InlineData("2024-09-01T12:34:56+00:00", 2024, 9, 1, 12, 34, 56)]
        [InlineData("2024-09-01T12:34:56.789Z", 2024, 9, 1, 12, 34, 56, 789)]
        public void ConvertIsoToDateTime_ValidIsoString_ReturnsExpectedDateTime(string isoString, int year, int month, int day, int hour, int minute, int second, int millisecond = 0)
        {
            // Act
            DateTime result = HourUtil.ConvertIsoToDateTime(isoString);

            // para UTC antes da comparação
            DateTime expectedDateTime = new DateTime(year, month, day, hour, minute, second, millisecond, DateTimeKind.Utc);

            // Assert
            Assert.Equal(expectedDateTime.ToUniversalTime(), result.ToUniversalTime());
        }


        [Fact]
        public void ConvertIsoToDateTime_InvalidIsoString_ThrowsFormatException()
        {
            // Arrange
            string invalidIsoString = "invalid-iso-string";

            // Act & Assert
            Assert.Throws<FormatException>(() => HourUtil.ConvertIsoToDateTime(invalidIsoString));
        }
    }
}
