using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Discount;
using System.Collections.Generic;

namespace Sabio.Services.Interfaces
{
    public interface IDiscountService
    {
        Paged<Discount> GetByCouponCode(int pageIndex, int pageSize, string cCode);

        List<Discount> GetByListingId(int listingId);

        List<Discount> GetAllCoupons();

        Paged<Discount> GetAll(int pageIndex, int pageSize);

        Paged<Discount> GetCreatedBy(int pageIndex, int pageSize, int createdBy);

        Discount GetById(int id);

        int Add(DiscountAddRequest model, int currId);

        void DeleteCoupon(int Id);

        void Update(DiscountUpdateRequest model, int userId);
    }
}
