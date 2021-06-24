using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Sabio.Models.Requests.Discount
{
    public class DiscountAddRequest
    {
        [Required]
        [Range(1, Int32.MaxValue)]
        public int ListingId { get; set; }

        [Required]
        [StringLength(20, MinimumLength = 5)]
        public string CouponCode { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 10)]
        public string Title { get; set; }

        public string Description { get; set; }
        
        [Required]
        public float Percentage { get; set; }
        
        [Requied]
        public DateTime ValidFrom { get; set; }
        
        [Required]
        public DateTime ValidUntil { get; set; }

        [Required]
        public bool IsRedeemedAllowed { get; set; }
    }
}
