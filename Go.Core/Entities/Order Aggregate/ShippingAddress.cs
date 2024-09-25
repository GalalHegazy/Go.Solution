
namespace Go.Core.Entities.Order_Aggregate
{
    public class ShippingAddress // (Component (Composite attribute for Order Table))
    {
        public string FirstName { get; set; }   
        public string LastName { get; set; }
        public string  Street { get; set; } 
        public string City { get; set; }
        public string Country { get; set; }

    }
}
