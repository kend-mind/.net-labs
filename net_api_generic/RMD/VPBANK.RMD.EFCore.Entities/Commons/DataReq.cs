using System.Collections.Generic;

namespace VPBANK.RMD.EFCore.Entities.Commons
{
    public class DataReq<T> where T : class
    {
        public int Id { get; set; }
        public string Comment { get; set; }
        public IEnumerable<T> Entity { get; set; }
    }
}
