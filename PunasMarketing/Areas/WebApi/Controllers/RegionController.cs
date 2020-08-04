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
    public class RegionController : ApiController
    {
        private readonly RegionRepository _blRegion;

        public RegionController(RegionRepository blRegion)
        {
            _blRegion = blRegion;
        }


        [HttpGet]
        public IHttpActionResult Get()
        {
            var regions = _blRegion.Select();
            if (regions == null)
            {
                return NotFound();
            }

            List<RegionResponse> regionResponses = Mapper.Map<List<Region>, List<RegionResponse>>(regions.ToList());

            return Ok(regionResponses);
        }


        [HttpGet]
        public IHttpActionResult Get(int id)
        {
            var region = _blRegion.Find(id);

            if (region == null)
            {
                return NotFound();
            }

            RegionResponse regionResponse = Mapper.Map<Region, RegionResponse>(region);
            return Ok(regionResponse);
        }

        [HttpGet]
        public IHttpActionResult GetByCity(short id)
        {
            var regions = _blRegion.Where(i => i.CityId == id);
            if (regions == null)
            {
                return NotFound();
            }

            List<RegionResponse> regionResponses = Mapper.Map<List<Region>, List<RegionResponse>>(regions.ToList());

            return Ok(regionResponses);
        }

        [HttpPost]
        public IHttpActionResult Create([FromBody]RegionResponse regionResponse)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Region region = Mapper.Map<RegionResponse, Region>(regionResponse);

            if (_blRegion.Add(region))
            {
                return Ok("منطقه جدید با موفقیت ثبت شد");
            }
            else
            {
                return BadRequest("خطا در ثبت منطقه جدید!");
            }
        }


        [HttpPut]
        public IHttpActionResult Update([FromBody]RegionResponse regionResponse)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Region region = Mapper.Map<RegionResponse, Region>(regionResponse);

            if (_blRegion.Update(region))
            {
                return Ok("ویرایش موفق");
            }
            else
            {
                return BadRequest("خطا در ویرایش !");
            }
        }

        [HttpDelete]
        public IHttpActionResult Delete(int id)
        {
            Region region = _blRegion.Find(id);
            if (region == null)
            {
                return NotFound();
            }

            if (_blRegion.Delete(id, out bool isUsed))
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
