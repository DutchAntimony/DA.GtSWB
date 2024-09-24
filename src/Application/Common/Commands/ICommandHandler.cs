using MediatR;

namespace DA.GtSWB.Application.Common.Commands;

internal interface ICommandHandler<TCommand>
    : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{ }
