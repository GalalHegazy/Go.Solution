
namespace Go.Core.Entities.Order_Aggregate
{
    public class DeliveryMethod : BaseEntity  // int  Id  // (Table)
    {
        public string ShortName { get; set; }   
        public string Description { get; set; }
        public decimal Cost { get; set; }   
        public string DeliveryTime  { get; set; }
        /*public ICollection<Order> Orders { get; set; } = new List<Order>();*/ // Navegtion Property (Many) Relation (1)
    }
}
