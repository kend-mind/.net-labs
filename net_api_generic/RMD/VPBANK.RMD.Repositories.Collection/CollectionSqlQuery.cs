namespace VPBANK.RMD.Repositories.Collection
{
    public class CollectionSqlQuery
    {
        public static readonly string get_Collection_Repay_Report = "EXECUTE Collection.dbo.get_Collection_Repay_Report '{0}', '{1}', {2}, '{3}'";
        public static readonly string get_Collection_NewBook_Report = "EXECUTE Collection.dbo.get_Collection_NewBook_Report '{0}', {1}, '{2}'";

        public static readonly string Collection_Dead_Loan_Truncate_Table = "TRUNCATE TABLE Core.Collection_Dead_Loan";
        public static readonly string Collection_Sell_Loan_Truncate_Table = "TRUNCATE TABLE Core.Collection_Sell_Loan";
        public static readonly string Con_Collection_List_Daily_Truncate_Table = "TRUNCATE TABLE Core.Con_Collection_List";
        public static readonly string Collection_Repay_Adj_Truncate_Table = "TRUNCATE TABLE Core.Collection_Repay_Adj";
        public static readonly string Collection_DPD_Manual_Truncate_Table = "TRUNCATE TABLE Core.Collection_DPD_Manual";
    }
}
