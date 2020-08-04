using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class BankRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public BankRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Bank entity, bool autosave = true)
        {
            try
            {
                _db.Banks.Add(entity);
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

        public bool Update(Bank entity)
        {
            try
            {
                _db.Banks.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
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

        public IQueryable<Bank> Select()
        {
            try
            {
                return _db.Banks.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Bank> Where(Expression<Func<Bank, bool>> predicate)
        {
            try
            {
                return _db.Banks.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Bank, TResult>> selector)
        {
            try
            {
                return _db.Banks.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Bank Find(short id)
        {
            try
            {
                return _db.Banks.Find(id);
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

        ~BankRepository()
        {
            Dispose(false);
        }
    }
}