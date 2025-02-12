using System.ComponentModel;

namespace VPBANK.RMD.Utils.Common
{
    public class Enums
    {
        public enum ResolverCaseEnums
        {
            // Camel Case (ex: someVar, someClass, somePackage.xyz)
            Camelize = 1,
            // Pascal Case (ex: SomeVar, SomeClass, SomePackage.xyz).
            Pascalize,
            // some_var, some_class, some_package.xyz
            PascalizeUnderscore
        }

        /// <summary>  
        /// Enums for filter options same sequence UI is following
        /// </summary>
        public enum FilterOptions
        {
            // number, datetime
            Equals = 1,
            DoesNotEquals,
            LessThan,
            LessThanOrEquals,
            GreaterThan,
            GreaterThanOrEquals,
            Between,

            // string
            Contains,
            DoesNotContain,
            StartsWith,
            EndsWith,
            IsEmpty,
            IsNotEmpty
        }

        /// <summary>
        /// Enums for sorting options same sequence UI is following
        /// </summary>
        public enum SortOrders
        {
            [Description("ASC")]
            Asc = 1,
            [Description("DESC")]
            Desc = 2
        }
    }
}
