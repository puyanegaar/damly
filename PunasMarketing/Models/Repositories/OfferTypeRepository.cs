using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.Repositories
{
    public class OfferTypeRepository
    {
        private readonly List<OfferTypeMetaData> _collection = new List<OfferTypeMetaData>
        {
            new OfferTypeMetaData{Id = 1, Name = "تخفیف ریالی" },
            new OfferTypeMetaData{Id = 2, Name = "تخفیف درصدی" },
            new OfferTypeMetaData{Id = 3, Name = "تخفیف کالایی" },
        };

        public List<OfferTypeMetaData> Select()
        {
            try
            {
                return _collection.ToList();
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<TResult> Select<TResult>(Func<OfferTypeMetaData, TResult> selector)
        {
            try
            {
                return _collection.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<OfferTypeMetaData> Where(Func<OfferTypeMetaData, bool> predicate)
        {
            try
            {
                return _collection.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public OfferTypeMetaData Find(byte id)
        {
            try
            {
                return _collection.FirstOrDefault(i => i.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<OfferTypeMetaData> Find(string search)
        {
            try
            {
                return _collection.Where(i => i.Name.Contains(search));
            }
            catch
            {
                return null;
            }
        }
    }
}