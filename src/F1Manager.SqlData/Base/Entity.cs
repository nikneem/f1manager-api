using System.ComponentModel.DataAnnotations;

namespace F1Manager.SqlData.Base
{
    public abstract class Entity<T>
    {
        [Key]
        public T Id { get; set; }
    }
}
