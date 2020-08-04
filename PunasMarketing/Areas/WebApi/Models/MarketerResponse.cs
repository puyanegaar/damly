using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class MarketerResponse
    {
        public int Id { get; set; }
        public string ImageName { get; set; }
        public string PersonnelCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CodeMelli { get; set; }
        public string Mobile1 { get; set; }
        public string HomeAddress { get; set; }
        public string Region { get; set; }
        public string Email { get; set; }
        public DateTime HireDate { get; set; }
        public string BankName { get; set; }
        public string BankCardNum { get; set; }
        public string Iban { get; set; }
    }
}