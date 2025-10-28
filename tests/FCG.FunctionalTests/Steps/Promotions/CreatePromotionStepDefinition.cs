using FCG.Application.UseCases.Promotions.Create;
using FCG.FunctionalTests.Fixtures;
using FluentAssertions;
using Reqnroll;

namespace FCG.FunctionalTests.Steps.Promotions
{
    [Binding]
    public class CreatePromotionStepDefinition(FixtureManager fixtureManager)
    {
        private readonly FixtureManager _fixtureManager = fixtureManager;
        private CreatePromotionRequest? _createPromotionRequest;
        private CreatePromotionResponse? _createPromotionResponse;
        private Exception? _exception;

        [Given(@"a criacao de uma nova promocao")]
        public void GivenTheCreationOfANewPromotion()
        {
            _createPromotionRequest = _fixtureManager.CreatePromotion.CreatePromotionRequest;
        }

        [When(@"o admin envia uma requisicao de criacao de promocao com dados validos")]
        public async Task WhenAdminSendsPromotionCreationRequestWithValidData()
        {
            try
            {
                _createPromotionResponse = await _fixtureManager.CreatePromotion.CreatePromotionUseCase.Handle(_createPromotionRequest!, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _exception = ex;
            }
        }

        [Then(@"a promocao deve ser criada com sucesso")]
        public void ThenThePromotionShouldBeCreatedSuccessfully()
        {
            _exception.Should().BeNull();
            _createPromotionResponse.Should().NotBeNull();
            _createPromotionResponse.Id.Should().NotBeEmpty();
            _createPromotionResponse.GameId.Should().Be(_createPromotionRequest!.GameId);
            _createPromotionResponse.Discount.Should().Be(_createPromotionRequest.DiscountPercentage);
            _createPromotionResponse.StartDate.Should().Be(_createPromotionRequest.StartDate);
            _createPromotionResponse.EndDate.Should().Be(_createPromotionRequest.EndDate);
        }
    }
}
