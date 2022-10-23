using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MovieAdvice.Model.Entities
{
    public class User : BaseEntity
    {
        [StringLength(200)]
        public string LastName { get; set; }

        [StringLength(200)]
        public string FirstName { get; set; }

        [StringLength(200)]
        public string Email { get; set; }

        [StringLength(50)]
        public string Password { get; set; }

        [StringLength(500)]
        public string Token { get; set; }

        public DateTime? TokenExpireDate { get; set; }

        public string HashCode { get; set; }

        public bool IsActive { get; set; }

        public bool Deleted { get; set; }
    }
}
