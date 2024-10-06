using DA.GtSWB.Domain.Models.Contributie;

namespace DA.GtSWB.Application.Contributie.Services;
internal interface INotaTekstProvider
{
    NotaTekst GetVoorContributieNota(DateOnly verzenddatum);
}