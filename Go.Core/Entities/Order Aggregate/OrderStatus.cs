using System.Runtime.Serialization;

namespace Go.Core.Entities.Order_Aggregate
{
    public enum OrderStatus  // (Component (Composite attribute for Order Table))*
    {
        [EnumMember(Value ="Pending")] // To Return Enum Label Value As string
        Pending,
        [EnumMember(Value = "Payment Received")]   // To Return Enum Label Value As string
        PaymentReceived,
        [EnumMember(Value = "Payment Failed")]  // To Return Enum Label Value As string
        PaymentFailed
    }
}
