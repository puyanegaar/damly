using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PunasMarketing.Areas.WebApi.Controllers
{

    //[Authorize]
    public class CustomerCategoryController : ApiController
    {
        private readonly CustomerCategoryRepository _blCustomerCategory;


        public CustomerCategoryController(CustomerCategoryRepository blCustomerCategory)
        {
            _blCustomerCategory = blCustomerCategory;
        }


        //[HttpGet]
        //public IHttpActionResult Get()
        //{
        //    var customerCategories = _blCustomerCategory.Select();
        //    if (customerCategories == null)
        //    {
        //        return NotFound();
        //    }

        //    List<CustomerCategoryResponse> customerCategoryResponses = Mapper.Map<List<CustomerCategory>, List<CustomerCategoryResponse>>(customerCategories.ToList());

        //    return Ok(customerCategoryResponses);
        //}


        //[HttpGet]
        //public IHttpActionResult Get(int id)
        //{
        //    var customerCategory = _blCustomerCategory.Find(id);

        //    if (customerCategory == null)
        //    {
        //        return NotFound();
        //    }

        //    CustomerCategoryResponse customerCategoryResponse = Mapper.Map<CustomerCategory, CustomerCategoryResponse>(customerCategory);
        //    return Ok(customerCategoryResponse);
        //}       
    }
}
