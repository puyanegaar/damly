using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class CashDeskRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public CashDeskRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(CashDesk entity, bool autosave = true)
        {
            try
            {
                _db.CashDesks.Add(entity);
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
        public bool Update(CashDesk entity)
        {
            try
            {
                _db.CashDesks.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }
        public bool UpdateTafsili(short chashDeskId, string name)
        {
            try
            {
                var result = _db.UpdateCashDeskTafsili(chashDeskId, name);
                return Convert.ToBoolean(result);
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

        public IQueryable<CashDesk> Select()
        {
            try
            {
                return _db.CashDesks.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<CashDesk> Where(Expression<Func<CashDesk, bool>> predicate)
        {
            try
            {
                return _db.CashDesks.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<CashDesk, TResult>> selector)
        {
            try
            {
                return _db.CashDesks.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public CashDesk Find(short id)
        {
            try
            {
                return _db.CashDesks.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public short GetMaxId()
        {
            try
            {
                return _db.CashDesks.Max(i => i.Id);
            }
            catch (Exception e)
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
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        ~CashDeskRepository()
        {
            Dispose(false);
        }
    }
}