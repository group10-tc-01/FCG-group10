using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace FCG.Application.UseCases.Users.MyGames
{
    public class LibraryGameUseCaseRequest : IRequest<ICollection<LibraryGameResponse>>
    {
    }
}
