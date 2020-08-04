using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class TransactionRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public TransactionRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(decimal money, decimal? Commission,short? CashDeskId,short? BankAccountId,string IssueTracking,string Description,byte TransactionType, bool autosave = true)
        {
            try
            {
                Transaction entity = new Transaction();
                entity.Amount = money;
                entity.Commission = Commission;
                entity.CashDeskId = CashDeskId;
                entity.BankAccountId = BankAccountId;
                entity.IssueTracking = IssueTracking;
                entity.Description = Description;
                entity.TransactionType = TransactionType;
                _db.Transactions.Add(entity);
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
        public bool Update(Transaction entity, bool autosave = true)
        {
            try
            {
                _db.Transactions.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return true;
            }
        }

        public bool Delete(short transactionId)
        {
            try
            {
                Transaction transaction = Find(transactionId);
                //transaction.IsDelete = true;
                //return Convert.ToBoolean(_db.SaveChanges());
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<Transaction> Select()
        {
            try
            {
                //return _db.Transactions.Where(a => a.IsDelete == false).OrderByDescending(a => a.SubmitDate).AsQueryable();
                return null;
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Transaction> Where(Expression<Func<Transaction, bool>> predicate)
        {
            try
            {
                return _db.Transactions.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Transaction, TResult>> selector)
        {
            try
            {
                return _db.Transactions.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Transaction Find(short id)
        {
            try
            {
                //return _db.Transactions.SingleOrDefault(a => a.IsDelete == false && a.Id == id);
                return null;
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
                if (_db.Transactions.Any())
                    return _db.Transactions.OrderByDescending(p => p.Id).First().Id;
                else
                    return 0;
            }
            catch
            {
                return -1;
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
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        ~TransactionRepository()
        {
            Dispose(false);
        }
    }
}