using Go.Core.Entities.Order_Aggregate;

namespace Go.Core.Specifications.OrderSpec
{
    public class OrderWithSpec : BaseSpecifications<Order>
    {
        public OrderWithSpec(string email) : base(O => O.BuyerEmail == email) 
        {
            Includes.Add(O => O.DeliveryMethod);  // Loud Nav Prop for Releation (1)  One

            Includes.Add(O => O.Items);  // Loud Nav Prop for Releation (1)  Many

            AddOrderByDesc(O => O.OrderDate); // Order Coming Data Desc By Order data of Order For Newest To Oldest

        }
        public OrderWithSpec(int orderId, string email) 
             : base(O => O.Id == orderId && O.BuyerEmail == email) 
        {
            Includes.Add(O => O.DeliveryMethod);  // Loud Nav Prop for Releation (1)  One

            Includes.Add(O => O.Items);  // Loud Nav Prop for Releation (1)  Many
        }
    }
}
