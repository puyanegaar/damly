using System.ComponentModel.DataAnnotations;

namespace PunasMarketing.Areas.WebApi.Models
{
    public class UnitResponse
    {
        public short Id { get; set; }

        [Display(Name = "نام واحد اندازه گیری")]
        public string Name { get; set; }
    }
}