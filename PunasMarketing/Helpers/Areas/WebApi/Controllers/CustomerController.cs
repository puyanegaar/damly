using System;
using AutoMapper;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;
using PunasMarketing.Areas.WebApi.Models;


namespace PunasMarketing.Areas.WebApi.Controllers
{
    public class CustomerController : ApiController
    {
        private readonly CustomerRepository _blCustomer;


        public CustomerController(CustomerRepository blCustomer)
        {
            _blCustomer = blCustomer;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var customers = _blCustomer.Select();
            if (customers == null)
            {
                return NotFound();
            }
            
            List<CustomerResponse> customerResponses = Mapper.Map<List<Customer>, List<CustomerResponse>>(customers.ToList());
            foreach (var item in customerResponses)
            {
               item.ImageName = Url.Content("~/Content/Upload/Image/Customers/" + item.ImageName);
            }
            return Ok(customerResponses);
        }

        [HttpGet]
        public IHttpActionResult Get([FromUri]int id)
        {
            var customer = _blCustomer.Find(id);

            if (customer == null)
            {
                return NotFound();
            }

            CustomerResponse customerResponse = Mapper.Map<Customer, CustomerResponse>(customer);
            customerResponse.ImageName = Url.Content("~/Content/Upload/Image/Customers/" + customerResponse.ImageName);
            return Ok(customerResponse);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]CustomerResponse customerResponse)
        {
            Customer customer = Mapper.Map<CustomerResponse, Customer>(customerResponse);
            customer.Password = "123";
            customer.IsActive = false;
            customer.SaltCode = "123";
            customer.CustomerCategoryId = null;
            customer.Username = customerResponse.Mobile1;

            if (!ModelState.IsValid)
            {
                var message = ModelState.Values.FirstOrDefault().Errors[0].ErrorMessage;
                return BadRequest(message);
            }

            if (_blCustomer.Add(customer))
            {
                return Ok("مشتری جدید با موفقیت ثبت شد");

            }
            else
            {
                return BadRequest("خطا در ثبت مشتری جدید!");
            }
        }

        [HttpPost]
        public IHttpActionResult PostImage(int id)
        {
            try
            {
                HttpRequest httpRequest = HttpContext.Current.Request;

                if (httpRequest.Files.Count <= 0)
                {
                    return BadRequest("فایل، به درستی، بارگزاری نشد!");
                }


                HttpPostedFile postedFile = httpRequest.Files[0];
                string imageName = "Customer" + "_" + TimeSpan.TicksPerSecond + Path.GetExtension(postedFile.FileName);
                if (postedFile.ContentLength > 0)
                {

                    int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

                    IList<string> allowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
                    string ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
                    string extension = ext.ToLower();
                    if (!allowedFileExtensions.Contains(extension))
                    {
                        return BadRequest("فایل تصویری باید در یکی از فرمت‌های .jpg یا .gif یا .png باشد!");
                    }

                    if (postedFile.ContentLength > MaxContentLength)
                    {
                        return BadRequest("حجم فایل، باید کمتر از 2 مگابایت باشد!");
                    }

                    string directoryVirtualPath = "~/Content/Upload/Image/Customers/";
                    string directoryPhysicalPath = HttpContext.Current.Server.MapPath(directoryVirtualPath);
                    //Directory.CreateDirectory(directoryPhysicalPath);


                    string filePhysicalPath = HttpContext.Current.Server.MapPath(
                        directoryVirtualPath + "/" + imageName);

                    postedFile.SaveAs(filePhysicalPath);
                }

                var customer = _blCustomer.Find(id);
                customer.ImageName = imageName;
                _blCustomer.Update(customer);

                return Ok("تصویر با موفقیت بارگزاری شد.");
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
        
        [HttpPut]
        public IHttpActionResult Update([FromBody]CustomerResponse customerResponse)
        {
            Customer customer = Mapper.Map<CustomerResponse, Customer>(customerResponse);
            Customer dbCustomer = _blCustomer.Find(customer.Id);
            customer.Password = dbCustomer.Password;
            customer.SaltCode = dbCustomer.SaltCode;


            if (_blCustomer.Update(customer))
            {
                return Ok("مشتری مورد نظر با موفقیت ویرایش شد.");
            }
            else
            {
                return BadRequest("خطا در ویرایش مشتری جدید!");
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete([FromUri]int id)
        {
            Customer customer = _blCustomer.Find(id);
            if (customer == null)
            {
                return NotFound();
            }

            if (_blCustomer.Delete(id))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }

        

       
    }
}
