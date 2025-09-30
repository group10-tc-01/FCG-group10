using FCG.Application.DependencyInjection;
using FCG.Domain.Entities;
using FCG.Domain.Enum;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var services = new ServiceCollection();

services.AddLogging(cfg => cfg.AddConsole());

services.AddApplication();

var provider = services.BuildServiceProvider();
var mediator = provider.GetRequiredService<IMediator>();

var userTest = User.Create("Lohhan", "lohhan@gmail.com", "Lj@@12131234", Role.User);

foreach (var domainEvent in userTest.DomainEvents)
{
    await mediator.Publish(domainEvent);
}

userTest.ClearDomainEvents();