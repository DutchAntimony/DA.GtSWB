namespace DA.GtSWB.Common.Types.Enums;

public enum BetaalStatus
{
    Open,                   // Betaalopdracht die verstuurd is maar nog niet betaald is.
    Teruggevorderd,         // Incasso die is teruggevorderd.
    Kwijtgescholden,        // Betaalopdracht die niet voldaan hoeft te worden.
    Vervallen,              // Betaalopdracht waar een andere voor in de plaats is gekomen.
    Voldaan                 // Betaalopdracht die voldaan is.
}