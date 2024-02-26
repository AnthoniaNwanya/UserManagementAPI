

using Swashbuckle.AspNetCore.Annotations;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models
{
    public class User
    {
        [SwaggerSchema(ReadOnly = true)]
        public int Id { get; set; }

        [Required]
        public string? Firstname { get; set; }

        [Required]
        public string? Lastname { get; set; }

        [Required]
        public string? Email { get; set; }

        [Required]
        public string? PhoneNumber { get; set; }

        [Required]
        public DateOnly DOB { get; set; }

        [Required]
        public string? Password { get; set; }

    }
}
