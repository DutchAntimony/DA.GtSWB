namespace DA.Sepa.DataTypes;

/// <summary>
/// Definition of a Sepa transfer payment information.
/// </summary>
/// <param name="MessageId">Unique identifier of the opdracht. Sequential, currently equal to YY + 22.</param>
/// <param name="CreditorName">Creditor name for the crediting agency.</param>
/// <param name="CreditorIban">Iban of the crediting agency.</param>
/// <param name="CreditorBic">Bic of the crediting agency.</param>
/// <param name="PayeeId">Incasso machtiging of the crediting agency.</param>
/// <param name="CreationDate">The creation date of this payment.</param>
/// <param name="CollectionDate">The collection date, so the execution date of this payment.</param>
public sealed record SepaPayment(
    int MessageId,
    string CreditorName,
    string CreditorIban,
    string CreditorBic,
    string PayeeId,
    DateTime CreationDate,
    DateOnly CollectionDate)
{
    public const string Schema = "pain.008.001.02";
    public const string CreditorAccountCurrency = "EUR";

    private readonly List<DirectDebitTransaction> _transactions = [];

    public int ControlCount { get; private set; } = 0;
    public decimal ControlAmount { get; private set; } = 0;

    public IReadOnlyCollection<DirectDebitTransaction> Transactions => _transactions.ToList();

    public void AddDirectDebitTransaction(DirectDebitTransaction transaction)
    {
        _transactions.Add(transaction);
        ControlCount++;
        ControlAmount += transaction.Amount;
    }
}
