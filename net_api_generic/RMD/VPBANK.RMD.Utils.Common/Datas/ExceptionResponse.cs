using System;
using System.Net;

namespace VPBANK.RMD.Utils.Common.Datas
{
    public class BusinessExceptionResponse
    {
        public DateTime Timestamp { get; set; }

        // 200, 201, 404, 500
        // Ok, Created, NotFound, InternalServerError
        public HttpStatusCode Status { get; set; }

        public string Error { get; set; }

        public string Message { get; set; }

        public string Path { get; set; }
    }

    public class FieldValidateResponse
    {
        public string Error { get; set; }

        public string Field { get; set; }

        public string Description { get; set; }
    }
}
