using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PunasMarketing.Areas.WebApi.Controllers
{
    //[Authorize]
    public class OrderController : ApiController
    {
        private readonly OrderRepository _orderRepo;
        private readonly OrderItemRepository _orderItemRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly PersonnelRepository _personnelRepo;
        private readonly ProductRepository _productRepo;

        public OrderController(
            OrderRepository orderRepo,
            OrderItemRepository orderItemRepo,
            CustomerRepository customerRepo,
            PersonnelRepository personnelRepo,
            ProductRepository productRepo)
        {
            _orderRepo = orderRepo;
            _orderItemRepo = orderItemRepo;
            _customerRepo = customerRepo;
            _personnelRepo = personnelRepo;
            _productRepo = productRepo;
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]OrderRequest orderRequest)
        {
            if (orderRequest == null)
            {
                return BadRequest();
            }

            if (!_customerRepo.Exists(orderRequest.CustomerId))
            {
                return BadRequest($"شناسه مشتری {orderRequest.CustomerId} معتبر نیست.");
            }

            if (orderRequest.MarketerId.HasValue)
            {
                if (!_personnelRepo.Where(i => i.JobTitleId == 0 && i.Id == orderRequest.MarketerId).Any())
                {
                    return BadRequest($"شناسه بازاریاب {orderRequest.MarketerId} معتبر نیست.");
                }
            }

            if (!ModelState.IsValid)
            {
                var message = ModelState.Values.FirstOrDefault()?.Errors[0].ErrorMessage;
                return BadRequest(message);
            }

            Order order = Mapper.Map<OrderRequest, Order>(orderRequest);
            List<OrderItem> orderItems = Mapper.Map<List<OrderItemRequest>, List<OrderItem>>(orderRequest.Items);

            order.CreatedDate = DateTime.Now;
            order.IsVerified = false;

            if (_orderRepo.Add(order))
            {
                int addedOrderId = _orderRepo.GetLastId();

                foreach (var item in orderItems.ToList())
                {
                    item.OrderId = addedOrderId;

                    Product product = _productRepo.Find(item.ProductId);

                    if (product == null)
                    {
                        _orderRepo.Delete(addedOrderId, out bool isUsed);
                        return BadRequest($"شناسه کالای {item.ProductId} معتبر نیست.");
                    }

                    if (!product.IsSellable || !product.IsAvailable)
                    {
                        _orderRepo.Delete(addedOrderId, out bool isUsed);
                        return BadRequest($"کالا با شناسه {item.ProductId} در حال حاضر قابل فروش نیست.");
                    }

                    item.UnitRate = product.UnitRate;

                    var productsWithDisocunt = _productRepo.GetProductsWithDiscount(
                        orderRequest.CustomerId, short.MinValue, 1, item.ProductId, null);

                    var productWithDiscount = productsWithDisocunt.FirstOrDefault();
                    if (productWithDiscount != null)
                    {
                        item.UnitPrice = (long)(productWithDiscount.CustomerPrice ?? 0);
                        if (productWithDiscount.GiftProductId.HasValue) // اگر دارای تخفیف کالایی بود
                        {
                            if (item.MainUnitCount >= productWithDiscount.MinProductCount.Value) // اگر تعداد کالا بیشتر از حد نصاب تخفیف کالایی بود
                            {
                                double giftProductCount = Math.Floor(item.MainUnitCount / productWithDiscount.MinProductCount.Value);
                                Product giftProduct = _productRepo.Find(productWithDiscount.GiftProductId.Value);
                                OrderItem giftItem = new OrderItem
                                {
                                    OrderId = addedOrderId,
                                    ProductId = productWithDiscount.GiftProductId.Value,
                                    MainUnitCount = giftProductCount,
                                    UnitRate = giftProduct.UnitRate,
                                    UnitPrice = 0,
                                    UnitDiscount = 0,
                                    Description = "کالای هدیه جشنواره"
                                };
                                orderItems.Add(giftItem);
                            }
                        }
                        else if (productWithDiscount.PriceWithDiscount.HasValue) // اگر دارای تخفیف ریالی یا درصدی بود
                        {
                            item.UnitDiscount = (productWithDiscount.CustomerPrice ?? 0) -
                                                (productWithDiscount.PriceWithDiscount ?? 0);
                            if (item.UnitDiscount < 0)
                            {
                                item.UnitDiscount = 0;
                            }
                        }
                    }
                }

                decimal totalPrice = 0;
                foreach (var orderItem in orderItems)
                {
                    var itemTotal = (decimal)orderItem.MainUnitCount * orderItem.UnitPrice;
                    var itemDiscount = (decimal)orderItem.MainUnitCount * orderItem.UnitDiscount;
                    var itemFinal = itemTotal - itemDiscount;
                    totalPrice += itemFinal;
                }

                if (_orderItemRepo.AddRange(orderItems))
                {
                    var addedOrder = _orderRepo.Find(addedOrderId);
                    addedOrder.TotalPrice = totalPrice;
                    _orderRepo.Update(addedOrder);

                    return Ok("سفارش شما با موفقیت ثبت شد.");
                }
                else
                {
                    return InternalServerError();
                }
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpGet]
        public IHttpActionResult GetByCustomerId(int id)
        {
            List<Order> orders = _orderRepo.Where(i => i.CustomerId == id).ToList();
            List<OrderResponse> orderResponses = Mapper.Map<List<Order>, List<OrderResponse>>(orders);

            foreach (var orderResponse in orderResponses)
            {
                orderResponse.CustomerName = _customerRepo.Find(orderResponse.CustomerId).BusinessName;
                if (orderResponse.MarketerId.HasValue)
                {
                    orderResponse.MarketerName = _personnelRepo.Find(orderResponse.MarketerId.Value).FullName;
                }

                //List<OrderItem> orderItems = _orderItemRepo.Where(i => i.OrderId == orderResponse.Id).ToList();
                //List<OrderItemResponse> orderItemResponses = Mapper.Map<List<OrderItem>, List<OrderItemResponse>>(orderItems);

                //foreach (var orderItemResponse in orderItemResponses)
                //{
                //    orderItemResponse.ProductName = _productRepo.Find(orderItemResponse.ProductId).Name;
                //}

                //orderResponse.Items = orderItemResponses;
            }

            return Ok(orderResponses.OrderByDescending(i => i.Id));
        }

        [HttpGet]
        public IHttpActionResult GetByMarketerId(int id)
        {
            List<Order> orders = _orderRepo.Where(i => i.MarketerId == id).ToList();
            List<OrderResponse> orderResponses = Mapper.Map<List<Order>, List<OrderResponse>>(orders);

            foreach (var orderResponse in orderResponses)
            {
                orderResponse.CustomerName = _customerRepo.Find(orderResponse.CustomerId).BusinessName;
                if (orderResponse.MarketerId.HasValue)
                {
                    orderResponse.MarketerName = _personnelRepo.Find(orderResponse.MarketerId.Value).FullName;
                }

                //List<OrderItem> orderItems = _orderItemRepo.Where(i => i.OrderId == orderResponse.Id).ToList();
                //List<OrderItemResponse> orderItemResponses = Mapper.Map<List<OrderItem>, List<OrderItemResponse>>(orderItems);

                //foreach (var orderItemResponse in orderItemResponses)
                //{
                //    orderItemResponse.ProductName = _productRepo.Find(orderItemResponse.ProductId).Name;
                //}

                //orderResponse.Items = orderItemResponses;
            }

            return Ok(orderResponses.OrderByDescending(i => i.Id));
        }

        [HttpGet]
        public IHttpActionResult GetDetails(int id)
        {
            var order = _orderRepo.Find(id);
            if (order == null)
            {
                return NotFound();
            }
            OrderResponse orderResponse = Mapper.Map<Order, OrderResponse>(order);

            orderResponse.CustomerName = _customerRepo.Find(orderResponse.CustomerId).BusinessName;
            if (orderResponse.MarketerId.HasValue)
            {
                orderResponse.MarketerName = _personnelRepo.Find(orderResponse.MarketerId.Value).FullName;
            }

            List<OrderItem> orderItems = _orderItemRepo.Where(i => i.OrderId == orderResponse.Id).ToList();
            List<OrderItemResponse> orderItemResponses = Mapper.Map<List<OrderItem>, List<OrderItemResponse>>(orderItems);

            foreach (var orderItemResponse in orderItemResponses)
            {
                orderItemResponse.ProductName = _productRepo.Find(orderItemResponse.ProductId).Name;
            }

            orderResponse.Items = orderItemResponses;

            return Ok(orderResponse);
        }
    }
}
