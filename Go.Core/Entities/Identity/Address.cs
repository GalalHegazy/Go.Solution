namespace Go.Core.Entities.Identity
{
    public class Address : BaseEntity // Int Id
    {
        public string FristName { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public ApplicationUser User { get; set; } // Navgtion Prop For One (mandatory)
        public string ApplicationUserId { get; set; } // Fk 
    }
}