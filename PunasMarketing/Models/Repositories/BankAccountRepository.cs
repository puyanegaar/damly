using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class BankAccountRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public BankAccountRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(BankAccount entity, bool autosave = true)
        {
            try
            {
                _db.BankAccounts.Add(entity);
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
        public bool Update(BankAccount entity, bool autosave = true)
        {
            try
            {
                _db.BankAccounts.Attach(entity);
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
                return false;
            }
        }

        public bool UpdateTafsili(short bankAccId, string name)
        {
            try
            {
                var result = _db.UpdateBankAccountTafsili(bankAccId, name);
                return Convert.ToBoolean(result);
            }
            catch(Exception ex)
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

        public IQueryable<BankAccount> Select()
        {
            try
            {
                return _db.BankAccounts.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<BankAccountView> SelectView()
        {
            try
            {
                return _db.BankAccountViews.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> SelectView<TResult>(Expression<Func<BankAccountView, TResult>> selector)
        {
            try
            {
                return _db.BankAccountViews.Select(selector);
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<BankAccount> Where(Expression<Func<BankAccount, bool>> predicate)
        {
            try
            {
                return _db.BankAccounts.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<BankAccount, TResult>> selector)
        {
            try
            {
                return _db.BankAccounts.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public BankAccount Find(short id)
        {
            try
            {
                return _db.BankAccounts.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public void WithDraw(short id, decimal amount, bool autosave = true)
        {
            BankAccount account = Find(id);
            Update(account, false);
            if (autosave)
            {
                _db.SaveChanges();
            }

        }

        public short GetMaxId()
        {
            try
            {
                return _db.BankAccounts.Max(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        public void Deposit(short id, decimal amount, bool autosave = true)
        {

            BankAccount account = Find(id);
            Update(account, false);
            if (autosave)
            {
                _db.SaveChanges();
            }

        }

        //public bool IsSufficient(short id, decimal amount)
        //{
        //    try
        //    {
        //        return _db.BankAccounts.Any(a => a.Id == id && a.CurrentBalance >= amount);
        //    }
        //    catch (Exception e)
        //    {
        //        return false;
        //    }
        //}

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

        ~BankAccountRepository()
        {
            Dispose(false);
        }
    }
}