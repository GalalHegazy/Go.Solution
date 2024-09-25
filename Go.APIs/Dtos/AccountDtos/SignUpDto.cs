using System.ComponentModel.DataAnnotations;

namespace Go.APIs.Dtos.AccountDtos
{
    public class SignUpDto
    {
        [Required]
        public string Name { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Phone { get; set; }
        [Required]
        public string Password { get; set; }

    }
}
