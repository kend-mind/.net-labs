using System;

namespace VPBANK.RMD.EFCore.Entities
{
    public abstract class AuditableEntity : IAuditableEntity
    {
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get; set; }
        public string Created_By { get; set; }
        public string Modified_By { get; set; }
    }

    public interface IAuditableEntity
    {
        public DateTime Created_At { get; set; }
        public DateTime Modified_At { get; set; }
        public string Created_By { get; set; }
        public string Modified_By { get; set; }
    }
}
