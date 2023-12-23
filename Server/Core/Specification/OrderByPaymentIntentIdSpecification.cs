using Core.Entities.OrderAggregate;

namespace Core.Specification
{
    public class OrderByPaymentIntentIdSpecification : Specification<Order>
    {
        public OrderByPaymentIntentIdSpecification(string paymentIntentId) 
            : base(o => o.PaymentIntentId == paymentIntentId)
        {
        }
    }
}