using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Utils.AuditLog.Models
{
    public class ElasticSearchPaging
    {
        public string Filters { get; set; } = SpecificSystems.BULLET;

        // start index = 0
        public int From { get; set; } = 0;

        // min limit 25 items
        public int Size { get; set; } = 25;
    }
}
