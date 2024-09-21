namespace DA.Sepa.DataTypes;

/// <summary>
/// Representatie van een SepaTransactie.
/// </summary>
/// <param name="Id">Identifier van de transactie</param>
/// <param name="DebtorIban">Iban van de debtor, diegene die moet betalen.</param>
/// <param name="DebtorName">Ten naam stelling van de rekening van de debtor.</param>
/// <param name="Amount">Bedrag</param>
/// <param name="RemittanceInformation">Omschrijving van de afschrijving.</param>
/// <param name="DateOfSignature">Datum waarop het mandaad is getekend.</param>
/// <param name="MandateIdentification">Identificatienummer van het mandaad.</param>
/// <param name="EndToEndId"></param>
public sealed record DirectDebitTransaction(
    int Id,
    string DebtorIban,
    string DebtorName,
    decimal Amount,
    string Currency,
    string RemittanceInformation,
    DateOnly DateOfSignature,
    string MandateIdentification,
    string EndToEndId);
