using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;
using Receipt = PunasMarketing.Models.DomainModel.Receipt;

namespace PunasMarketing.Models.Repositories
{
    public class ReceiptRepository
    {
        private readonly gsharing_DamliEntities _db;

        public ReceiptRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Receipt entity, bool autosave = true)
        {
            try
            {
                _db.Receipts.Add(entity);
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

        public bool Update(Receipt entity)
        {
            try
            {
                _db.Receipts.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var entity = Find(id);
                _db.Entry(entity).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<Receipt> Select()
        {
            try
            {
                return _db.Receipts.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Receipt> Where(Expression<Func<Receipt, bool>> predicate)
        {
            try
            {
                return _db.Receipts.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Receipt, TResult>> selector)
        {
            try
            {
                return _db.Receipts.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Receipt Find(int id)
        {
            try
            {
                return _db.Receipts.Find(id);
            }
            catch
            {
                return null;
            }
        }
        public int GetLastIdentity()
        {
            try
            {
                if (_db.Receipts.Any())
                    return _db.Receipts.OrderByDescending(p => p.Id).First().Id;
                else
                    return 0;
            }
            catch
            {
                return -1;
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

        ~ReceiptRepository()
        {
            Dispose(false);
        }
    }
}