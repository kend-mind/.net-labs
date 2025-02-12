using System.Collections.Generic;
using VPBANK.RMD.EFCore.Entities.Commons;

namespace VPBANK.RMD.Services.Collection.Interfaces
{
    public interface ISegmentService
    {
        IEnumerable<SelectedItem> GetGui(bool? hasAll);
    }
}
