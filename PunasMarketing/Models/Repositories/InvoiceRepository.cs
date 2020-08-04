using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class InvoiceRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public InvoiceRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public int Add(Invoice entity, bool autosave = true)
        {
            _db.Invoices.Add(entity);
            _db.SaveChanges();
            return entity.Id;
        }

        public bool Update(Invoice entity)
        {
            try
            {
                var invoice = Find(entity.Id);
                invoice.FactorNum = entity.FactorNum;
                invoice.ThisWareHouseId = entity.ThisWareHouseId;
                invoice.PersonnelId = entity.PersonnelId;
                invoice.Description = entity.Description;

                _db.Invoices.Attach(invoice);
                _db.Entry(invoice).State = EntityState.Modified;
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

        public IQueryable<Invoice> Select()
        {
            try
            {
                return _db.Invoices.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Invoice> Where(Expression<Func<Invoice, bool>> predicate)
        {
            try
            {
                return _db.Invoices.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Invoice, TResult>> selector)
        {
            try
            {
                return _db.Invoices.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Invoice Find(int id)
        {
            try
            {
                return _db.Invoices.Find(id);
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

        public bool SetComplete(int invoiceId)
        {
            try
            {
                var invoice = Find(invoiceId);
                var invoiceItems = _db.InvoiceItems.Where(a => a.InvoiceId == invoiceId);

                //ChangeInventory
                foreach (var item in invoiceItems)
                {
                    Product product = _db.Products.Find(item.ProductId);

                    if (product != null)
                    {
                        if (invoice.FactorNum != null)
                        {
                            int FactorId = int.Parse(invoice.FactorNum);
                            var factorItem = _db.FactorItems.Where(x => x.FactorId == FactorId && x.ProductId==item.ProductId).FirstOrDefault();
                                int unitId = (int)factorItem.UnitId;
                                bool isMain = _db.Units.Where(x => x.Id == unitId).FirstOrDefault().IsMain;
                                if (!isMain)
                                {
                                    if (invoice.IsReceive)
                                    {
                                        double inventoruChange = (double)factorItem.UnitsRate * (double)factorItem.MainUnitCount;
                                        product.Inventory += inventoruChange;
                                    }
                                    else
                                    {
                                        double inventoruChange = (double)factorItem.UnitsRate * (double)factorItem.MainUnitCount;
                                        product.Inventory -= inventoruChange;
                                    }
                                    
                                }
                                else if(invoice.FactorNum == null || isMain)
                                {
                                    if (invoice.IsReceive)
                                    {
                                        product.Inventory += (int)item.MainUnitCount;
                                    }
                                    else
                                    {
                                        product.Inventory -= (int)item.MainUnitCount;
                                    }
                                }
                        }
                    }
                }

                invoice.IsCompleted = true;

                Save();

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
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

        ~InvoiceRepository()
        {
            Dispose(false);
        }
    }
}