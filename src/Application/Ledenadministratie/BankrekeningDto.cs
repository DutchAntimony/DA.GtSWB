using DA.GtSWB.Common.Types;
using DA.GtSWB.Common.Types.Enums;
using DA.GtSWB.Domain.Models.Ledenadministratie;

namespace DA.GtSWB.Application.Ledenadministratie;

public record BankrekeningDto
{
    private Iban? _convertedIban;

    private string? _iban;
    public string? IbanInput
    {
        get => _iban;
        set => _iban = value?.Replace(" ", "").Sanitize();
    }

    private string? _bic;
    public string? Bic
    {
        get => _bic;
        set => _bic = value?.Replace(" ", "").Sanitize();
    }

    private string? _tenNameVan;
    public string? TenNameVan
    {
        get => _tenNameVan;
        set => _tenNameVan = value?.Sanitize();
    }

    public MandaadType MandaadType { get; set; } = MandaadType.Geen;
    public DateOnly? MandaadDatum { get; set; }

    public Result Validate(IBicProvider bicProvider)
    {
        return new ValidationFailureCollection()
            .AddFrom(ParseIbanAndBic(bicProvider))
            .AddFrom(TenNameVan.VerifyNotEmpty().Bind(TenNameVan.VerifyMaxLength(127)))
            .ToResult();
    }

    public Result<Bankrekening> ToDomainModel(IBicProvider bicProvider)
    {
        return Validate(bicProvider)
            .MapFunc(() => Bankrekening.Create(_convertedIban!.Value, Bic!, TenNameVan!, MandaadDatum, MandaadType));
    }

    private Result<string> ParseIbanAndBic(IBicProvider bicProvider)
    {
        return _iban.VerifyNotNull("Iban")
            .Bind(Iban.TryCreate)
            .Tap(valid => _convertedIban = valid)
            .Bind(valid => bicProvider.TryFindBic(valid, _bic))
            .Tap(bic => _bic = bic);
    }
}

