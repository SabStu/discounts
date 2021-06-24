using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Sabio.Models;
using Sabio.Models.Domain;
using Sabio.Models.Requests.Discount;
using Sabio.Services;
using Sabio.Services.Interfaces;
using Sabio.Web.Controllers;
using Sabio.Web.Models.Responses;
using System;
using System.Collections.Generic;

namespace Sabio.Web.Api.Controllers
{
    [Route("api/coupons")]
    [ApiController]
    public class DiscountsApiController : BaseApiController
    {
        private IDiscountService _service = null;
        private IAuthenticationService<int> _authService = null;

        public DiscountsApiController(IDiscountService service
                , ILogger<DiscountsApiController> logger
                , IAuthenticationService<int> authService) : base(logger) 
        {
            _service = service;
            _authService = authService;
        }


        [HttpGet("details")]
        public ActionResult<ItemResponse<List<Discount>>> GetAllDetails()
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                List<Discount> coupons = _service.GetAllCoupons();
                if(coupons == null)
                {
                    code = 404;
                    response = new ErrorResponse("Not Found.");
                }
                else
                {
                    response = new ItemResponse<List<Discount>> { Item = coupons };
                }
            }
            catch (Exception ex)
            {
                code = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

        #region HTTPGET
        [HttpGet]
        public ActionResult<ItemResponse<Paged<Discount>>> GetAll(int pageIndex, int pageSize)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Discount> discount = _service.GetAll(pageIndex, pageSize);

                if (discount == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Discount>> { Item = discount };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("createdBy")]
        public ActionResult<ItemResponse<Discount>> GetCreatedBy(int pageIndex, int pageSize, int userId)
        {
            int iCode = 200;
            BaseResponse response = null;
            try
            {
                Paged<Discount> discount = _service.GetCreatedBy(pageIndex, pageSize, userId);

                if (discount == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Discount>> { Item = discount };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("listing/{id:int}")]
        public ActionResult<ItemsResponse<Discount>> GetCouponByListingId(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                List<Discount> discount = _service.GetByListingId(id);

                if (discount == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Not Found.");
                }
                else
                {
                    response = new ItemsResponse<Discount>{ Items = discount };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("code")]
        public ActionResult<ItemResponse<Discount>> GetCouponCodeDetails(int pageIndex, int pageSize, string couponCode)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Paged<Discount> discount = _service.GetByCouponCode(pageIndex, pageSize, couponCode);

                if (discount == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Not Found.");
                }
                else
                {
                    response = new ItemResponse<Paged<Discount>> { Item = discount };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpGet("{id:int}")]
        public ActionResult<ItemResponse<Discount>> GetById(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                Discount discount = _service.GetById(id);
                if (discount == null)
                {
                    iCode = 404;
                    response = new ErrorResponse("Coupon Id Not Found.");
                }
                else
                {
                    response = new ItemResponse<Discount> { Item = discount };
                }
            }
            catch (Exception ex)
            {
                iCode = 500;
                base.Logger.LogError(ex.ToString());
                response = new ErrorResponse($"Generic Error: {ex.Message}");
            }
            return StatusCode(iCode, response);
        } 
        #endregion

        [HttpPost]
        public ActionResult<ItemResponse<int>> Create(DiscountAddRequest model)
        {
            ObjectResult result = null;

            try
            {
                int userId = _authService.GetCurrentUserId();
                int id = _service.Add(model, userId);
                ItemResponse<int> response = new ItemResponse<int> { Item = id };

                result = Created201(response);
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.ToString());
                ErrorResponse response = new ErrorResponse(ex.Message);
                result = StatusCode(500, response);
            }
            return result;
        }

        [HttpDelete("{id:int}")]
        public ActionResult<SuccessResponse> DeleteCoupon(int id)
        {
            int iCode = 200;
            BaseResponse response = null;

            try
            {
                _service.DeleteCoupon(id);

                response = new SuccessResponse();
            }
            catch (Exception ex)
            {
                iCode = 500;
                Logger.LogError(ex.ToString());
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(iCode, response);
        }

        [HttpPut("{id:int}")]
        public ActionResult<ItemResponse<int>> Update(DiscountUpdateRequest model, int id)
        {
            int code = 200;
            BaseResponse response = null;
            try
            {
                _service.Update(model, id);
                response = new SuccessResponse();

            }
            catch (Exception ex)
            {
                code = 500;
                response = new ErrorResponse(ex.Message);
            }
            return StatusCode(code, response);
        }

    }
}
