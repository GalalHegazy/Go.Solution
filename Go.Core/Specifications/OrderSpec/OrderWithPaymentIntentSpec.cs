using Go.Core.Entities.Order_Aggregate;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Go.Core.Specifications.OrderSpec
{
    public class OrderWithPaymentIntentSpec : BaseSpecifications<Order>
    {
        public OrderWithPaymentIntentSpec(string? paymentIntentId) : base(O => O.PaymentIntentId == paymentIntentId){}
    }
}
