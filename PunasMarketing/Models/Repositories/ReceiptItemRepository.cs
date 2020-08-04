using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class ReceiptItemRepository
    {
        private readonly gsharing_DamliEntities _db;

        public ReceiptItemRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(int ReceiptId,int? TransactionId,int? ChequeId, bool autosave = true)
        {
            try
            {
                ReceiptItem entity = new ReceiptItem();
                entity.ReceiptId = ReceiptId;
                entity.TransactionId = TransactionId;
                entity.ChequeId = ChequeId;
                _db.ReceiptItems.Add(entity);
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

        public bool Update(ReceiptItem entity)
        {
            try
            {
                _db.ReceiptItems.Attach(entity);
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
            catch(Exception ex)
            {
                return false;
            }
        }

        public IQueryable<ReceiptItem> Select()
        {
            try
            {
                return _db.ReceiptItems.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<ReceiptItem> Where(Expression<Func<ReceiptItem, bool>> predicate)
        {
            try
            {
                return _db.ReceiptItems.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<ReceiptItem, TResult>> selector)
        {
            try
            {
                return _db.ReceiptItems.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public ReceiptItem Find(int id)
        {
            try
            {
                return _db.ReceiptItems.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public ReceiptItem FindByChequeId(int chequeId)
        {
            try
            {
                return _db.ReceiptItems.FirstOrDefault(i => i.ChequeId == chequeId);
            }
            catch
            {
                return null;
            }
        }

        public ReceiptItem FindByTransactionId(int transactionId)
        {
            try
            {
                return _db.ReceiptItems.FirstOrDefault(i => i.TransactionId == transactionId);
            }
            catch
            {
                return null;
            }
        }

        public bool Save()
        {
            try
            {
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
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

        ~ReceiptItemRepository()
        {
            Dispose(false);
        }
    }
}