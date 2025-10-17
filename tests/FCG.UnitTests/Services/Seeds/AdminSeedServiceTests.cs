using FCG.CommomTestsUtilities.Builders.Services;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using Moq;

namespace FCG.UnitTests.Services.Seeds
{
    public class AdminSeedServiceTests
    {
        [Fact(DisplayName = "SeedAsync deve criar admin quando não existir nenhum")]
        public async Task SeedAsync_ShouldCreateAdmin_WhenNoAdminExists()
        {
            var builder = new AdminSeedServiceBuilder().WithNoAdminExisting();
            var service = builder.Build();

            await service.SeedAsync();

            builder.WriteOnlyRepoMock.Verify(
                x => x.AddAsync(It.Is<User>(u =>
                    u.Email.Value == "admin@mail.com" &&

                    u.Role == Role.Admin
                ),
                It.IsAny<Wallet>()
                ),
                Times.Once
            );

            builder.UnitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Once);
        }

        [Fact(DisplayName = "SeedAsync não deve criar admin se já existir um")]
        public async Task SeedAsync_ShouldNotCreateAdmin_WhenAdminAlreadyExists()
        {
            var builder = new AdminSeedServiceBuilder().WithAdminAlreadyExisting();
            var service = builder.Build();

            await service.SeedAsync();

            builder.WriteOnlyRepoMock.Verify(x => x.AddAsync(It.IsAny<User>(), It.IsAny<Wallet>()), Times.Never);
            builder.UnitOfWorkMock.Verify(x => x.SaveChangesAsync(default), Times.Never);
        }
    }
}
