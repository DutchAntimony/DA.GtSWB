﻿using DA.GtSWB.Common.Types.IDs;
using MediatR;

namespace DA.GtSWB.Application.Common.Commands;

public interface ICreateEntityCommandHandler<TCreateCommand, TKey>
    : IRequestHandler<TCreateCommand, Result<TKey>>
    where TCreateCommand : ICreateEntityCommand<TKey>
    //where TKey : IId
{ }
