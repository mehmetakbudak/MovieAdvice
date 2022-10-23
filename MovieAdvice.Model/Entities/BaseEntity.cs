using System.ComponentModel.DataAnnotations;

namespace MovieAdvice.Model.Entities
{
    public class BaseEntity
    {
        [Key]
        public int Id { get; set; }
    }
}
