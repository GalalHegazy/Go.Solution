
using System.ComponentModel.DataAnnotations;

namespace AdminDashBoard.ViewModels.Users
{
    public class UserViewModel
    {
        public string Id { get; set; }
        [Display(Name ="Name")]
        public string UserName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public IEnumerable<string> Roles  { get; set; } 
    }
}
