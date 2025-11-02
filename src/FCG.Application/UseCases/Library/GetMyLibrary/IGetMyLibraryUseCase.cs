using FCG.Application.UseCases.Library.GetMyLibrary;
using MediatR;
using Microsoft.Extensions.Logging;

namespace FCG.Application.UseCases.Library.GetMyLibrary
{
    public interface IGetMyLibraryUseCase : IRequestHandler<GetMyLibraryRequest, GetMyLibraryResponse>
    {
    }
}
