namespace Aurora.Core.Financial;

public interface IBicProvider
{
    public bool TryGetValue(string iban, out string? bic);
}

public class BicProvider : IBicProvider
{
    public bool TryGetValue(string iban, out string? bic)
    {
        bic = null;
        if (string.IsNullOrWhiteSpace(iban))
            return false;

        if (iban is null || iban.Length < 8)
            return false;

        string bankIdentifier = iban.Substring(4, 4);

        return _codes.TryGetValue(bankIdentifier, out bic);
    }

    private static readonly Dictionary<string, string> _codes = new() {
            {"ABNA", "ABNANL2A"},
            {"FTSB", "ABNANL2A"},
            {"ABNC", "ABNCNL2A"},
            {"ADYB", "ADYBNL2A"},
            {"AEGO", "AEGONL2U"},
            {"ANDL", "ANDLNL2A"},
            {"ARBN", "ARBNNL22"},
            {"ARSN", "ARSNNL21"},
            {"ASNB", "ASNBNL21"},
            {"ATBA", "ATBANL2A"},
            {"BARC", "BARCNL22"},
            {"BCDM", "BCDMNL22"},
            {"BCIT", "BCITNL2A"},
            {"BICK", "BICKNL2A"},
            {"BINK", "BINKNL21"},
            {"BITS", "BITSNL2A"},
            {"BKCH", "BKCHNL2R"},
            {"BKMG", "BKMGNL2A"},
            {"BLGW", "BLGWNL21"},
            {"BMEU", "BMEUNL21"},
            {"BNDA", "BNDANL2A"},
            {"BNGH", "BNGHNL2G"},
            {"BNPA", "BNPANL2A"},
            {"BOFA", "BOFANLNX"},
            {"BOFS", "BOFSNL21002"},
            {"BOTK", "BOTKNL2X"},
            {"BUNQ", "BUNQNL2A"},
            {"CHAS", "CHASNL2X"},
            {"CITC", "CITCNL2A"},
            {"CITI", "CITINL2X"},
            {"COBA", "COBANL2X"},
            {"DELE", "DELENL22"},
            {"DEUT", "DEUTNL2A"},
            {"DHBN", "DHBNNL2R"},
            {"DLBK", "DLBKNL2A"},
            {"DNIB", "DNIBNL2G"},
            {"EBPB", "EBPBNL22"},
            {"EBUR", "EBURNL21"},
            {"FBHL", "FBHLNL2A"},
            {"FLOR", "FLORNL2A"},
            {"FRNX", "FRNXNL2A"},
            {"FVLB", "FVLBNL22"},
            {"FXBB", "FXBBNL22"},
            {"GILL", "GILLNL2A"},
            {"HAND", "HANDNL2A"},
            {"HHBA", "HHBANL22"},
            {"HSBC", "HSBCNL2A"},
            {"ICBC", "ICBCNL2A"},
            {"ICBK", "ICBKNL2A"},
            {"ICEP", "ICEPNL21"},
            {"INGB", "INGBNL2A"},
            {"ISAE", "ISAENL2A"},
            {"ISBK", "ISBKNL2A"},
            {"KABA", "KABANL2A"},
            {"KASA", "KASANL2A"},
            {"KNAB", "KNABNL2H"},
            {"KOEX", "KOEXNL2A"},
            {"KRED", "KREDNL2X"},
            {"LOCY", "LOCYNL2A"},
            {"LOYD", "LOYDNL2A"},
            {"LPLN", "LPLNNL2F"},
            {"MHCB", "MHCBNL2A"},
            {"MOYO", "MOYONL21"},
            {"NNBA", "NNBANL2G"},
            {"NWAB", "NWABNL2G"},
            {"PCBC", "PCBCNL2A"},
            {"RABO", "RABONL2U"},
            {"RBRB", "RBRBNL21"},
            {"SNSB", "SNSBNL2A"},
            {"SOGE", "SOGENL2A"},
            {"TRIO", "TRIONL2U"},
            {"UGBI", "UGBINL2A"},
            {"VOWA", "VOWANL21"},
            {"VPAY", "VPAYNL22"},
            {"ZWLB", "ZWLBNL21"}
        };
}
