using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class InvoiceItemRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public InvoiceItemRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(InvoiceItem entity, bool autosave = true)
        {
            try
            {
                _db.InvoiceItems.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddRange(IEnumerable<InvoiceItem> invoiceItems, bool autoSave = true)
        {
            try
            {
                _db.InvoiceItems.AddRange(invoiceItems);
                if (autoSave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Update(InvoiceItem entity)
        {
            try
            {
                var invoiceItem = Find(entity.Id);
                invoiceItem.ProductId = entity.ProductId;
                invoiceItem.MainUnitCount = entity.MainUnitCount;
                invoiceItem.SubUnitCount = entity.SubUnitCount;
                invoiceItem.UnitsRate = entity.UnitsRate;
                invoiceItem.InvoiceId = entity.InvoiceId;
                _db.InvoiceItems.Attach(invoiceItem);
                _db.Entry(invoiceItem).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception e)
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

        public IQueryable<InvoiceItem> Select()
        {
            try
            {
                return _db.InvoiceItems.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<InvoiceItem> Where(Expression<Func<InvoiceItem, bool>> predicate)
        {
            try
            {
                return _db.InvoiceItems.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<InvoiceItem, TResult>> selector)
        {
            try
            {
                return _db.InvoiceItems.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public InvoiceItem Find(int id)
        {
            try
            {
                return _db.InvoiceItems.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<InvoiceItem> GetInvoiceItems(int id)
        {
            return _db.InvoiceItems.Where(a => a.InvoiceId == id);
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

        ~InvoiceItemRepository()
        {
            Dispose(false);
        }
    }
}