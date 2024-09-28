using DA.GtSWB.Common.Types.IDs;
using MediatR;

namespace DA.GtSWB.Application.Common.Commands;

public interface ICreateEntityCommand<TKey> : IRequest<Result<TKey>>
    //where TKey : IId
{ }
