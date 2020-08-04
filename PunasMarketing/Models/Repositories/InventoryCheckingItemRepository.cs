using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class InventoryCheckingItemRepository
    {
        private gsharing_DamliEntities _db;

        public InventoryCheckingItemRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(InventoryCheckingItem entity, bool autosave = true)
        {
            try
            {
                _db.InventoryCheckingItems.Add(entity);
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

        public bool AddRange(List<InventoryCheckingItem> items, short inventoryCheckingId, bool autosave = true)
        {
            try
            {
                foreach (var item in items)
                {
                    item.InventoryCheckingId = inventoryCheckingId;
                }

                _db.InventoryCheckingItems.AddRange(items);
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

        public bool DeleteRange(short inventoryCheckingId, bool autoSave = true)
        {
            try
            {
                IEnumerable<InventoryCheckingItem> inventoryCheckingItems =
                    _db.InventoryCheckingItems.Where(i => i.InventoryCheckingId == inventoryCheckingId);
                _db.InventoryCheckingItems.RemoveRange(inventoryCheckingItems);

                if (autoSave)
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

        public bool Update(InventoryCheckingItem entity)
        {
            try
            {
                _db.InventoryCheckingItems.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
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

        public IQueryable<InventoryCheckingItem> Select()
        {
            try
            {
                return _db.InventoryCheckingItems.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<InventoryCheckingItem> Where(Expression<Func<InventoryCheckingItem, bool>> predicate)
        {
            try
            {
                return _db.InventoryCheckingItems.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<InventoryCheckingItem, TResult>> selector)
        {
            try
            {
                return _db.InventoryCheckingItems.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public InventoryCheckingItem Find(int id)
        {
            try
            {
                return _db.InventoryCheckingItems.AsNoTracking().FirstOrDefault(i => i.Id == id);
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

        ~InventoryCheckingItemRepository()
        {
            Dispose(false);
        }
    }
}