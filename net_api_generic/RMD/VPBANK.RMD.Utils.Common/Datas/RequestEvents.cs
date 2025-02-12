using System.ComponentModel;

namespace VPBANK.RMD.Utils.Common.Datas
{
    public static class RequestEvents
    {
        public static readonly string DB_SAVE = "db_save";
        public static readonly string DB_SAVE_EXTENTION = "db_save_extention";
        public static readonly string END_POINT = "endpoint";
        public static readonly string MANUAL_FILE = "manual_file";
        public static readonly string ASSUMPTION_FILE = "assumption_file";
        public static readonly string CF_ASSUMPTION_DETAILS = "cf_assumption_details";
    }

    public static class RequestSegment
    {
        public static string SEGMENT_LAST_SEND_REQ = "/req";
        public static string SEGMENT_LAST_APPROVE_REQ = "/approve";

        public static string FILE_UPLOAD_CLASS = "vpbank.risk.upload.model.FileUploadInfo";
        public static string END_POINT_APPROVE_FILE_UPLOAD = "/file/upload";
        public static string END_POINT_APPROVE_REQ = @"/request/approve/{0}";

        public static string PAYLOAD_SERIALIZE = @"/file/upload/payload/serialize";
        public static string PAYLOAD_DESERIALIZE = @"/file/upload/payload/deserialize/{0}";


        public static string NOTIFICATION_SEND_MSG = @"/noti/send";
    }

    public class MESSAGE_TYPE
    {
        public static readonly string INFO = "INFO";
        public static readonly string WARNING = "WARNING";
        public static readonly string ERROR = "ERROR";
        public static readonly string SUCCESS = "SUCCESS";
        public static readonly string CUSTOM = "CUSTOM";
    }

    public enum RequestStep
    {
        NONE,
        PENDING,
        APPROVED,
        REJECT,
        CANCEL,
        DELETED
    }

    public static class RequestStatus
    {
        public static string PENDING = "Pending";
        public static string APPROVED = "Approved";
        public static string CANCEL = "Cancel";
        public static string REJECT = "Reject";
        public static string DELETED = "Delete";
    }

    public static class ReqObjectTypes
    {
        public static string OLD = "Old";
        public static string NEW = "New";
        public static string DELETE = "Delete";
    }

    public enum ActionTypes
    {
        [Description("INFO")]
        INFO,

        [Description("INSERT")]
        INSERT,
        [Description("UPDATE")]
        UPDATE,
        [Description("DELETE")]
        DELETE,
        [Description("QUERY")]
        QUERY,
        [Description("FIND_BY_ID")]
        FIND_BY_ID,

        [Description("REQUEST")]
        REQUEST,
        [Description("APPROVAL")]
        APPROVAL,
        [Description("CANCEL")]
        CANCEL,
        [Description("REJECT")]
        REJECT,

        [Description("EXECUTE")]
        EXECUTE,
        [Description("UPLOAD")]
        UPLOAD,
        [Description("RE_UPLOAD")]
        RE_UPLOAD,

        [Description("BATCH_EXEC")]
        BATCH_EXEC,
        [Description("BATCH_JOB_MONITOR")]
        BATCH_JOB_MONITOR,
        [Description("JOB_EXEC")]
        JOB_EXEC,
        [Description("ARCHIVE")]
        ARCHIVE,
        [Description("TERMINATE")]
        TERMINATE,
        [Description("ADJUST_EXECUTE")]
        ADJUST_EXECUTE
    }

    public class ApproveStatusReq
    {
        public int Id { get; set; }
        public int FkRequestId { get; set; }
        public string Approver { get; set; }
        public string Comment { get; set; }
        public bool End { get; set; }
        public string Status { get; set; }
        public int Step { get; set; }
    }
}
