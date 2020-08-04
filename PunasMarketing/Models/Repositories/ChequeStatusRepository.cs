using System;
using System.Collections.Generic;
using System.Linq;
using PunasMarketing.Models.EntityModel;

namespace PunasMarketing.Models.Repositories
{
    public class ChequeStatusRepository
    {
        private readonly List<ChequeStatusMetaData> _collection = new List<ChequeStatusMetaData>
        {
            // چک های دریافتی
            new ChequeStatusMetaData{Id = 1, Name = "دریافتی" },
            new ChequeStatusMetaData{Id = 2, Name = "به حساب خوابیده" },
            new ChequeStatusMetaData{Id = 3, Name = "وصول شده" },
            new ChequeStatusMetaData{Id = 4, Name = "عودت داده" },
            //new ChequeStatusMetaData{Id = 5, Name = "برگشت خورده" },
            new ChequeStatusMetaData{Id = 6, Name = "خرج شده" },
            //new ChequeStatusMetaData{Id = 7, Name = "پس داده شده" },

            // چک های پرداختی
            new ChequeStatusMetaData{Id = 11, Name = "پرداختی" },
            new ChequeStatusMetaData{Id = 12, Name = "وصول شده" },
            new ChequeStatusMetaData{Id = 13, Name = "عودت گرفته" },
            //new ChequeStatusMetaData{Id = 14, Name = "برگشت خورده" },
            //new ChequeStatusMetaData{Id = 15, Name = "پس داده شده" }
        };

        public IEnumerable<ChequeStatusMetaData> GetReceiveStatuses()
        {
            try
            {
                return _collection.Where(i => i.Id <= 10);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ChequeStatusMetaData> GetPayStatuses()
        {
            try
            {
                return _collection.Where(i => i.Id > 10);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<ChequeStatusMetaData> Select()
        {
            try
            {
                return _collection;
            }
            catch
            {
                return null;
            }
        }

        

        public IEnumerable<TResult> Select<TResult>(Func<ChequeStatusMetaData, TResult> selector)
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

        public IEnumerable<ChequeStatusMetaData> Where(Func<ChequeStatusMetaData, bool> predicate)
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

        public ChequeStatusMetaData Find(byte id)
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

        public IEnumerable<ChequeStatusMetaData> Find(string search)
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