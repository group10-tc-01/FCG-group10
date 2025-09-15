using FCG.Domain.Repositories.ExampleRepository;
using ExampleEntity = FCG.Domain.Entities.Example;

namespace FCG.Application.UseCases.Example.CreateExample
{
    public class CreateExampleUseCase : ICreateExampleUseCase
    {
        private readonly IWriteOnlyExampleRepository _writeOnlyExampleRepository;
        public CreateExampleUseCase(IWriteOnlyExampleRepository writeOnlyExampleRepository)
        {
            _writeOnlyExampleRepository = writeOnlyExampleRepository;
        }

        public async Task<CreateExampleOutput> Handle(CreateExampleInput request, CancellationToken cancellationToken)
        {
            var example = ExampleEntity.Create(request.Name, request.Description);

            await _writeOnlyExampleRepository.AddAsync(example);

            var output = MapToOutput(example);

            return output;
        }

        private static CreateExampleOutput MapToOutput(ExampleEntity example)
        {
            return new CreateExampleOutput
            {
                Example = new CreateExampleDto
                {
                    Name = example.Name,
                    Description = example.Description
                }
            };
        }
    }
}
