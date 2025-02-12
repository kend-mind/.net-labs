using System;

namespace VPBANK.RMD.Services.PhoenixConf.Email
{
    public class CollectionSendEmailReq
    {
        public DateTime businessDate { get; set; }
        public string segment { get; set; }
        public string osCompany { get; set; }
    }
}
