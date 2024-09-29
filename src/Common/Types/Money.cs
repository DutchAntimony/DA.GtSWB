namespace DA.GtSWB.Common.Types;

public readonly record struct Money : IComparable<Money>
{
    public static Money Zero => new(0);
    public decimal Amount { get; }

    public Money() : this(0) { }
    public Money(decimal amount) =>
        Amount = Math.Abs(Math.Round(amount, 2));

    public bool IsZero => Amount == 0;

    public Money Add(Money other) =>
        new(Amount + other.Amount);

    public Money Subtract(Money other) =>
        other.IsZero ? this
        : Amount == other.Amount ? Zero
        : Amount > other.Amount ? new Money(Amount - other.Amount)
        : throw new InvalidOperationException("Cannot get to a negative amount of money.");

    public Money Scale(int factor) =>
        factor < 0 ? throw new InvalidOperationException("Cannot multiply money with a negative amount.")
        : new Money(Amount * factor);

    public int CompareTo(Money other) =>
          IsZero && other.IsZero ? 0
        : IsZero ? -1
        : other.IsZero ? 1
        : Amount.CompareTo(other.Amount);

    public static Money operator +(Money left, Money right) => left.Add(right);
    public static Money operator -(Money left, Money right) => left.Subtract(right);
    public static Money operator *(Money left, int right) => left.Scale(right);
    public static Money operator *(int left, Money right) => right.Scale(left);

    public override string ToString() => $"{Amount:0.00}";
}
