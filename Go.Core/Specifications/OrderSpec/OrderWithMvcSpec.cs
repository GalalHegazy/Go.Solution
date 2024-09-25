using Go.Core.Entities.Order_Aggregate;

namespace Go.Core.Specifications.OrderSpec
{
    public class OrderWithMvcSpec : BaseSpecifications<Order>
    {
        public OrderWithMvcSpec()
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            Includes.Add(O => O.Address);
         
        }
        public OrderWithMvcSpec(string email) : base(O => O.BuyerEmail == email)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            Includes.Add(O => O.Address);
        }
        public OrderWithMvcSpec(int id) : base(P => P.Id == id)
        {
            Includes.Add(O => O.DeliveryMethod);
            Includes.Add(O => O.Items);
            Includes.Add(O => O.Address);
        }
    }
}
