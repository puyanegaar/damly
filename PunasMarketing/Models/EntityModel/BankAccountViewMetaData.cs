using PunasMarketing.Models.EntityModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.EntityModel
{
    internal class BankAccountViewMetaData
    {

    }
}

namespace PunasMarketing.Models.DomainModel
{
    [MetadataType(typeof(BankAccountViewMetaData))]
    public partial class BankAccountView
    {

        public string HesabCompeletName
        {
            get { return Name + "/" + AccountNum + "/" + Owner; }
        }
    }
}