﻿using MediatR;

namespace FCG.Application.UseCases.Games.Register
{
    public class RegisterGameInput : IRequest<RegisterGameOutput>
    {
        public string Name { get; init; } = string.Empty;
        public string Description { get; init; } = string.Empty;
        public decimal Price { get; init; }
        public string Category { get; init; } = string.Empty;
    }
}
