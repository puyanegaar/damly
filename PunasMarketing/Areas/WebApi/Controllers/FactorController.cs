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
    public class FactorController : ApiController
    {
        private readonly FactorRepository _factorRepo;
        private readonly FactorItemRepository _factorItemRepo;
        private readonly ProductRepository _productRepository;

        public FactorController(
            FactorRepository factorRepo,
            FactorItemRepository factorItemRepo,
            ProductRepository productRepository)
        {
            _factorRepo = factorRepo;
            _factorItemRepo = factorItemRepo;
            _productRepository = productRepository;
        }


        [HttpGet]
        public IHttpActionResult GetByCustomerId(int id)
        {
            var factors = _factorRepo.Where(i => i.CustomerId == id);
            if (factors == null)
            {
                return NotFound();
            }

            List<FactorResponse> factorResponses = Mapper.Map<List<Factor>, List<FactorResponse>>(factors.ToList());

            foreach (var factor in factorResponses)
            {
                factor.IsSent = factors.First(i => i.Id == factor.Id).InvoiceId.HasValue;

                var factorItems = _factorItemRepo.Where(i =>
                    i.FactorId == factor.Id && i.PeriodId == factor.PeriodId);

                List<FactorItemResponse> factorItemResponses =
                    Mapper.Map<IQueryable<FactorItem>, List<FactorItemResponse>>(factorItems);

                foreach (var factorItemResponse in factorItemResponses)
                {
                    factorItemResponse.ProductName = _productRepository.Find(factorItemResponse.ProductId).Name;
                }

                if (factorItemResponses.Any())
                {
                    factorResponses.First(i => i.Id == factor.Id).Items = factorItemResponses;
                }
                else
                {
                    factorResponses.RemoveAll(i => i.Id == factor.Id);
                }
            }

            return Ok(factorResponses);
        }

        [HttpPost]
        public IHttpActionResult Confirm(int id, short periodId)
        {
            return SetConfirm(id, periodId, true);
        }

        [HttpPost]
        public IHttpActionResult UnConfirm(int id, short periodId)
        {
            return SetConfirm(id, periodId, false);
        }

        private IHttpActionResult SetConfirm(int id, short periodId, bool isConfirmed)
        {
            var factor = _factorRepo.Find(id, periodId);
            if (factor == null)
            {
                return NotFound();
            }

            factor.IsConfirmed = isConfirmed;

            if (_factorRepo.Update(factor))
            {
                return Ok("عملیات با موفقیت انجام شد.");
            }
            else
            {
                return InternalServerError();
            }
        }
    }
}
