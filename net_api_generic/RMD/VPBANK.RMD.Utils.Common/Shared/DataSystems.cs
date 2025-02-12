using System.ComponentModel;

namespace VPBANK.RMD.Utils.Common.Shared
{
    public static class DataSystems
    {
        public const string Select_Item_All = "All";
    }

    public static class COLUMNS_NAME
    {
        public const string ENGINE_CODE = "Engine_Code";
        public const string ENTITY_CODE = "Entity_Code";
        public const string APPROACH = "Approach";
        public const string SCENARIO_ID = "Scenario_Id";
        public const string VERSION_ID = "Version_Id";
        public const string START_DATE = "Start_Date";
        public const string END_DATE = "End_Date";
    }

    public static class ENTITY_CODES
    {
        public const string VPBANK = "VPBANK";
    }

    public static class ENGINE_CODES
    {
        public const string SIRR = "SIRR";
        public const string GIRR = "GIRR";
        public const string FXR = "FXR";
        public const string OPR = "OPR";
        public const string CA = "CA";
        public const string LCCF = "LCCF";
        public const string LMCF = "LMCF";
        public const string ICCF = "ICCF";
        public const string IMCF = "IMCF";
        public const string COTR = "COTR";
        public const string CF = "CF";
        public const string CR = "CR";
        public const string PSR = "PSR";
        public const string RECO = "RECO";
        public const string EIR = "EIR";
        public const string LIQUI_RPT = "LIQUI_RPT";
        public const string LRCF = "LRCF";
        public const string CAR_RPT = "CAR_RPT";
        public const string OCE = "OCE";
        public const string COLN = "COLN";
    }

    public static class CollectionTypes
    {
        public const string Con_Collection_List_Daily = "Con_Collection_List_Daily";
        public const string Collection_Dead_Loan = "Collection_Dead_Loan";
        public const string Collection_Sell_Loan = "Collection_Sell_Loan";
        public const string Collection_Campaign = "Collection_Campaign";
    }

    public enum CollectionReason
    {
        [Description("PLN")]
        PLN,
        [Description("HMK")]
        HMK,
        [Description("GLX")]
        GLX
    }
}
