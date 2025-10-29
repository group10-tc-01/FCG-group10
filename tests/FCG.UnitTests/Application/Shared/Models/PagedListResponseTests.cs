using FCG.Domain.Models.Pagination;
using FluentAssertions;

namespace FCG.UnitTests.Application.Shared.Models
{
    public class PagedListResponseTests
    {
        [Fact(DisplayName = "Construtor deve definir propriedades corretamente")]
        public void Constructor_GivenValidParameters_ShouldSetPropertiesCorrectly()
        {
            // Arrange
            var items = new List<string> { "item1", "item2", "item3" };
            var totalCount = 10;
            var currentPage = 2;
            var pageSize = 5;

            // Act
            var pagedList = new PagedListResponse<string>(items, totalCount, currentPage, pageSize);

            // Assert
            pagedList.TotalCount.Should().Be(totalCount);
            pagedList.CurrentPage.Should().Be(currentPage);
            pagedList.PageSize.Should().Be(pageSize);
            pagedList.TotalPages.Should().Be(2);
            pagedList.Items.Should().BeEquivalentTo(items);
        }

        [Fact(DisplayName = "HasPrevious deve ser false quando pagina atual for a primeira")]
        public void HasPrevious_GivenFirstPage_ShouldBeFalse()
        {
            // Arrange & Act
            var pagedList = new PagedListResponse<string>(new List<string>(), 10, 1, 5);

            // Assert
            pagedList.HasPrevious.Should().BeFalse();
        }

        [Fact(DisplayName = "HasPrevious deve ser true quando pagina atual for maior que um")]
        public void HasPrevious_GivenPageGreaterThanOne_ShouldBeTrue()
        {
            // Arrange & Act
            var pagedList = new PagedListResponse<string>(new List<string>(), 10, 2, 5);

            // Assert
            pagedList.HasPrevious.Should().BeTrue();
        }

        [Fact(DisplayName = "HasNext deve ser false quando pagina atual for a ultima")]
        public void HasNext_GivenLastPage_ShouldBeFalse()
        {
            // Arrange & Act
            var pagedList = new PagedListResponse<string>(new List<string>(), 10, 2, 5);

            // Assert
            pagedList.HasNext.Should().BeFalse();
        }

        [Fact(DisplayName = "HasNext deve ser true quando pagina atual nao for a ultima")]
        public void HasNext_GivenNotLastPage_ShouldBeTrue()
        {
            // Arrange & Act
            var pagedList = new PagedListResponse<string>(new List<string>(), 15, 2, 5);

            // Assert
            pagedList.HasNext.Should().BeTrue();
        }

        [Fact(DisplayName = "Deve calcular TotalPages corretamente para casos de divsao exata")]
        public void Constructor_GivenExactDivision_ShouldCalculateTotalPagesCorrectly()
        {
            // Arrange & Act
            var pagedList = new PagedListResponse<string>(new List<string>(), 20, 1, 5);

            // Assert
            pagedList.TotalPages.Should().Be(4);
        }
    }
}