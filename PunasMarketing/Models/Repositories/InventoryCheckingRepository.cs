using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class InventoryCheckingRepository
    {
        private gsharing_DamliEntities _db;

        public InventoryCheckingRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(InventoryChecking entity, bool autosave = true)
        {
            try
            {
                _db.InventoryCheckings.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Update(InventoryChecking entity)
        {
            try
            {
                _db.InventoryCheckings.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                //InventoryChecking inventoryChecking = _db.InventoryCheckings.Find(entity.Id);
                //if (inventoryChecking != null)
                //{
                //    inventoryChecking.CreatedByUserId = entity.CreatedByUserId;
                //    inventoryChecking.CreatedDate = entity.CreatedDate;
                //    inventoryChecking.Description = entity.Description;
                //    inventoryChecking.WarehouseId = entity.WarehouseId;
                //}

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(short id, out bool isUsed)
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

        public IQueryable<InventoryChecking> Select()
        {
            try
            {
                return _db.InventoryCheckings.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<InventoryChecking> Where(Expression<Func<InventoryChecking, bool>> predicate)
        {
            try
            {
                return _db.InventoryCheckings.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<InventoryChecking, TResult>> selector)
        {
            try
            {
                return _db.InventoryCheckings.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public short GetLastId()
        {
            try
            {
                return _db.InventoryCheckings.Max(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        public InventoryChecking Find(short id)
        {
            try
            {
                return _db.InventoryCheckings.AsNoTracking().FirstOrDefault(i => i.Id == id);
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

        ~InventoryCheckingRepository()
        {
            Dispose(false);
        }
    }
}