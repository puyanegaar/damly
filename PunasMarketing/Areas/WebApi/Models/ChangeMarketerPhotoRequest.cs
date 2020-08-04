using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class ChangeMarketerPhotoRequest
    {
        public int MarketerId { get; set; }
        public string ImageBase64 { get; set; }
    }
}