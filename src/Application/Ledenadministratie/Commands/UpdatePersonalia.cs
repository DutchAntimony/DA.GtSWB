﻿using DA.GtSWB.Application.Common;
using DA.GtSWB.Application.Common.Commands;
using DA.GtSWB.Application.Ledenadministratie.Personen;
using DA.GtSWB.Common.Data;
using DA.GtSWB.Common.Types.IDs;
using DA.GtSWB.Domain.Models.Ledenadministratie;
using DA.GtSWB.Domain.ServiceDefinitions;
using DA.GtSWB.Domain.Specifications;

namespace DA.GtSWB.Application.Ledenadministratie.Commands;

public static class UpdatePersonalia
{
    public record Command(LidId LidId, PersonaliaDto Dto, RequestMetadata Metadata) : ICommand;

    internal sealed class Handler(IUnitOfWork unitOfWork, ISpecification<Lid> alleLeden)
        : ICommandHandler<Command>
    {
        public async Task<Result> Handle(Command request, CancellationToken cancellationToken)
        {
            return await
                unitOfWork.Leden.SingleOrFailureAsync(alleLeden.ById(request.LidId), cancellationToken)
                .Check(request.Metadata.Validate())
                .Combine(_ => request.Dto.ToDomainModel(request.Metadata.Timestamp))
                .Tap((lid, personalia) => lid.UpdatePersonalia(personalia, request.Metadata.Timestamp, request.Metadata.Gebruiker));
        }
    }
}
