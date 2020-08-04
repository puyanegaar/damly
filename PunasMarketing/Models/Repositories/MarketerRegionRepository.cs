using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class MarketerRegionRepository
    {
        private gsharing_DamliEntities _db;

        public MarketerRegionRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(MarketerRegion entity, bool autoSave = true)
        {
            try
            {
                _db.MarketerRegions.Add(entity);
                if (autoSave)
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

        public bool AddRange(List<MarketerRegion> marketerRegions, bool autoSave = true)
        {
            try
            {
                _db.MarketerRegions.AddRange(marketerRegions);
                if (autoSave)
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

        public bool Update(MarketerRegion entity)
        {
            try
            {
                _db.MarketerRegions.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int marketerRegionId)
        {
            try
            {
                MarketerRegion marketerRegion = Find(marketerRegionId);
                _db.Entry(marketerRegion).State = EntityState.Deleted;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool DeleteRange(int marketerId)
        {
            try
            {
                var marketerRegions = _db.MarketerRegions.Where(i => i.MarketerId == marketerId);

                foreach (var marketerRegion in marketerRegions)
                {
                    _db.Entry(marketerRegion).State = EntityState.Deleted;
                }

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public IQueryable<MarketerRegion> Select()
        {
            try
            {
                return _db.MarketerRegions.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<MarketerRegion> Where(Expression<Func<MarketerRegion, bool>> predicate)
        {
            try
            {
                return _db.MarketerRegions.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<MarketerRegion, TResult>> selector)
        {
            try
            {
                return _db.MarketerRegions.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public MarketerRegion Find(int id)
        {
            try
            {
                return _db.MarketerRegions.Find(id);
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
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        ~MarketerRegionRepository()
        {
            Dispose(false);
        }
    }
}