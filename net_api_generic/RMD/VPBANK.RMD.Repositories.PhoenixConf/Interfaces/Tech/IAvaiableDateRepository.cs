using System.Collections.Generic;
using VPBANK.RMD.Data.PhoenixConf;
using VPBANK.RMD.Data.PhoenixConf.Functions;
using VPBANK.RMD.EFCore.Abstractions;

namespace VPBANK.RMD.Repositories.PhoenixConf.Interfaces.Tech
{
    public interface IAvaiableDateRepository : IQueryRepository<PhoenixConfContext, AvaiableDate>
    {
        public IEnumerable<AvaiableDate> FindCollectionAvaiDatesBySegment(string segment);
    }
}