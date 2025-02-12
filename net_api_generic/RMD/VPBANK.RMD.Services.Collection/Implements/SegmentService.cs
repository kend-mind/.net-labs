using System;
using System.Collections.Generic;
using System.Linq;
using VPBANK.RMD.Data.Collection;
using VPBANK.RMD.Data.Collection.Entities.POCOs;
using VPBANK.RMD.EFCore;
using VPBANK.RMD.EFCore.Entities.Commons;
using VPBANK.RMD.EFCore.Generics;
using VPBANK.RMD.Services.Collection.Interfaces;
using VPBANK.RMD.Utils.Common.Shared;

namespace VPBANK.RMD.Services.Collection.Implements
{
    public class SegmentService : ISegmentService
    {
        private readonly IUnitOfWork<CollectionContext> _unitOfWork;
        private readonly IGenericRepository<CollectionContext, Segment, int> _genSegmentRepository;
        public SegmentService(IUnitOfWork<CollectionContext> unitOfWork,
            IGenericRepository<CollectionContext, Segment, int> genSegmentRepository)
        {
            _unitOfWork = unitOfWork;
            _genSegmentRepository = genSegmentRepository;
        }

        public IEnumerable<SelectedItem> GetGui(bool? hasAll)
        {
            var results = new List<SelectedItem>();
            try
            {
                if (hasAll.Value && hasAll.Value)
                    results.Add(new SelectedItem
                    {
                        Key = DataSystems.Select_Item_All,
                        Val = DataSystems.Select_Item_All
                    });
                results.AddRange(_genSegmentRepository
                    .Queryable()
                    .AsEnumerable()
                    .Select(c => new SelectedItem
                    {
                        Key = c.Segment_Name,
                        Val = c.Segment_Name
                    })
                    .OrderBy(c => c.Key)
                    .Distinct()
                    .ToList());
            }
            catch (Exception)
            {
            }
            return results;
        }
    }
}
