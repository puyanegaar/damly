using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class SupplierRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public SupplierRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Supplier entity, bool autosave = true)
        {
            try
            {
                _db.Suppliers.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Update(Supplier entity)
        {
            try
            {
                _db.Suppliers.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateTafsili(short supplierId, string name)
        {
            try
            {
                var result = _db.UpdateSupplierTafsili(supplierId, name);
                return Convert.ToBoolean(result);
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

        public IQueryable<Supplier> Select()
        {
            try
            {
                return _db.Suppliers.AsQueryable();
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<Supplier> Where(Expression<Func<Supplier, bool>> predicate)
        {
            try
            {
                return _db.Suppliers.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Supplier, TResult>> selector)
        {
            try
            {
                return _db.Suppliers.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Supplier Find(int id)
        {
            try
            {
                return _db.Suppliers.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public List<SupplierReport_Result> GetFinancialReport(short fiscalId, int supplierId)
        {
            try
            {
                return _db.SupplierReport(fiscalId, supplierId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public short GetMaxid()
        {
            try
            {
                return _db.Suppliers.Max(i => i.Id);
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

        ~SupplierRepository()
        {
            Dispose(false);
        }
    }
}