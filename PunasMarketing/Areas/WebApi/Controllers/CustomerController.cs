using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;


namespace PunasMarketing.Areas.WebApi.Controllers
{
    //[Authorize]
    public class CustomerController : ApiController
    {
        private readonly CustomerRepository _customerRepo;
        private readonly TafsiliRepository _tafsiliRepo;

        public CustomerController(
            CustomerRepository customerRepo,
            TafsiliRepository tafsiliRepo)
        {
            _customerRepo = customerRepo;
            _tafsiliRepo = tafsiliRepo;
        }

        [HttpGet]
        public IHttpActionResult Get()
        {
            var customers = _customerRepo.Select();
            if (customers == null)
            {
                return NotFound();
            }

            List<CustomerResponse> customerResponses = Mapper.Map<List<Customer>, List<CustomerResponse>>(customers.ToList());
            foreach (var item in customerResponses)
            {
                if (string.IsNullOrWhiteSpace(item.ImageName))
                {
                    item.ImageName = null;
                }
                else
                {
                    var virtualPath = ApiHelpers.GetImageVirtualPath(EntityType.Customer, item.ImageName);
                    var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

                    string imageUrl = File.Exists(physicalPath) ? Url.Content(virtualPath) : null;
                    item.ImageName = imageUrl;
                }
            }
            return Ok(customerResponses);
        }

        [HttpGet]
        public IHttpActionResult Get(string username)
        {
            var customer = _customerRepo.FindByUsername(username);

            if (customer == null)
            {
                return NotFound();
            }

            CustomerResponse customerResponse = Mapper.Map<Customer, CustomerResponse>(customer);

            if (string.IsNullOrWhiteSpace(customerResponse.ImageName))
            {
                customerResponse.ImageName = null;
            }
            else
            {
                var virtualPath = ApiHelpers.GetImageVirtualPath(EntityType.Customer, customerResponse.ImageName);
                var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

                string imageUrl = File.Exists(physicalPath) ? Url.Content(virtualPath) : null;
                customerResponse.ImageName = imageUrl;
            }

            return Ok(customerResponse);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]CustomerCreateRequest request)
        {
            Customer customer = Mapper.Map<CustomerCreateRequest, Customer>(request);

            var keyNew = Utilities.GeneratePassword(20);
            var password = Utilities.EncodePassword(customer.CodeMelli, keyNew);
            customer.Username = customer.Mobile1;
            customer.Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#");
            customer.SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#");
            customer.IsActive = false;
            customer.CustomerCategoryId = null;

            if (!ModelState.IsValid)
            {
                var message = ModelState.Values.FirstOrDefault()?.Errors[0].ErrorMessage;
                return BadRequest(message);
            }

            string imageName = string.Empty;
            if (!string.IsNullOrWhiteSpace(request.ImageBase64))
            {
                imageName = "Customer" + "_" + Guid.NewGuid() + ".png";
                customer.ImageName = imageName;
            }

            if (_customerRepo.Add(customer))
            {
                int addedCustomerId = _customerRepo.GetLastIdentity();
                List<Tafsili> tafsilis = new List<Tafsili>
                {
                    new Tafsili
                    {
                        CustomerId = addedCustomerId,
                        SarfaslId = (short)SarfaslEnums.HesabDaryaftani,
                        PersonTypeId = 2,
                        TafsiliName = customer.OwnerName
                    },
                    new Tafsili
                    {
                        CustomerId = addedCustomerId,
                        SarfaslId = (short)SarfaslEnums.AsnadDaryftani,
                        PersonTypeId = 2,
                        TafsiliName = customer.OwnerName
                    },
                    new Tafsili
                    {
                        CustomerId = addedCustomerId,
                        SarfaslId = (short)SarfaslEnums.HesabPardakhtani,
                        PersonTypeId = 2,
                        TafsiliName = customer.OwnerName
                    }
                };

                if (!_tafsiliRepo.AddRange(tafsilis))
                {
                    return BadRequest("خطا در ثبت حساب های تفصیلی مشتری!");
                }

                if (string.IsNullOrWhiteSpace(request.ImageBase64))
                {
                    return Ok("مشتری جدید با موفقیت ثبت شد");
                }
                else
                {
                    if (ApiHelpers.SaveImage(EntityType.Customer, request.ImageBase64, imageName))
                    {
                        return Ok("مشتری جدید با موفقیت ثبت شد");
                    }
                    else
                    {
                        return Ok("مشتری جدید با موفقیت ثبت شد؛ اما تصویر به درستی ذخیره نشد.");
                    }
                }
            }
            else
            {
                return BadRequest("خطا در ثبت مشتری جدید!");
            }
        }

        [HttpPut]
        public IHttpActionResult Update([FromBody]CustomerUpdateRequest request)
        {
            if (!ModelState.IsValid)
            {
                var message = ModelState.Values.FirstOrDefault()?.Errors[0].ErrorMessage;
                return BadRequest(message);
            }

            Customer customer = Mapper.Map<CustomerUpdateRequest, Customer>(request);
            Customer dbCustomer = _customerRepo.Find(customer.Id);

            customer.CodeMelli = dbCustomer.CodeMelli;
            customer.CustomerCategoryId = dbCustomer.CustomerCategoryId;
            customer.MarketerId = dbCustomer.MarketerId;
            customer.Username = dbCustomer.Username;
            customer.Password = dbCustomer.Password;
            customer.SaltCode = dbCustomer.SaltCode;
            customer.IsActive = dbCustomer.IsActive;

            if (_customerRepo.Update(customer))
            {
                _customerRepo.UpadteTafsili(customer.Id, customer.OwnerName);

                if (string.IsNullOrWhiteSpace(request.ImageBase64)) // حذف عکس
                {
                    customer.ImageName = null;
                    ApiHelpers.DeleteImage(EntityType.Customer, dbCustomer.ImageName);
                }
                else if (string.Equals(request.ImageBase64.Trim(), "unchanged")) // عدم تغییر عکس
                {
                    customer.ImageName = dbCustomer.ImageName;
                }
                else // ذخیره عکس
                {
                    string imageName = "Customer" + "_" + Guid.NewGuid() + ".png";
                    bool imageSaved = ApiHelpers.SaveImage(EntityType.Customer, request.ImageBase64, imageName);

                    if (imageSaved)
                    {
                        ApiHelpers.DeleteImage(EntityType.Customer, dbCustomer.ImageName);
                        customer.ImageName = imageName;
                    }
                    else
                    {
                        return Ok("اطلاعات مشتری با موفقیت به روزرسانی شد؛ اما تصویر به درستی ذخیره نشد.");
                    }
                }

                if (_customerRepo.Update(customer)) // ثبت اسم تصویر در جدول
                {
                    return Ok("اطلاعات مشتری با موفقیت به روزرسانی شد.");
                }
                else
                {
                    return Ok("اطلاعات مشتری با موفقیت به روزرسانی شد؛ اما تصویر به درستی ذخیره نشد.");
                }
            }
            else
            {
                return BadRequest("خطا در ویرایش اطلاعات مشتری!");
            }
        }

        [HttpPut]
        public IHttpActionResult ChangePassword(ChangePasswordRequest request)
        {
            int customerId = 0;
            bool login = _customerRepo.Login(request.Username, request.OldPass, ref customerId);
            Customer customer = _customerRepo.Where(i => i.Id == customerId).FirstOrDefault();

            if (login == false || customer == null)
            {
                return BadRequest("نام کاربری یا کلمه عبور اشتباه است.");
            }

            var keyNew = Utilities.GeneratePassword(20);
            var password = Utilities.EncodePassword(request.NewPass, keyNew);
            customer.Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#");
            customer.SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#");

            if (_customerRepo.Update(customer))
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }

        //[HttpPost]
        //public IHttpActionResult PostImage(int id)
        //{
        //    try
        //    {
        //        HttpRequest httpRequest = HttpContext.Current.Request;

        //        if (httpRequest.Files.Count <= 0)
        //        {
        //            return BadRequest("فایل، به درستی، بارگزاری نشد!");
        //        }

        //        HttpPostedFile postedFile = httpRequest.Files[0];
        //        string imageName = "Customer" + "_" + TimeSpan.TicksPerSecond + Path.GetExtension(postedFile.FileName);
        //        if (postedFile.ContentLength > 0)
        //        {

        //            int MaxContentLength = 1024 * 1024 * 2; //Size = 2 MB  

        //            IList<string> allowedFileExtensions = new List<string> { ".jpg", ".gif", ".png" };
        //            string ext = postedFile.FileName.Substring(postedFile.FileName.LastIndexOf('.'));
        //            string extension = ext.ToLower();
        //            if (!allowedFileExtensions.Contains(extension))
        //            {
        //                return BadRequest("فایل تصویری باید در یکی از فرمت‌های .jpg یا .gif یا .png باشد!");
        //            }

        //            if (postedFile.ContentLength > MaxContentLength)
        //            {
        //                return BadRequest("حجم فایل، باید کمتر از 2 مگابایت باشد!");
        //            }

        //            string directoryVirtualPath = "~/Content/Upload/Image/Customers/";
        //            string directoryPhysicalPath = HttpContext.Current.Server.MapPath(directoryVirtualPath);
        //            //Directory.CreateDirectory(directoryPhysicalPath);


        //            string filePhysicalPath = HttpContext.Current.Server.MapPath(
        //                directoryVirtualPath + "/" + imageName);

        //            postedFile.SaveAs(filePhysicalPath);
        //        }

        //        var customer = _blCustomer.Find(id);
        //        customer.ImageName = imageName;
        //        _blCustomer.Update(customer);

        //        return Ok("تصویر با موفقیت بارگزاری شد.");
        //    }
        //    catch (Exception ex)
        //    {
        //        return InternalServerError(ex);
        //    }
        //}



        //[HttpDelete]
        //public IHttpActionResult Delete([FromUri]int id)
        //{
        //    Customer customer = _blCustomer.Find(id);
        //    if (customer == null)
        //    {
        //        return NotFound();
        //    }

        //    if (_blCustomer.Delete(id))
        //    {
        //        return Ok();
        //    }
        //    else
        //    {
        //        return BadRequest();
        //    }
        //}
    }
}
