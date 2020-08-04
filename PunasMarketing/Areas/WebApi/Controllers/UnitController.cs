using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using AutoMapper;
using PunasMarketing.Areas.WebApi.Models;
using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Repositories;

namespace PunasMarketing.Areas.WebApi.Controllers
{
    //[Authorize]
    public class UnitController : ApiController
    {
        private readonly UnitRepository _blUnit;

        public UnitController(UnitRepository blUnit)
        {
            _blUnit = blUnit;
        }

        
        //[HttpGet]
        //public IHttpActionResult GetAllUnits()
        //{
        //    var units = _blUnit.Where(a => a.IsMain == true);
        //    if (!units.Any())
        //    {
        //        return NotFound();
        //    }

        //    List<UnitResponse> unitResponse = Mapper.Map<List<Unit>, List<UnitResponse>>(units.ToList());
        //    return Ok(unitResponse);
        //}
    }
}
