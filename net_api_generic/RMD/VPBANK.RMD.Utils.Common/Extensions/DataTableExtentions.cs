using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity.Core.Mapping;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;

namespace VPBANK.RMD.Utils.Common.Extensions
{
    public static class DataTableExtentions
    {
        public static string GetTableName<T>(this DbContext context) where T : class
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName(typeof(T));
        }

        public static string GetTableName(this DbContext context, Type t)
        {
            ObjectContext objectContext = ((IObjectContextAdapter)context).ObjectContext;
            return objectContext.GetTableName(t);
        }

        private static readonly Dictionary<Type, string> TableNames = new Dictionary<Type, string>();

        public static string GetTableName(this ObjectContext context, Type t)
        {
            string result;

            if (!TableNames.TryGetValue(t, out result))
            {
                lock (TableNames)
                {
                    if (!TableNames.TryGetValue(t, out result))
                    {

                        string entityName = t.Name;

                        ReadOnlyCollection<EntityContainerMapping> storageMetadata = context.MetadataWorkspace.GetItems<EntityContainerMapping>(DataSpace.CSSpace);

                        foreach (EntityContainerMapping ecm in storageMetadata)
                        {
                            EntitySet entitySet;
                            if (ecm.StoreEntityContainer.TryGetEntitySetByName(entityName, true, out entitySet))
                            {
                                //TODO: brackets
                                result = entitySet.Schema + "." + entitySet.Table;
                                break;
                            }
                        }

                        TableNames.Add(t, result);
                    }
                }
            }

            return result;
        }
    }
}
