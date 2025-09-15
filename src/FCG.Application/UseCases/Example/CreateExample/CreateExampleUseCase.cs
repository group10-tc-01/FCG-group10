namespace FCG.Application.UseCases.Example.CreateExample
{
    public class CreateExampleUseCase : ICreateExampleUseCase
    {
        public async Task<CreateExampleOutput> Handle(CreateExampleInput request, CancellationToken cancellationToken)
        {
            await Task.FromResult(0);

            var output = MapToOutput(request);

            return output;
        }

        private static CreateExampleOutput MapToOutput(CreateExampleInput request)
        {
            return new CreateExampleOutput
            {
                Example = new CreateExampleDto
                {
                    Name = request.Name,
                    Description = request.Description
                }
            };
        }
    }
}
