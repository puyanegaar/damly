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
    public class ProvinceController : ApiController
    {
        private readonly ProvinceRepository _blProvince;
        
        public ProvinceController(ProvinceRepository blProvince)
        {
            _blProvince = blProvince;
        }


        [HttpGet]
        public IHttpActionResult Get()
        {
            var provinces = _blProvince.Select();
            if (provinces == null)
            {
                return NotFound();
            }

            List<ProvinceResponse> provinceResponses = Mapper.Map<List<Province>, List<ProvinceResponse>>(provinces.ToList());

            return Ok(provinceResponses);
        }

    }
}
