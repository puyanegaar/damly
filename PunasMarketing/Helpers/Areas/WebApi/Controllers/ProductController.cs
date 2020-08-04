using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Areas.WebApi.Controllers
{
    public class ProductController : ApiController
    {
        private readonly ProductRepository _blProduct;
        private readonly ProductionStatusRepository _blProductStatus;
        private readonly UnitRepository _blUnit;

        public ProductController(ProductRepository blProduct, ProductionStatusRepository blProductStatus)
        {
            _blProduct = blProduct;
            _blProductStatus = blProductStatus;
        }

        
        [HttpGet]
        public IHttpActionResult Get()
        {
            var products = _blProduct.Where(a=>a.IsAvailable == true && a.IsSellable == true);
            if (products == null)
            {
                return NotFound();
            }

            List<ProductResponse> productResponse = Mapper.Map<List<Product>, List<ProductResponse>>(products.ToList());
            foreach (var item in productResponse)
            {
                item.ImageName = Url.Content("~/Content/Upload/Image/Products/" + item.ImageName);
                item.ProductionStatus = _blProductStatus.Find(byte.Parse(item.ProductionStatus)).Value;
            }
            return Ok(productResponse);
        }

       
    }
}
