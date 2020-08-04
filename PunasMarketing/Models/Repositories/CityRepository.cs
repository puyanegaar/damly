using System;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class CityRepository:IDisposable
    {
        private readonly gsharing_DamliEntities _db;

        public CityRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public IQueryable<City> Select()
        {
            try
            {
                return _db.Cities.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<City> Where(Expression<Func<City, bool>> predicate)
        {
            try
            {
                return _db.Cities.Where(predicate);
            }
            catch
            {
                return null;
            }
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
                _db?.Dispose();
            }
        }

        ~CityRepository()
        {
            Dispose(false);
        }
    }
}