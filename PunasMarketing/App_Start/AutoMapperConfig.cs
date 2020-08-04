using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using ProvinceResponse = PunasMarketing.Areas.WebApi.Models.ProvinceResponse;

namespace PunasMarketing.App_Start
{
    public class AutoMapperConfig
    {
        public static void Initialize()
        {
            Mapper.Initialize((config) =>
            {
                config.CreateMap<Customer, CustomerResponse>().ReverseMap();

                config.CreateMap<Region, RegionResponse>().ReverseMap();

                config.CreateMap<City, CityResponse>().ReverseMap();

                config.CreateMap<Province, ProvinceResponse>().ReverseMap();

                config.CreateMap<CustomerCategory, CustomerCategoryResponse>().ReverseMap();

                config.CreateMap<Product, ProductResponse>().ReverseMap();

                config.CreateMap<Unit, UnitResponse>().ReverseMap();

                config.CreateMap<ProductCategory, ProductCategoryResponse>().ReverseMap();

                config.CreateMap<Offer, OfferResponse>().ReverseMap();

                config.CreateMap<OfferItem, OfferItemResponse>().ReverseMap();

                config.CreateMap<Personnel, MarketerResponse>().ReverseMap();

                config.CreateMap<Order, OrderRequest>().ReverseMap();

                config.CreateMap<OrderItem, OrderItemRequest>().ReverseMap();

                config.CreateMap<Order, OrderResponse>().ReverseMap();

                config.CreateMap<OrderItem, OrderItemResponse>().ReverseMap();

                config.CreateMap<Customer, CustomerUpdateRequest>().ReverseMap();

                config.CreateMap<Customer, CustomerCreateRequest>().ReverseMap();

                config.CreateMap<Factor, FactorResponse>().ReverseMap();

                config.CreateMap<FactorItem, FactorItemResponse>().ReverseMap();
            });
        }
    }
}