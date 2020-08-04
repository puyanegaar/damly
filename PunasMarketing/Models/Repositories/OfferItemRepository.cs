using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class OfferItemRepository
    {
        private readonly gsharing_DamliEntities _db;

        public OfferItemRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(OfferItem entity, bool autosave = true)
        {
            try
            {
                _db.OfferItems.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Update(OfferItem entity)
        {
            try
            {
                _db.OfferItems.Attach(entity);
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

        public bool DeleteByOfferId(short id)
        {
            try
            {
                var items = _db.OfferItems.Where(a => a.OfferId == id).ToList();
                if (items.Any())
                {
                    foreach (var i in items)
                    {
                        _db.OfferItems.Remove(i);
                    }
                }
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public IQueryable<OfferItem> Select()
        {
            try
            {
                return _db.OfferItems.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<OfferItem> Where(Expression<Func<OfferItem, bool>> predicate)
        {
            try
            {
                return _db.OfferItems.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<OfferItem, TResult>> selector)
        {
            try
            {
                return _db.OfferItems.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public OfferItem Find(int id)
        {
            try
            {
                return _db.OfferItems.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<OfferItem> GetOfferItemByOfferId(short id)
        {
            return _db.OfferItems.Where(a => a.OfferId == id);
        }

        public int GetMaxid()
        {
            try
            {
                return _db.OfferItems.Max(i => i.Id);
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
                _db?.Dispose();
            }
        }

        ~OfferItemRepository()
        {
            Dispose(false);
        }
    }
}