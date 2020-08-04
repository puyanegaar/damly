using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class WarehouseRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public WarehouseRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Warehouse entity, bool autosave = true)
        {
            try
            {
                _db.WareHouses.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(Warehouse entity)
        {
            try
            {
                _db.WareHouses.Attach(entity);
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

        public IQueryable<Warehouse> Select()
        {
            try
            {
                return _db.WareHouses.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Warehouse> Where(Expression<Func<Warehouse, bool>> predicate)
        {
            try
            {
                return _db.WareHouses.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public Warehouse Find(int id)
        {
            try
            {
                return _db.WareHouses.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Warehouse, TResult>> selector)
        {
            try
            {
                return _db.WareHouses.Select(selector);
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
                return _db.WareHouses.Max(i => i.Id);
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

        ~WarehouseRepository()
        {
            Dispose(false);
        }
    }
}