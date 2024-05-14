
namespace ECommerceProject.Web.Models
{
    public class CouponDtoResponse
    {
        public int CouponId { get; set; }
        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }
    }
}
