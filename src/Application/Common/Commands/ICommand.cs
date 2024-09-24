using MediatR;

namespace DA.GtSWB.Application.Common.Commands;

internal interface ICommand : IRequest<Result> { }
