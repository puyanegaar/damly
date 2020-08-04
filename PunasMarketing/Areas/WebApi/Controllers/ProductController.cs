using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PunasMarketing.Areas.WebApi.Controllers
{
    [Authorize]
    public class ProductController : ApiController
    {
        private readonly ProductRepository _productRepo;
        private readonly ProductionStatusRepository _blProductStatus;
        private readonly ProductCategoryRepository _blProductCategory;
        private readonly UnitRepository _blUnit;
        private readonly ProductPriceListRepository _blPriceList;
        private readonly PriceTypeRepository _blPriceType;
        private readonly ProductImageRepository _productImageRepo;
        private readonly OfferRepository _offerRepository;
        private readonly OfferItemRepository _offerItemRepo;
        private readonly CustomerRepository _customerRepo;

        public ProductController(
            ProductRepository productRepo,
            ProductionStatusRepository blProductStatus,
            ProductCategoryRepository blProductCategory,
            UnitRepository blUnit,
            ProductPriceListRepository blPriceList,
            PriceTypeRepository blPriceType,
            ProductImageRepository productImageRepo,
            OfferRepository offerRepository,
            OfferItemRepository offerItemRepo,
            CustomerRepository customerRepo)
        {
            _productRepo = productRepo;
            _blProductStatus = blProductStatus;
            _blProductCategory = blProductCategory;
            _blUnit = blUnit;
            _blPriceList = blPriceList;
            _blPriceType = blPriceType;
            _productImageRepo = productImageRepo;
            _offerRepository = offerRepository;
            _offerItemRepo = offerItemRepo;
            _customerRepo = customerRepo;
        }

        [HttpGet]
        public IHttpActionResult Get(short? productId = null, short? productCategoryId = null,
            short? lastId = null, int? count = null)
        {
            if (productId.HasValue)
            {
                productCategoryId = null;
                lastId = null;
                count = null;
            }

            lastId = lastId ?? short.MinValue; // set default value
            count = count ?? int.MaxValue; // set default value

            int? customerId = null;
            if (ApiAuth.IsInRole(Request, UserType.Customer))
            {
                customerId = ApiAuth.GetUserId(Request);
            }

            var productsWithDisocunt = _productRepo.GetProductsWithDiscount(
                customerId, lastId, count, productId, productCategoryId);

            List<ProductResponse> productResponses = new List<ProductResponse>();

            if(productsWithDisocunt != null)
            {
                foreach (var productWithDiscount in productsWithDisocunt)
                {
                    ProductResponse productResponse = new ProductResponse
                    {
                        Id = productWithDiscount.Id ?? 0,
                        ProductCode = productWithDiscount.ProductCode,
                        Name = productWithDiscount.Name,
                        MainUnitId = productWithDiscount.MainUnitId ?? 0,
                        MainUnitName = productWithDiscount.MainUnitName,
                        ProductCategoryId = productWithDiscount.ProductCategoryId ?? 0,
                        ProductCategoryName = productWithDiscount.ProductCategoryName,
                        ProductionStatus = _blProductStatus.Find(productWithDiscount.ProductionStatus ?? 0).Value,
                        Description = productWithDiscount.Description,
                        CustomerPrice = (long)(productWithDiscount.CustomerPrice ?? 0),
                    };

                    if (productWithDiscount.OfferTypeId.HasValue)
                    {
                        productResponse.Discount = new DiscountResponse
                        {
                            PriceWithDiscount = (long?)productWithDiscount.PriceWithDiscount,
                            GiftProductId = productWithDiscount.GiftProductId,
                            MinProductCount = productWithDiscount.MinProductCount,
                            GiftProductName = productWithDiscount.GiftProductName,
                            GiftProductCount = productWithDiscount.GiftProductCount
                        };
                    }

                    if (productId.HasValue)
                    {
                        productResponse.Images = GetProductImageNames(productId.Value);
                    }
                    else
                    {
                        var virtualPath = ApiHelpers.GetImageVirtualPath(EntityType.Product, productWithDiscount.Image);
                        var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);
                        string imageUrl = File.Exists(physicalPath) ? Url.Content(virtualPath) : null;
                        if (!string.IsNullOrWhiteSpace(imageUrl))
                        {
                            productResponse.Images = new List<string> { imageUrl };
                        }
                    }

                    var catalogVirtualPath = ApiHelpers.GetImageVirtualPath(EntityType.Product, productWithDiscount.CatalogFileName);
                    var catalogPhysicalPath = HttpContext.Current.Server.MapPath(catalogVirtualPath);
                    string catalogUrl = File.Exists(catalogPhysicalPath) ? Url.Content(catalogVirtualPath) : null;
                    if (!string.IsNullOrWhiteSpace(catalogUrl))
                    {
                        productResponse.CatalogFileName = catalogUrl;
                    }

                    productResponses.Add(productResponse);
                }
            }
          

            return Ok(productResponses);
        }


        //[HttpGet]
        //[Authorize]
        //public IHttpActionResult Get(int? lastId = null, int? count = null)
        //{
        //    lastId = lastId ?? 0; // set default value
        //    count = count ?? 10; // set default value

        //    var products = _blProduct.Where(i => i.IsAvailable && i.IsSellable && i.Id > lastId)?.Take(count.Value);
        //    if (products == null)
        //    {
        //        return null;
        //    }

        //    var units = _blUnit.Select();
        //    var productCats = _blProductCategory.Select();

        //    List<ProductResponse> productResponses = Mapper.Map<List<Product>, List<ProductResponse>>(products.ToList());
        //    foreach (var productResponse in productResponses)
        //    {
        //        try
        //        {
        //            productResponse.ProductionStatus = _blProductStatus.Find(byte.Parse(productResponse.ProductionStatus)).Value;
        //        }
        //        catch
        //        {
        //            // ignored
        //        }

        //        productResponse.ProductCategoryName = productCats.FirstOrDefault(a => a.Id == productResponse.ProductCategoryId)?.Name;
        //        productResponse.MainUnitName = units.FirstOrDefault(i => i.Id == productResponse.MainUnitId)?.Name;

        //        ProductPriceList productPrice = _blPriceList.Where(
        //            i => i.ProductId == productResponse.Id && i.PriceTypeId == 0).FirstOrDefault();
        //        if (productPrice != null)
        //        {
        //            productResponse.CustomerPrice = (long)productPrice.Price;
        //        }

        //        productResponse.Images = GetProductImageNames(productResponse.Id, 1);

        //        int? customerId = null;
        //        if (ApiAuth.IsInRole(Request, UserType.Customer))
        //        {
        //            customerId = ApiAuth.GetUserId(Request);
        //        }
        //        productResponse.Discount = GetDiscount(productResponse, customerId);
        //    }

        //    return Ok(productResponses);
        //}


        //[HttpGet]
        //public IHttpActionResult Get(int id)
        //{
        //    var product = _blProduct.Where(a => a.Id == id && a.IsAvailable && a.IsSellable).FirstOrDefault();
        //    if (product == null)
        //    {
        //        return NotFound();
        //    }

        //    ProductResponse productResponse = Mapper.Map<Product, ProductResponse>(product);

        //    try
        //    {
        //        productResponse.ProductionStatus = _blProductStatus.Find(byte.Parse(productResponse.ProductionStatus)).Value;
        //    }
        //    catch
        //    {
        //        // ignored
        //    }

        //    var units = _blUnit.Select();
        //    var productCats = _blProductCategory.Select();

        //    productResponse.ProductCategoryName = productCats.FirstOrDefault(a => a.Id == productResponse.ProductCategoryId)?.Name;
        //    productResponse.MainUnitName = units.FirstOrDefault(i => i.Id == productResponse.MainUnitId)?.Name;

        //    ProductPriceList productPrice = _blPriceList.Where(
        //        i => i.ProductId == productResponse.Id && i.PriceTypeId == 0).FirstOrDefault();
        //    if (productPrice != null)
        //    {
        //        productResponse.CustomerPrice = (long)productPrice.Price;
        //    }

        //    productResponse.Images = GetProductImageNames(productResponse.Id);

        //    int? customerId = null;
        //    if (ApiAuth.IsInRole(Request, UserType.Customer))
        //    {
        //        customerId = ApiAuth.GetUserId(Request);
        //    }
        //    productResponse.Discount = GetDiscount(productResponse, customerId);

        //    return Ok(productResponse);
        //}



        [HttpGet]
        public IHttpActionResult GetCategories()
        {
            var productCategories = _blProductCategory.Select();
            if (productCategories == null)
            {
                return NotFound();
            }

            List<ProductCategoryResponse> productCategoryResponses = Mapper.Map<List<ProductCategory>, List<ProductCategoryResponse>>(productCategories.ToList());

            return Ok(productCategoryResponses);
        }

        [HttpGet]
        public IHttpActionResult GetCategories(int id)
        {
            var productCategories = _blProductCategory.Find(id);
            if (productCategories == null)
            {
                return NotFound();
            }

            ProductCategoryResponse productCategoryResponses = Mapper.Map<ProductCategory, ProductCategoryResponse>(productCategories);

            return Ok(productCategoryResponses);
        }


        //[HttpGet]
        //public IHttpActionResult GetProductByCategory(int id)
        //{
        //    var products = _blProduct.Where(a => a.ProductCategory.Id == id && a.IsAvailable && a.IsSellable);
        //    if (products == null)
        //    {
        //        return NotFound();
        //    }

        //    List<ProductResponse> productResponse = Mapper.Map<List<Product>, List<ProductResponse>>(products.ToList());
        //    foreach (var item in productResponse)
        //    {
        //        item.Images = GetProductImageNames(item.Id);
        //        item.ProductionStatus = _blProductStatus.Find(byte.Parse(item.ProductionStatus)).Value;
        //        item.MainUnitName = _blUnit.Find(item.MainUnitId)?.Name;
        //    }
        //    return Ok(productResponse);
        //}

        #region Helpers
        private List<string> GetProductImageNames(short productId, int? count = null)
        {
            List<string> imageNames = new List<string>();

            var productImageNames = _productImageRepo.Where(i => i.ProductId == productId).
                Select(i => i.ImageName).ToArray();

            count = count ?? productImageNames.Length;

            if (productImageNames.Length > 0)
            {
                for (int i = 0; i < count; i++)
                {
                    var imageName = productImageNames[i];

                    var virtualPath = ApiHelpers.GetImageVirtualPath(EntityType.Product, imageName);
                    var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

                    string imageUrl = File.Exists(physicalPath) ? Url.Content(virtualPath) : null;
                    if (!string.IsNullOrWhiteSpace(imageUrl))
                    {
                        imageNames.Add(imageUrl);
                    }
                }
            }

            return imageNames;
        }
        #endregion
    }
}
