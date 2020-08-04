using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class UnitRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public UnitRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public UnitRepository()
        {
        }

        public bool Add(Unit entity, bool autosave = true)
        {
            try
            {
                _db.Units.Add(entity);
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
        public bool Update(Unit entity)
        {
            try
            {
                _db.Units.Attach(entity);
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

        public IQueryable<Unit> Select()
        {
            try
            {
                return _db.Units.AsQueryable();
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<Unit> Where(Expression<Func<Unit, bool>> predicate)
        {
            try
            {
                return _db.Units.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Unit, TResult>> selector)
        {
            try
            {
                return _db.Units.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Unit Find(int id)
        {
            try
            {
                return _db.Units.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public int GetMaxid()
        {
            try
            {
                return _db.Units.Max(i => i.Id);
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
                if (this._db != null)
                {
                    this._db.Dispose();
                    this._db = null;
                }
            }
        }

        ~UnitRepository()
        {
            Dispose(false);
        }
    }
}