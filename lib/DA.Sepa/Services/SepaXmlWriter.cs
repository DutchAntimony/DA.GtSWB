using System.Text;
using System.Xml;
using DA.Sepa.DataTypes;
using DA.Sepa.Extensions;

namespace DA.Sepa.Services;

internal sealed class SepaXmlWriter : ISepaXmlWriter
{
    private const string _pmtMtd = "DD";
    private const string _btchBookg = "true";
    private const string _svcLvl = "SEPA";
    private const string _prtry = "SEPA";
    private const string _lclInstrm = "CORE";
    private const string _seqTp = "FRST";
    private const string _chrgBr = "SLEV";

    public async Task WriteAsync(string file, SepaPayment payment)
    {
        var xml = await Generate(payment);
        xml.Save(file);
    }

    private static async Task<XmlDocument> Generate(SepaPayment payment)
    {
        var xml = new XmlDocument();
        await Task.Run(() =>
        {

            _ = xml.AppendChild(xml.CreateXmlDeclaration("1.0", Encoding.UTF8.BodyName, "yes"));
            var document = (XmlElement)xml.AppendChild(xml.CreateElement("Document"))!;
            document.SetAttribute("xmlns:xsi", "http://www.w3.org/2001/XMLSchema-instance");
            document.SetAttribute("xmlns", $"urn:iso:std:iso:20022:tech:xsd:{SepaPayment.Schema}");
            var cstmrDrctDbtInitn = document.NewElement("CstmrDrctDbtInitn");

            AddGroupHeader(cstmrDrctDbtInitn, payment);
            AddPaymentInfo(cstmrDrctDbtInitn, payment);
        });

        return xml;
    }

    private static void AddGroupHeader(XmlElement root, SepaPayment payment)
    {
        var grpHdr = root.NewElement("GrpHdr");
        _ = grpHdr.NewElement("MsgId", payment.MessageId);
        _ = grpHdr.NewElement("CreDtTm", payment.CreationDate.FormatDateTime());
        _ = grpHdr.NewElement("NbOfTxs", payment.ControlCount);
        _ = grpHdr.NewElement("CtrlSum", payment.ControlAmount.FormatAmount());
        _ = grpHdr.NewElement("InitgPty").NewElement("Nm", payment.CreditorName);
    }

    private static void AddPaymentInfo(XmlElement root, SepaPayment payment)
    {
        var pmtInf = root.NewElement("PmtInf");
        _ = pmtInf.NewElement("PmtInfId", payment.MessageId);
        _ = pmtInf.NewElement("PmtMtd", _pmtMtd);
        _ = pmtInf.NewElement("BtchBookg", _btchBookg);
        _ = pmtInf.NewElement("NbOfTxs", payment.ControlCount);
        _ = pmtInf.NewElement("CtrlSum", payment.ControlAmount.FormatAmount());

        var pmtTpInf = pmtInf.NewElement("PmtTpInf");
        _ = pmtTpInf.NewElement("SvcLvl").NewElement("Cd", _svcLvl);
        _ = pmtTpInf.NewElement("LclInstrm").NewElement("Cd", _lclInstrm);
        _ = pmtTpInf.NewElement("SeqTp", _seqTp);

        _ = pmtInf.NewElement("ReqdColltnDt", payment.CollectionDate.FormatDate());
        _ = pmtInf.NewElement("Cdtr").NewElement("Nm", payment.CreditorName);

        var cdtrAcct = pmtInf.NewElement("CdtrAcct");
        _ = cdtrAcct.NewElement("Id").NewElement("IBAN", payment.CreditorIban);
        _ = cdtrAcct.NewElement("Ccy", SepaPayment.CreditorAccountCurrency);

        _ = pmtInf.NewElement("CdtrAgt").NewElement("FinInstnId").NewElement("BIC", payment.CreditorBic);
        _ = pmtInf.NewElement("ChrgBr", _chrgBr);

        var othr = pmtInf.NewElement("CdtrSchmeId").NewElement("Id")
        .NewElement("PrvtId")
            .NewElement("Othr");
        _ = othr.NewElement("Id", payment.PayeeId);
        _ = othr.NewElement("SchmeNm").NewElement("Prtry", _prtry);

        foreach (var transaction in payment.Transactions)
        {
            AddTransaction(pmtInf, transaction);
        }
    }

    private static void AddTransaction(XmlElement root, DirectDebitTransaction transaction)
    {
        var drctDbtTxInf = root.NewElement("DrctDbtTxInf");
        var pmtId = drctDbtTxInf.NewElement("PmtId");
        _ = pmtId.NewElement("EndToEndId", transaction.EndToEndId);
        drctDbtTxInf.NewElement("InstdAmt", transaction.Amount.FormatAmount()).SetAttribute("Ccy", transaction.Currency);

        var mndtRltdInf = drctDbtTxInf.NewElement("DrctDbtTx").NewElement("MndtRltdInf");
        _ = mndtRltdInf.NewElement("MndtId", transaction.MandateIdentification);
        _ = mndtRltdInf.NewElement("DtOfSgntr", transaction.DateOfSignature.FormatDate());

        _ = drctDbtTxInf.NewElement("DbtrAgt").NewElement("FinInstnId"); //, transaction.Debtor.Bic);
        _ = drctDbtTxInf.NewElement("Dbtr").NewElement("Nm", transaction.DebtorName);
        _ = drctDbtTxInf.NewElement("DbtrAcct").NewElement("Id").NewElement("IBAN", transaction.DebtorIban);

        _ = drctDbtTxInf.NewElement("RmtInf").NewElement("Ustrd", transaction.RemittanceInformation);
    }
}
