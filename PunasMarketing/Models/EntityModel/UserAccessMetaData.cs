using System.ComponentModel.DataAnnotations;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.EntityModel
{
    public class UserAccessMetaData
    {
        [Display(Name = "شناسه سند")]
        public int Id { get; set; }

        [Display(Name = "کاربر")]
        public int UserId { get; set; }

        [Display(Name = "عملیات")]
        public int ActionId { get; set; }
    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(UserAccessMetaData))]
    public partial class UserAccess
    {

    }
}