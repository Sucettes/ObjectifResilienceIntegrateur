using System;
using System.ComponentModel.DataAnnotations;

namespace Gwenael.Domain.Entities
{
    public abstract class EntityBase<T>
    {
        [Key] public T Id { get; protected set; }

        public DateTime CreationDate { get; protected set; }
        public DateTime? LastUpdateDate { get; protected set; }

        public bool IsDeleted { get; protected set; }
        public DateTime? DeleteDate { get; protected set; }

        protected EntityBase()
        {
            CreationDate = DateTime.UtcNow;
        }

        public void SoftDelete()
        {
            IsDeleted = true;
            DeleteDate = DateTime.UtcNow;
        }

        public void Updated()
        {
            LastUpdateDate = DateTime.UtcNow;
        }
    }
}