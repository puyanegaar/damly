using System;
using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.Repositories;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Http;

namespace PunasMarketing.Areas.WebApi.Controllers
{
    //[Authorize]
    public class MarketerController : ApiController
    {
        private readonly PersonnelRepository _personnelRepo;
        private readonly CustomerRepository _customerRepo;
        private readonly TafsiliRepository _tafsiliRepo;
        private readonly SanadItemRepository _sanadItemRepo;
        private readonly UserRepository _userRepo;

        public MarketerController(
            PersonnelRepository personnelRepo,
            CustomerRepository customerRepo,
            TafsiliRepository tafsiliRepo,
            SanadItemRepository sanadItemRepo,
            UserRepository userRepo)
        {
            _personnelRepo = personnelRepo;
            _customerRepo = customerRepo;
            _tafsiliRepo = tafsiliRepo;
            _sanadItemRepo = sanadItemRepo;
            _userRepo = userRepo;
        }

        [HttpGet]
        public IHttpActionResult Get(string username)
        {
            var user = _userRepo.FindByUsername(username);
            if (user == null)
            {
                return NotFound();
            }

            var personnelId = user.PersonnelId;

            var marketer = _personnelRepo.Find(personnelId);

            if (marketer == null)
            {
                return NotFound();
            }

            if (marketer.JobTitleId != 0) // عنوان شغلی، بازاریاب نیست
            {
                return NotFound();
            }

            MarketerResponse marketerResponse = Mapper.Map<Personnel, MarketerResponse>(marketer);

            var virtualPath = ApiHelpers.GetImageVirtualPath(EntityType.Marketer, marketerResponse.ImageName);
            var physicalPath = HttpContext.Current.Server.MapPath(virtualPath);

            string imageUrl = File.Exists(physicalPath) ? Url.Content(virtualPath) : null;
            marketerResponse.ImageName = imageUrl;

            return Ok(marketerResponse);
        }

        [HttpGet]
        public IHttpActionResult GetSanadItems(int id)
        {
            var marketer = _personnelRepo.Find(id);

            if (marketer == null)
            {
                return NotFound();
            }

            var marketerTafsilis = _tafsiliRepo.Where(i =>
                i.PersonnelId == id &&
                (i.SarfaslId == (short)SarfaslEnums.HesabPardakhtani ||
                 i.SarfaslId == (short)SarfaslEnums.HesabDaryaftani)).ToList();

            var firstTafsiliId = marketerTafsilis[0].Id;
            var secondTafsiliId = marketerTafsilis[1].Id;

            List<SanadItemResponse> sanadItemResponses = new List<SanadItemResponse>();

            if (marketerTafsilis.Count == 2)
            {
                List<SanadItem> sanadItems = _sanadItemRepo.Where(i => i.TafsiliId == firstTafsiliId || i.TafsiliId == secondTafsiliId).ToList();
                sanadItemResponses = Mapper.Map<IEnumerable<SanadItem>, List<SanadItemResponse>>(sanadItems);
            }

            return Ok(sanadItemResponses);
        }

        [HttpGet]
        public IHttpActionResult GetRegisteredCustomers(int id)
        {
            var marketer = _personnelRepo.Find(id);

            if (marketer == null)
            {
                return NotFound();
            }

            IEnumerable<Customer> registeredCustomers = _customerRepo.Where(i => i.MarketerId == id);
            List<CustomerResponse> customerResponses = Mapper.Map<IEnumerable<Customer>, List<CustomerResponse>>(registeredCustomers);

            return Ok(customerResponses);
        }

        [HttpPut]
        public IHttpActionResult ChangePassword(ChangePasswordRequest request)
        {
            User user = new User();
            bool login = _personnelRepo.Login(request.Username, request.OldPass, ref user);
           
            if (login == false || user == null)
            {
                return BadRequest("نام کاربری یا کلمه عبور اشتباه است.");
            }

            var keyNew = Utilities.GeneratePassword(20);
            var password = Utilities.EncodePassword(request.NewPass, keyNew);
            user.Password = password.EncryptString("#+Samoosh!Group@+Generate$pas+#");
            user.SaltCode = keyNew.EncryptString("#+Samoosh!Group@+Generate$key+#");
            user.RepeatPassword = "foofoo";

            if (_userRepo.Update(user))
            {
                return Ok();
            }
            else
            {
                return InternalServerError();
            }
        }

        [HttpPut]
        public IHttpActionResult ChangePhoto(ChangeMarketerPhotoRequest request)
        {
            Personnel dbMarketer = _personnelRepo.Find(request.MarketerId);

            if (string.IsNullOrWhiteSpace(request.ImageBase64)) // حذف عکس
            {
                dbMarketer.ImageName = null;
                ApiHelpers.DeleteImage(EntityType.Marketer, dbMarketer.ImageName);
            }
            else // ذخیره عکس
            {
                string imageName = "Marketer" + "_" + Guid.NewGuid() + ".png";
                bool imageSaved = ApiHelpers.SaveImage(EntityType.Marketer, request.ImageBase64, imageName);

                if (imageSaved)
                {
                    ApiHelpers.DeleteImage(EntityType.Marketer, dbMarketer.ImageName);
                    dbMarketer.ImageName = imageName;
                }
                else
                {
                    return BadRequest("خطا در ذخیره تصویر!");
                }
            }

            if (_personnelRepo.Update(dbMarketer)) // ثبت اسم تصویر در جدول
            {
                return Ok("تصویر جدید با موفقیت ذخیره شد.");
            }
            else
            {
                return InternalServerError();
            }
        }
    }
}
