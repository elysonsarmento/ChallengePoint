using Xunit;
using ChallengePoint.Utils;

namespace ChallengePoint.Tests.Utils
{
    public class SimplePaginationTests
    {
        [Fact]
        public void SimplePagination_CreatesCorrectPage()
        {
            // Arrange
            var source = Enumerable.Range(1, 20).ToList(); // Lista de 1 a 20
            int pageNumber = 2;
            int pageQuantity = 5;

            // Act
            var pagination = new SimplePagination<int>(source, pageNumber, pageQuantity);

            // Assert
            var expectedItems = new List<int> { 6, 7, 8, 9, 10 }; // Itens esperados na segunda página
            Assert.Equal(expectedItems, pagination.Items);
            Assert.Equal(pageNumber, pagination.PageNumber);
            Assert.Equal(pageQuantity, pagination.PageQuantity);
        }

        [Fact]
        public void SimplePagination_HandlesEmptySource()
        {
            // Arrange
            var source = new List<int>(); // Lista vazia
            int pageNumber = 1;
            int pageQuantity = 5;

            // Act
            var pagination = new SimplePagination<int>(source, pageNumber, pageQuantity);

            // Assert
            Assert.Empty(pagination.Items);
            Assert.Equal(pageNumber, pagination.PageNumber);
            Assert.Equal(pageQuantity, pagination.PageQuantity);
        }

        [Fact]
        public void SimplePagination_CreatesCorrectPage_WithCreateMethod()
        {
            // Arrange
            var source = Enumerable.Range(1, 10).ToList(); // Lista de 1 a 10
            int pageNumber = 1;
            int pageQuantity = 3;

            // Act
            var pagination = SimplePagination<int>.Create(source, pageNumber, pageQuantity);

            // Assert
            var expectedItems = new List<int> { 1, 2, 3 }; // Itens esperados na primeira página
            Assert.Equal(expectedItems, pagination.Items);
            Assert.Equal(pageNumber, pagination.PageNumber);
            Assert.Equal(pageQuantity, pagination.PageQuantity);
        }
    }
}
