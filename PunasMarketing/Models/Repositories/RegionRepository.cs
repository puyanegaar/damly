using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using Region = PunasMarketing.Models.DomainModel.Region;

namespace PunasMarketing.Models.Repositories
{
    public class RegionRepository
    {
        private readonly gsharing_DamliEntities _db;

        public RegionRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Region entity, bool autosave = true)
        {
            try
            {
                _db.Regions.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Region entity)
        {
            try
            {
                _db.Regions.Attach(entity);
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

        public IQueryable<Region> Select()
        {
            try
            {
                return _db.Regions.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Region> Where(Expression<Func<Region, bool>> predicate)
        {
            try
            {
                return _db.Regions.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Region, TResult>> selector)
        {
            try
            {
                return _db.Regions.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Region Find(int id)
        {
            try
            {
                return _db.Regions.Find(id);
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

        ~RegionRepository()
        {
            Dispose(false);
        }
    }
}