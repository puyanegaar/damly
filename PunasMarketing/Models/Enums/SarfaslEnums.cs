// ReSharper disable IdentifierTypo

using System.ComponentModel;

namespace PunasMarketing.Models.Enums
{
    public enum SarfaslEnums
    {
        DaraeiJari = 9,
        DaraeiSabet = 10,
        CashDesk = 11,
        Bank = 13,
        HesabDaryaftani = 14,
        AsnadDaryftani = 15,
        ChequeHayeDarJaryaneVosool = 16,
        MojoodiKala = 18,
        MaliatBarArzeshAfzodehKharid = 21,
        HesabPardakhtani = 29,
        AsnadPardakhati = 30,
        MaliatBarArzeshAfzodehForosh = 34,
        Sarmayeh=37,
        KharidKala = 43,
        BarghashtAzKharid = 44,
        ForosheKala = 46,
        BarghashtAzForosh = 47,
        TakhfifatNagdiKharid = 55,
        SayereDaramadha = 56,
        HazinehBazaryabiVaPorsant=71,
        SayereHazinehayeToziVaForosh = 73,
        HazinehTakhfifatNagdiForosh = 74,
    }

    public enum SandType
    {
        // سند
        [Description("دریافت")]
        Receive = 1,
        [Description("پرداخت")]
        Payment = 2,
        [Description("سند دستی")]
        SanadDasti = 3,
        [Description("انتقال وجه")]
        EntegalVajh = 4,

        // چک
        [Description("وصول چک دریافتی")]
        Vosool = 5,
        [Description("خواباندن چک دریافتی به حساب")]
        Khabandan = 6,
        [Description("عودت دادن چک دریافتی")]
        Odat = 7,
        [Description("وصول چک پرداختی")]
        VosoolPardakhti = 8,
        [Description("عودت گرفتن چک پرداختی")]
        OdatPardakhti = 9,

        // فاکتور
        [Description("فاکتور فروش")]
        FactorForosh = 11,
        [Description("فاکتور برگشت از فروش")]
        FactorBargashtiForosh = 12,
        [Description("فاکتور خرید")]
        FactorKharid = 13,
        [Description("فاکتور برگشت از خرید")]
        FactorBargashtiKharid = 14,
        CloseSanad=15,
        OpenSanad=16,
        TavazonSanad=17
    }

    public enum FactorType
    {
        [Description("خرید")]
        Kharid = 1,
        [Description("فروش")]
        Foroosh = 2,
        [Description("برگشت از فروش")]
        BargashAzForoosh = 3,
        [Description("برگشت از خرید")]
        BargashAzKharid = 4
    }

    public enum ChequeStatus
    {
        chequeDaryafti = 1,
        ChequeKhabidebeHesab = 2,
        ChequeVosolShode = 3,
        ChequeOdadt = 4,
        ChequeBargasht = 5,
        ChequeKharj = 6,

        ChequePardakhti = 11,
        ChequeVosolPardakhti = 12,
        ChequeOdatPardakhti = 13
    }

    public enum SarfaslGroup
    {
        daraei = 1,
        bedhi = 2,
        saham = 3,
        kharid = 4,
        forosh = 5,
        daramd = 6,
        hazineh = 7,
        saeir = 8
    }


}