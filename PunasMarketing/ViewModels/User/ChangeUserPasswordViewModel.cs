using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.ViewModels.User
{
    public class ChangeUserPasswordViewModel
    {
        public int PersonnelId { get; set; }
        public string NewPassword { get; set; }
    }
}