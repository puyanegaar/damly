using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class ChangePasswordRequest
    {
        public string Username { get; set; }
        public string OldPass { get; set; }
        public string NewPass { get; set; }
    }
}