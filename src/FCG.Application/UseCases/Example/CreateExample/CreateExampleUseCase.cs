using FCG.Domain.Repositories;
using FCG.Domain.Repositories.ExampleRepository;
using ExampleEntity = FCG.Domain.Entities.Example;

namespace FCG.Application.UseCases.Example.CreateExample
{
    public class CreateExampleUseCase : ICreateExampleUseCase
    {
        private readonly IWriteOnlyExampleRepository _writeOnlyExampleRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateExampleUseCase(IWriteOnlyExampleRepository writeOnlyExampleRepository, IUnitOfWork unitOfWork)
        {
            _writeOnlyExampleRepository = writeOnlyExampleRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<CreateExampleOutput> Handle(CreateExampleInput request, CancellationToken cancellationToken)
        {
            var example = ExampleEntity.Create(request.Name, request.Description);

            await SaveAsync(example, cancellationToken);

            var output = MapToOutput(example);

            return output;
        }

        private async Task SaveAsync(ExampleEntity example, CancellationToken cancellationToken)
        {
            await _writeOnlyExampleRepository.AddAsync(example);

            await _unitOfWork.SaveChangesAsync(cancellationToken);
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
