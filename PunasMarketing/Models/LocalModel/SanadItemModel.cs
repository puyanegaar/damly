using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.LocalModel
{
    public class SanadItemModel
    {
        public short Sarfasl { get; set; }
        public int? Tafsili { get; set; }
        public string Description { get; set; }

        public decimal BedehAmount { get; set; }
        
        public decimal BestanAmount { get; set; }

        public int ItemId { get; set; }


        public string Code { get ; set; }

        public string SarfaslName { get; set; }

        public string TafsiliName { get; set; }

    }
}