using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VPBANK.RMD.EFCore.Entities
{
    public abstract class BaseEntity<TKey> : RequestableEntity, IEntity<TKey> where TKey : IEquatable<TKey>
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Column(EfCoreConstants.Pk_Id, Order = 0)]
        public abstract TKey Pk_Id { get; set; }

        #region override method for key (TKey)

        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (!(obj is BaseEntity<TKey> entity)) return false;
            return entity.Pk_Id.Equals(Pk_Id);
        }

        public override int GetHashCode()
        {
            if (Pk_Id == null) return 0;
            return Pk_Id.ToString().GetHashCode();
        }

        #endregion
    }

    public interface IEntity<out TKey> where TKey : IEquatable<TKey>
    {
        TKey Pk_Id { get; }
    }
}
