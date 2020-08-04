using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class SarfaslRepository
    {
        private gsharing_DamliEntities _db;

        public SarfaslRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Sarfasl entity, bool autosave = true)
        {
            try
            {
                _db.Sarfasls.Add(entity);
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

        public bool Update(Sarfasl entity)
        {
            try
            {
                _db.Sarfasls.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Delete(Sarfasl sarfasl, out bool isUsed)
        {
            isUsed = false;

            try
            {
                _db.Entry(sarfasl).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public bool CodingExists(string coding)
        {
            try
            {
                return _db.Sarfasls.Any(i => i.Coding == coding);
            }
            catch
            {
                return false;
            }
        }

        public bool CodeExists(short? parentId, byte code)
        {
            try
            {
                return _db.Sarfasls.Any(i => i.ParentId == parentId && i.Code == code);
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<Sarfasl> Select()
        {
            try
            {
                return _db.Sarfasls.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Sarfasl> Where(Expression<Func<Sarfasl, bool>> predicate)
        {
            try
            {
                return _db.Sarfasls.Where(predicate);
                
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Sarfasl, TResult>> selector)
        {
            try
            {
                return _db.Sarfasls.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Sarfasl Find(short id)
        {
            try
            {
                return _db.Sarfasls.AsNoTracking().FirstOrDefault(i => i.Id == id);
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

        ~SarfaslRepository()
        {
            Dispose(false);
        }
    }
}