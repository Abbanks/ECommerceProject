using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Services.CouponAPI.Models.Dto
{
    public class CouponDto
    {
        [Required]
        public string CouponCode { get; set; }
        [Required]
        public double DiscountAmount { get; set; }
        [Required]
        public int MinAmount { get; set; }
    }
}
