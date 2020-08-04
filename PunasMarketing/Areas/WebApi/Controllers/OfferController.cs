using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using AutoMapper;
using Microsoft.AspNet.Identity;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Areas.WebApi.Controllers
{
    //[Authorize]
    public class OfferController : ApiController
    {
        private readonly OfferRepository _offerRepo;
        private readonly OfferItemRepository _offerItemRepo;
        private readonly OfferTypeRepository _offerTypeRepo;
        private readonly ProductRepository _productRepo;
        private readonly CustomerRepository _customerRepo;

        public OfferController(
            OfferRepository offerRepo,
            OfferItemRepository offerItemRepo,
            OfferTypeRepository offerTypeRepo,
            ProductRepository productRepo,
            CustomerRepository customerRepo)
        {
            _offerRepo = offerRepo;
            _offerItemRepo = offerItemRepo;
            _offerTypeRepo = offerTypeRepo;
            _productRepo = productRepo;
            _customerRepo = customerRepo;
        }

        [HttpGet]
        public IHttpActionResult GetActiveOffers()
        {
            int? customerId = null;

            if (ApiAuth.IsInRole(Request, UserType.Customer))
            {
                customerId = ApiAuth.GetUserId(Request);
            }

            var offers = _offerRepo.Where(
                a => a.IsActive && a.StartDate <= DateTime.Now && a.ExpDate >= DateTime.Now).ToList();

            if (customerId.HasValue)
            {
                short? customerCategoryId = _customerRepo.Find(customerId.Value)?.CustomerCategoryId;

                foreach (var offer in offers.ToList())
                {
                    if (!string.IsNullOrWhiteSpace(offer.ForCustomerCategories))
                    {
                        var customerCats = offer.ForCustomerCategories.Split(',');
                        if (!customerCats.Any())
                        {
                            continue; // همه میتوانند ببینند.
                        }

                        if (!customerCategoryId.HasValue || !customerCats.Contains(customerCategoryId.Value.ToString()))
                        {
                            offers.Remove(offer);
                        }
                    }
                }
            }

            List<OfferResponse> offerResponses = Mapper.Map<List<Offer>, List<OfferResponse>>(offers);

            foreach (var offerResponse in offerResponses.ToList())
            {
                string path = HttpContext.Current.Server.MapPath("~/Content/Upload/Image/Offers/");
                if (string.IsNullOrWhiteSpace(offerResponse.ImageName))
                {
                    offerResponse.ImageName = null;
                }
                else
                {
                    string imgPath = Path.Combine(path, offerResponse.ImageName);
                    offerResponse.ImageName = File.Exists(imgPath)
                        ? Url.Content("~/Content/Upload/Image/Offers/" + offerResponse.ImageName)
                        : null;
                }

                var offerItems = _offerItemRepo.Where(i => i.OfferId == offerResponse.Id);
                List<OfferItemResponse> offerItemResponses = Mapper.Map<IQueryable<OfferItem>, List<OfferItemResponse>>(offerItems);

                foreach (var offerItemResponse in offerItemResponses)
                {
                    offerItemResponse.OfferTypeName = _offerTypeRepo.Find(offerItemResponse.OfferTypeId).Name;

                    if (offerItemResponse.ProductId != null)
                    {
                        offerItemResponse.ProductName = _productRepo.Find(offerItemResponse.ProductId.Value).Name;
                    }

                    if (offerItemResponse.GiftProductId != null)
                    {
                        offerItemResponse.GiftProductName = _productRepo.Find(offerItemResponse.GiftProductId.Value).Name;
                    }
                }

                if (offerItemResponses.Any())
                {
                    offerResponses.First(i => i.Id == offerResponse.Id).Items = offerItemResponses;
                }
                else
                {
                    offerResponses.RemoveAll(i => i.Id == offerResponse.Id);
                }
            }

            return Ok(offerResponses.OrderByDescending(i => i.Id));
        }
    }
}
