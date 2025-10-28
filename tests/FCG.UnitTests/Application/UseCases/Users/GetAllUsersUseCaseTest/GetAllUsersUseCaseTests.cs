using FCG.Application.UseCases.Admin.GetAllUsers;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Repositories.UserRepository;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;

namespace FCG.UnitTests.Application.UseCases.Users.GetAllUsersUseCaseTest
{
    public class GetAllUsersUseCaseTests
    {
        private readonly Mock<IReadOnlyUserRepository> _userRepositoryMock;
        private readonly Mock<ILogger<GetAllUsersUseCase>> _loggerMock;
        private readonly GetAllUsersUseCase _handler;

        public GetAllUsersUseCaseTests()
        {
            _userRepositoryMock = new Mock<IReadOnlyUserRepository>();
            _loggerMock = new Mock<ILogger<GetAllUsersUseCase>>();
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();
            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");

            _handler = new GetAllUsersUseCase(_userRepositoryMock.Object, _loggerMock.Object, correlationIdProvider);
        }

        [Fact]
        public async Task Given_NoFilters_When_HandlingQuery_Then_ShouldReturnAllUsersPagedList()
        {
            // Arrange
            var users = new List<User>
            {
                User.Create("User A", "usera@test.com", "Password@123", Role.User),
                User.Create("User B", "userb@test.com", "Password@123", Role.Admin),
                User.Create("User C", "userc@test.com", "Password@123", Role.User)
            };

            var query = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };

            var expectedTuple = (
                Items: (IEnumerable<User>)users,
                TotalCount: users.Count
            );

            _userRepositoryMock
                .Setup(repo => repo.GetAllUsersAsync(
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedTuple);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Items.Should().HaveCount(3);
            result.TotalCount.Should().Be(3);
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Given_NoUsersFound_When_HandlingQuery_Then_ShouldReturnEmptyPagedList()
        {
            // Arrange
            var query = new GetAllUserCaseRequest
            {
                PageNumber = 1,
                PageSize = 10
            };
        
            var repositoryResponse = (
                Items: Enumerable.Empty<User>(),
                TotalCount: 0
            );
        
            _userRepositoryMock
                .Setup(repo => repo.GetAllUsersAsync(
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryResponse);
        
            // Act
            var result = await _handler.Handle(query, CancellationToken.None);
        
            // Assert
            result.Should().NotBeNull();
            result.Items.Should().BeEmpty("pois o totalCount é zero");
            result.TotalCount.Should().Be(0, "pois nenhum usuário foi encontrado");
            result.CurrentPage.Should().Be(1);
            result.PageSize.Should().Be(10);
        }

        [Fact]
        public async Task Given_PageNumber2_When_HandlingQuery_Then_ShouldReturnCorrectPage()
        {
            // Arrange
            var totalUsersInDb = new List<User>
            {
                User.Create("User A", "usera@test.com", "Password@123", Role.User),
                User.Create("User B", "userb@test.com", "Password@123", Role.Admin),
                User.Create("User C", "userc@test.com", "Password@123", Role.User)
            };

            var query = new GetAllUserCaseRequest
            {
                PageNumber = 2,
                PageSize = 2
            };

            var usersForPage2 = totalUsersInDb.Skip(2).Take(2).ToList();
            var repositoryResponse = (
                Items: (IEnumerable<User>)usersForPage2,
                TotalCount: totalUsersInDb.Count
            );

            _userRepositoryMock
                .Setup(repo => repo.GetAllUsersAsync(
                    query.PageNumber,
                    query.PageSize,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(repositoryResponse);

            // Act
            var result = await _handler.Handle(query, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.CurrentPage.Should().Be(2, "porque a página 2 foi solicitada");
            result.TotalCount.Should().Be(3, "o total de itens não muda com a paginação");
            result.Items.Should().HaveCount(1, "pois apenas um usuário sobrou para a segunda página");
            result.Items.First().Name.Should().Be("User C");
            result.TotalPages.Should().Be(2, "pois 3 itens divididos em páginas de 2 resultam em 2 páginas");
        }
    }
}
