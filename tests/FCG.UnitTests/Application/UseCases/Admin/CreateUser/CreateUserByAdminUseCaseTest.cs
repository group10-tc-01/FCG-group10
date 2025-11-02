using FCG.Application.UseCases.Admin.CreateUser;
using FCG.CommomTestsUtilities.Builders.Entities;
using FCG.CommomTestsUtilities.Builders.Inputs.Admin.CreateUser;
using FCG.CommomTestsUtilities.Builders.Repositories.UserRepository;
using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using FCG.Domain.Exceptions;
using FCG.Domain.Repositories;
using FCG.Domain.Repositories.LibraryRepository;
using FCG.Domain.Repositories.UserRepository;
using FCG.Domain.Repositories.WalletRepository;
using FCG.Domain.Services;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Moq;
using UoWBuilder = FCG.CommomTestsUtilities.Builders.Repositories.UnitOfWorkBuilder;

namespace FCG.UnitTests.Application.UseCases.Admin.CreateUser
{
    public class CreateUserByAdminUseCaseTest
    {
        private static CreateUserByAdminUseCase BuildUseCase(
            out IReadOnlyUserRepository readOnlyUserRepository,
            out IWriteOnlyUserRepository writeOnlyUserRepository,
            out IWriteOnlyWalletRepository writeOnlyWalletRepository,
            out IWriteOnlyLibraryRepository writeOnlyLibraryRepository,
            out IUnitOfWork unitOfWork,
            out IPasswordEncrypter passwordEncrypter,
            User? existingUser = null)
        {
            if (existingUser != null)
            {
                ReadOnlyUserRepositoryBuilder.SetupGetByEmailAsync(existingUser);
            }
            else
            {
                ReadOnlyUserRepositoryBuilder.SetupGetByEmailAsync(null);
            }

            readOnlyUserRepository = ReadOnlyUserRepositoryBuilder.Build();
            writeOnlyUserRepository = WriteOnlyUserRepositoryBuilder.Build();

            var writeOnlyWalletRepoMock = new Mock<IWriteOnlyWalletRepository>();
            writeOnlyWalletRepository = writeOnlyWalletRepoMock.Object;

            var writeOnlyLibraryRepoMock = new Mock<IWriteOnlyLibraryRepository>();
            writeOnlyLibraryRepository = writeOnlyLibraryRepoMock.Object;

            UoWBuilder.SetupBeginTransactionAsync();
            UoWBuilder.SetupCommitAsync();
            UoWBuilder.SetupRollbackAsync();
            unitOfWork = UoWBuilder.Build();

            PasswordEncrypterServiceBuilder.SetupEncrypt("hashedPassword123");
            passwordEncrypter = PasswordEncrypterServiceBuilder.Build();

            var logger = new Mock<ILogger<CreateUserByAdminUseCase>>().Object;

            CorrelationIdProviderBuilder.SetupGetCorrelationId("test-correlation-id");
            var correlationIdProvider = CorrelationIdProviderBuilder.Build();

            return new CreateUserByAdminUseCase(
                readOnlyUserRepository,
                writeOnlyUserRepository,
                writeOnlyWalletRepository,
                writeOnlyLibraryRepository,
                unitOfWork,
                passwordEncrypter,
                logger,
                correlationIdProvider
            );
        }

        [Fact]
        public async Task Given_ValidRequestWithUserRole_When_CreatingUser_Then_ShouldCreateUserSuccessfully()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.Build();
            var useCase = BuildUseCase(
                out var readOnlyRepo,
                out var writeOnlyRepo,
                out var walletRepo,
                out var libraryRepo,
                out var unitOfWork,
                out var passwordEncrypter
            );

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Email.Should().Be(request.Email);
            result.Role.Should().Be(Role.User);
            result.Id.Should().NotBeEmpty();

            Mock.Get(writeOnlyRepo).Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            Mock.Get(walletRepo).Verify(x => x.AddAsync(It.IsAny<Wallet>()), Times.Once);
            Mock.Get(libraryRepo).Verify(x => x.AddAsync(It.IsAny<Library>()), Times.Once);
            Mock.Get(unitOfWork).Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Given_ValidRequestWithAdminRole_When_CreatingUser_Then_ShouldCreateAdminSuccessfully()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.BuildWithAdminRole();
            var useCase = BuildUseCase(
                out var readOnlyRepo,
                out var writeOnlyRepo,
                out var walletRepo,
                out var libraryRepo,
                out var unitOfWork,
                out var passwordEncrypter
            );

            // Act
            var result = await useCase.Handle(request, CancellationToken.None);

            // Assert
            result.Should().NotBeNull();
            result.Name.Should().Be(request.Name);
            result.Email.Should().Be(request.Email);
            result.Role.Should().Be(Role.Admin);
            result.Id.Should().NotBeEmpty();

            Mock.Get(writeOnlyRepo).Verify(x => x.AddAsync(It.IsAny<User>()), Times.Once);
            Mock.Get(walletRepo).Verify(x => x.AddAsync(It.IsAny<Wallet>()), Times.Once);
            Mock.Get(libraryRepo).Verify(x => x.AddAsync(It.IsAny<Library>()), Times.Once);
            Mock.Get(unitOfWork).Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Given_DuplicateEmail_When_CreatingUser_Then_ShouldThrowDuplicateEmailException()
        {
            // Arrange
            var existingUser = UserBuilder.BuildRegularUser();
            var request = CreateUserByAdminInputBuilder.Build();
            var useCase = BuildUseCase(
                out _,
                out var writeOnlyRepo,
                out var walletRepo,
                out var libraryRepo,
                out var unitOfWork,
                out _,
                existingUser
            );

            // Act
            Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<DuplicateEmailException>();

            Mock.Get(writeOnlyRepo).Verify(x => x.AddAsync(It.IsAny<User>()), Times.Never);
            Mock.Get(walletRepo).Verify(x => x.AddAsync(It.IsAny<Wallet>()), Times.Never);
            Mock.Get(libraryRepo).Verify(x => x.AddAsync(It.IsAny<Library>()), Times.Never);
            Mock.Get(unitOfWork).Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
            Mock.Get(unitOfWork).Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task Given_ValidRequest_When_CreatingUser_Then_ShouldEncryptPassword()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.Build();
            var useCase = BuildUseCase(
                out _,
                out _,
                out _,
                out _,
                out _,
                out var passwordEncrypter
            );

            // Act
            await useCase.Handle(request, CancellationToken.None);

            // Assert
            Mock.Get(passwordEncrypter).Verify(
                x => x.Encrypt(request.Password),
                Times.Once
            );
        }

        [Fact]
        public async Task Given_ValidRequest_When_CreatingUser_Then_ShouldCreateWalletAndLibrary()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.Build();
            var useCase = BuildUseCase(
                out _,
                out _,
                out var walletRepo,
                out var libraryRepo,
                out _,
                out _
            );

            // Act
            await useCase.Handle(request, CancellationToken.None);

            // Assert
            Mock.Get(walletRepo).Verify(
                x => x.AddAsync(It.Is<Wallet>(w => w.UserId != Guid.Empty)),
                Times.Once
            );
            Mock.Get(libraryRepo).Verify(
                x => x.AddAsync(It.Is<Library>(l => l.UserId != Guid.Empty)),
                Times.Once
            );
        }

        [Fact]
        public async Task Given_ExceptionDuringPersistence_When_CreatingUser_Then_ShouldRollbackTransaction()
        {
            // Arrange
            var request = CreateUserByAdminInputBuilder.Build();
            var useCase = BuildUseCase(
                out _,
                out var writeOnlyRepo,
                out _,
                out _,
                out var unitOfWork,
                out _
            );

            Mock.Get(writeOnlyRepo)
                .Setup(x => x.AddAsync(It.IsAny<User>()))
                .ThrowsAsync(new Exception("Database error"));

            // Act
            Func<Task> act = async () => await useCase.Handle(request, CancellationToken.None);

            // Assert
            await act.Should().ThrowAsync<Exception>().WithMessage("Database error");
            Mock.Get(unitOfWork).Verify(x => x.RollbackAsync(It.IsAny<CancellationToken>()), Times.Once);
            Mock.Get(unitOfWork).Verify(x => x.CommitAsync(It.IsAny<CancellationToken>()), Times.Never);
        }
    }
}
