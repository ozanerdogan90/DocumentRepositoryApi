using System.ComponentModel.DataAnnotations;

namespace DocumentRepositoryApi.Models
{
    public class User
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Birthday { get; set; }
    }
}
