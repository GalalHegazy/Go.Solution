using Microsoft.AspNetCore.Identity;

namespace Go.Core.Entities.Identity
{
    public class ApplicationUser : IdentityUser
    {
        //Relation One To One Btw ApplicationUser(optional) => Address (mandatory) 
        public string DisplayName {  get; set; }    
        public Address?  Address { get; set; } // Navgtion Prop For One (Optional)
    }
}
