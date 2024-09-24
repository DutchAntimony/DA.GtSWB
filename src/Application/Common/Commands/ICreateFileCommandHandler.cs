using MediatR;

namespace DA.GtSWB.Application.Common.Commands;

internal interface ICreateFileCommandHandler<TCreateFileCommand>
    : IRequestHandler<TCreateFileCommand, Result<DirectoryInfo>>
    where TCreateFileCommand : ICreateFileCommand
{ }