using Sabio.Data;
using Sabio.Data.Providers;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Discount;
using Sabio.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Text;

namespace Sabio.Services
{
    public class DiscountService : IDiscountService
    {
        IDataProvider _data = null;

        public DiscountService(IDataProvider data)
        {
            _data = data;
        }

        public Paged<Discount> GetByCouponCode(int pageIndex, int pageSize, string couponCode)
        {
            Paged<Discount> discountList = null;
            List<Discount> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Discounts_SelectByCouponCode]";
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@pageIndex", pageIndex);
                paramCol.AddWithValue("@pageSize", pageSize);
                paramCol.AddWithValue("@CouponCode", couponCode);
            },
            delegate(IDataReader reader, short set) 
            {
                int index = 0;
                Discount discount = MapDiscountDetails(reader, ref index);
                if(totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index);
                }
                if(list == null)
                {
                    list = new List<Discount>();
                }
                list.Add(discount);
            });
            if(list != null)
            {
                discountList = new Paged<Discount>(list, pageIndex, pageSize, totalCount);
            }
            return discountList;
        }

        public List<Discount> GetByListingId(int listingId)
        { 
            List<Discount> list = null;
            

            string procName = "[dbo].[Discounts_Select_ByListingId]";
            _data.ExecuteCmd(procName, inputParamMapper: delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@ListingId", listingId);
            },
            singleRecordMapper: delegate (IDataReader reader, short set)
            {
                int index = 0;

                Discount discount = MapDiscountDetails(reader, ref index);
                
                if (list == null)
                {
                    list = new List<Discount>();
                }
                list.Add(discount);
            });
            
            return list;
        }

        public List<Discount> GetAllCoupons()
        {
            List<Discount> list = null;
            string procName = "[dbo].[Discounts_SelectAll_V2]";
            _data.ExecuteCmd(procName, inputParamMapper: null
                ,
                singleRecordMapper: delegate (IDataReader reader, short set)
                {
                    int index = 0;
                    Discount coupons = MapDiscountDetails(reader, ref index);
                    if (list == null)
                    {
                        list = new List<Discount>();
                    }
                    list.Add(coupons);
                }
            );
            return list;
        }


        public Paged<Discount> GetAll(int pageIndex, int pageSize)
        {
            Paged<Discount> discountList = null;
            List<Discount> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Discounts_SelectAll]";
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@pageIndex", pageIndex);
                paramCol.AddWithValue("@pageSize", pageSize);
            },
            delegate (IDataReader reader, short set)
            {
                int index = 0;

                Discount discount = MapDiscountDetails(reader, ref index);

                if(totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index);
                }
                if (list == null)
                {
                    list = new List<Discount>();
                }
                list.Add(discount);
            });
            if (list != null)
            {
                discountList = new Paged<Discount>(list, pageIndex, pageSize, totalCount);
            }
            return discountList;
        }

        public Paged<Discount> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            Paged<Discount> discountList = null;
            List<Discount> list = null;
            int totalCount = 0;

            string procName = "[dbo].[Discounts_Select_ByCreatedBy]";
            _data.ExecuteCmd(procName, delegate (SqlParameterCollection paramCol)
            {
                paramCol.AddWithValue("@pageIndex", pageIndex);
                paramCol.AddWithValue("@pageSize", pageSize);
                paramCol.AddWithValue("@UserId", userId);
            },
            delegate (IDataReader reader, short set)
            {
                int index = 0;
                Discount discount = MapDiscountDetails(reader, ref index);

                if(totalCount == 0)
                {
                    totalCount = reader.GetSafeInt32(index);
                }
                if(list == null)
                {
                    list = new List<Discount>();
                }
                list.Add(discount);
            });
            if (list != null) 
            {
                discountList = new Paged<Discount>(list, pageIndex, pageSize, totalCount);
            }
            return discountList;
        }

        public Discount GetById(int id)
        {
            string procName = "[dbo].[Discounts_SelectById]";
            Discount discount = null;

            _data.ExecuteCmd(procName, delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            }, delegate (IDataReader reader, short set)
            {
                int index = 0;
                discount = MapDiscountDetails(reader, ref index);
            });
            return discount;
        }

        public int Add(DiscountAddRequest model, int currId)
        {
            int id = 0;

            string procName = "[dbo].[Discounts_Insert]";
            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, currId);
                col.AddWithValue("@CreatedBy", currId);
                SqlParameter idOut = new SqlParameter("@Id", SqlDbType.Int);
                idOut.Direction = ParameterDirection.Output;

                col.Add(idOut);
            }
            , returnParameters: delegate (SqlParameterCollection returnCollections)
            {
                object oId = returnCollections["@Id"].Value;
                int.TryParse(oId.ToString(), out id);
            });
            return id;
        }

        public void DeleteCoupon(int id)
        {
            string procName = "[dbo].[Discounts_DeleteById]";

            _data.ExecuteNonQuery(procName, inputParamMapper: delegate (SqlParameterCollection col)
            {
                col.AddWithValue("@Id", id);
            });
        }

        public void Update(DiscountUpdateRequest model, int userId)
        {
            string procName = "[dbo].[Discounts_Update]";
            _data.ExecuteNonQuery(procName,
            inputParamMapper: delegate (SqlParameterCollection col)
            {
                AddCommonParams(model, col, userId);
                col.AddWithValue("@Id", userId);
            });
        }

        public Discount MapDiscountDetails(IDataReader reader, ref int index)
        {
            Discount discount = new Discount();

            discount.Id = reader.GetSafeInt32(index++);
            discount.ListingId = reader.GetSafeInt32(index++);
            discount.CouponCode = reader.GetSafeString(index++);
            discount.Title = reader.GetSafeString(index++);
            discount.Description = reader.GetSafeString(index++);
            discount.Percentage = reader.GetSafeDecimal(index++);
            discount.ValidFrom = reader.GetSafeUtcDateTime(index++);
            discount.ValidUntil = reader.GetSafeUtcDateTime(index++);
            discount.IsRedeemedAllowed = reader.GetSafeBool(index++);
            discount.DateCreated = reader.GetSafeUtcDateTime(index++);
            discount.DateModified = reader.GetSafeUtcDateTime(index++);
            discount.CreatedBy = reader.GetSafeInt32(index++);
            discount.ModifiedBy = reader.GetSafeInt32(index++);

            return discount;
        }

        private static void AddCommonParams(DiscountAddRequest model, SqlParameterCollection col, int userId)
        {
            col.AddWithValue("@ListingId", model.ListingId);
            col.AddWithValue("@CouponCode", model.CouponCode);
            col.AddWithValue("@Title", model.Title);
            col.AddWithValue("@Description", model.Description);
            col.AddWithValue("@Percentage", model.Percentage);
            col.AddWithValue("@ValidFrom", model.ValidFrom);
            col.AddWithValue("@ValidUntil", model.ValidUntil);
            col.AddWithValue("@IsRedeemedAllowed", model.IsRedeemedAllowed);
            col.AddWithValue("@ModifiedBy", userId);
        }
    }
}
