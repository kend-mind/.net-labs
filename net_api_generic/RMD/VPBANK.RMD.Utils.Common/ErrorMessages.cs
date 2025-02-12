namespace VPBANK.RMD.Utils.Common
{
    public static class ErrorMessages
    {
        // InternalServerError
        public static readonly string SE000 = "Internal Server Error, ({0}).";
        public static readonly string SE003 = "Username is incorrect.";
        public static readonly string SE402 = "Unauthorized";

        // NotFound
        public static readonly string SE011 = "Not found object.";
        // Invalid
        public static readonly string SE012 = "Invalid object.";

        //
        public static readonly string SE013 = "Internal Server Error\r\nCannot action approve in this request. Please try again.";

        public static readonly string EM204 = "The request has been successfully processed and that the response is intentionally blank.";



        // [Field #] is having wrong data format. Please make sure the data follows [Format #].
        public static readonly string EM002 = @"{0} is having wrong data format. Please make sure the data follows {1}.";
        public static readonly string EM003 = @"{0} has exceeded the maximum defined length of {1}.";
        public static readonly string EM004 = "\"{0}\" has already existed, please try again.";


        // [Field #] is a required field. Please fill in.
        public static readonly string EM001 = "{0} is a required field. Please fill in.";
        public static readonly string EM019 = "{0} must be equal or greater than {1}.";

        public static readonly string EM072 = "The data in {0} is not in Data Source.";

        public static readonly string EM123 = "The CL_OS_Start_Date can not be the future date.";
        public static readonly string EM124 = "The Execution date can not be the future date.";
        public static readonly string EM125 = "The CL_OS_Start_Date need to be future compared to Classify_BB_Date.";
        public static readonly string EM126 = "Can not update records having Reason which is not NULL.";
        public static readonly string EM127 = "Can not insert contracts which exist the set (Contract_id, CL_OS_Start_Date) on current Database.";
        public static readonly string EM128 = "Business_Date must be equal YYYYMMDD in file name.";
        public static readonly string EM129 = "Contract_ID and Customer_ID are NULL. Please fill in one of fields: Contract_ID or Customer_ID.";
        public static readonly string EM130 = "Contract_ID and Customer_ID are not NULL. You can only fill one of fields: Contract_ID or Customer_ID.";
        public static readonly string EM131 = "Contract_ID not exist on Current database. Please input Contract_ID exist on current database to update.";
        public static readonly string EM132 = "Customer_ID not exist on Current database. Please input Customer_ID exist on current database to update.";
        public static readonly string EM133 = "Can not update records Contract_ID having Reason which is not NULL.";
        public static readonly string EM134 = "Can not update records Customer_ID having Reason which is not NULL.";
        public static readonly string EM135 = "The field {0} must be NULL.";
        public static readonly string EM139 = "Cannot insert email information because set (Segment, Oustsource Company) existing in the database.";
        public static readonly string EM140 = "\"{0}\" Each email must separated by \";\".";
        public static readonly string EM141 = "[{0}] Invalid email.";

        public static readonly string EM142 = "Deletion is not allowed while Email Template is use in Collection_email.";
        public static readonly string EM145 = "Cannot deleted Days Past Due - DPD while Days Past Due - DPD is used in Conf_Collection_Fee_Ratio.";
        public static readonly string EM146 = "Please fill in DPD_CL_OS_Start_Date while Sell_Loan ='N'.";
        public static readonly string EM147 = "Please fill in Not_Bad_Debt_Flag while Dead_Loan='N'.";
    }
}
