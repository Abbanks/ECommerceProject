﻿using System.ComponentModel.DataAnnotations;

namespace ECommerceProject.Services.CouponAPI.Models
{
    public class Coupon
    {
        [Key]
        public int CouponId { get; set; }

        public string CouponCode { get; set; }

        public double DiscountAmount { get; set; }

        public int MinAmount { get; set; }

    }
}
