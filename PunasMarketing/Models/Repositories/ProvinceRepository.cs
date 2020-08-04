using System;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class ProvinceRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public ProvinceRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

      public IQueryable<Province> Select()
        {
            try
            {
                return _db.Provinces.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Province> Where(Expression<Func<Province, bool>> predicate)
        {
            try
            {
                return _db.Provinces.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public short GetProvinceId(short id)
        {
            return _db.Provinces.Where(a => a.Id == id).Select(a => a.Id).SingleOrDefault();
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        ~ProvinceRepository()
        {
            Dispose(false);
        }
    }
}