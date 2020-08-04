using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class FactorItemRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public FactorItemRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(FactorItem entity, bool autoSave = true)
        {
            try
            {
                _db.FactorItems.Add(entity);
                if (autoSave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return false;
            }
        }

        public bool AddRange(List<FactorItem> factorItems, bool autoSave = true)
        {
            try
            {
                _db.FactorItems.AddRange(factorItems);
                if (autoSave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Update(FactorItem entity)
        {
            try
            {
                _db.FactorItems.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id, short periodId, out bool isUsed)
        {
            isUsed = false;

            try
            {
                var entity = Find(id, periodId);
                _db.Entry(entity).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public IQueryable<FactorItem> Select()
        {
            try
            {
                return _db.FactorItems.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<FactorItem> Where(Expression<Func<FactorItem, bool>> predicate)
        {
            try
            {
                return _db.FactorItems.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<FactorItem, TResult>> selector)
        {
            try
            {
                return _db.FactorItems.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public FactorItem Find(int id, short periodId)
        {
            try
            {
                return _db.FactorItems.SingleOrDefault(a => a.Id == id && a.PeriodId == periodId);
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

        ~FactorItemRepository()
        {
            Dispose(false);
        }
    }
}