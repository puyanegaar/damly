using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class PriceTypeRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public PriceTypeRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(PriceType entity, bool autosave = true)
        {
            try
            {
                _db.PriceTypes.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
        }
        public bool Update(PriceType entity)
        {
            try
            {
                _db.PriceTypes.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id, out bool isUsed)
        {
            isUsed = false;

            try
            {
                var entity = Find(id);
                _db.Entry(entity).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public IQueryable<PriceType> Select()
        {
            try
            {
                return _db.PriceTypes.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<PriceType> Where(Expression<Func<PriceType, bool>> predicate)
        {
            try
            {
                return _db.PriceTypes.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<PriceType, TResult>> selector)
        {
            try
            {
                return _db.PriceTypes.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public PriceType Find(int id)
        {
            try
            {
                return _db.PriceTypes.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public short GetMaxid()
        {
            try
            {
                return _db.PriceTypes.Max(i => i.Id);
            }
            catch
            {
                return 0;
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
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        ~PriceTypeRepository()
        {
            Dispose(false);
        }
    }
}