using MediatR;

namespace DA.GtSWB.Application.Common.Commands;

internal interface ICreateFileCommand : IRequest<Result<DirectoryInfo>> { }
