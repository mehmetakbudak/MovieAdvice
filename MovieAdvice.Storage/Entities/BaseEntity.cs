using System.ComponentModel.DataAnnotations;

namespace MovieAdvice.Storage.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
