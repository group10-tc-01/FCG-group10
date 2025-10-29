using FCG.Domain.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FCG.Application.UseCases.Users.MyGames
{
    public class LibraryGameResponse
    {
        public Guid GameId { get; set; }
        public string GameName { get; set; }
        public DateTime PurchaseDate { get; set; }
        public decimal PurchasePrice { get; set; }
        public GameStatus Status { get; set; }
    }
}
