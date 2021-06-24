using System;

namespace Sabio.Models.Domain
{
    public class Discount
    {
        public int Id { get; set; }

        public int ListingId { get; set; }

        public string CouponCode { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public decimal Percentage { get; set; }

        public DateTime ValidFrom { get; set; }

        public DateTime ValidUntil { get; set; }

        public bool IsRedeemedAllowed { get; set; }

        public DateTime DateCreated { get; set; }

        public DateTime DateModified { get; set; }

        public int CreatedBy { get; set; }

        public int ModifiedBy { get; set; }
    }
}
