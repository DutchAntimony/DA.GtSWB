using DA.GtSWB.Common.Data.IDs;
using MediatR;

namespace DA.GtSWB.Application.Common.Commands;

public interface ICreateEntityCommand<TKey> : IRequest<Result<TKey>>
    //where TKey : IId
{ }
