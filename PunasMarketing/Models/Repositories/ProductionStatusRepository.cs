using System;
using System.Collections.Generic;
using System.Linq;

namespace PunasMarketing.Models.Repositories
{
    public class ProductionStatusRepository
    {
        private readonly Dictionary<byte, string> _collection = new Dictionary<byte, string>
        {
            {0, "در حال تولید" },
            {1, "تولید متوقف شده" },
            {2, "ناموجود" },
            {3, "موجود" }
        };

        public IEnumerable<KeyValuePair<byte, string>> Select()
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

        public IEnumerable<TResult> Select<TResult>(Func<KeyValuePair<byte, string>, TResult> selector)
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

        public IEnumerable<KeyValuePair<byte, string>> Where(Func<KeyValuePair<byte, string>, bool> predicate)
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

        public KeyValuePair<byte, string> Find(byte id)
        {
            try
            {
                return _collection.FirstOrDefault(i => i.Key == id);
            }
            catch
            {
                return new KeyValuePair<byte, string>(0, string.Empty);
            }
        }

        public IEnumerable<KeyValuePair<byte, string>> Find(string search)
        {
            try
            {
                return _collection.Where(i => i.Value.Contains(search));
            }
            catch
            {
                return null;
            }
        }

    }
}