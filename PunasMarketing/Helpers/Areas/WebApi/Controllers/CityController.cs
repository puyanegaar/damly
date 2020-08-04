using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace PunasMarketing.Areas.WebApi.Controllers
{

    public class CityController : ApiController
    {
        private readonly CityRepository _blCity;


        public CityController(CityRepository blCity)
        {
            _blCity = blCity;
        }


        [HttpGet]
        public IHttpActionResult Get()
        {
            var cities = _blCity.Select();
            if (cities == null)
            {
                return NotFound();
            }

            List<CityResponse> cityResponses = Mapper.Map<List<City>, List<CityResponse>>(cities.ToList());

            return Ok(cityResponses);
        }

    }
}
