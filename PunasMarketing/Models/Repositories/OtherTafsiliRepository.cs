using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class OtherTafsiliRepository
    {
        private gsharing_DamliEntities _db;

        public OtherTafsiliRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(OtherTafsili entity, bool autosave = true)
        {
            try
            {
                _db.OtherTafsilis.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Update(OtherTafsili entity)
        {
            try
            {
                _db.OtherTafsilis.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }
        public bool UpadteTafsili(short id, string name)
        {
            try
            {
                var result = _db.UpdateOtherTafsiliTafsili(id, name);
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

        public OtherTafsili Find(short id)
        {
            try
            {
                return _db.OtherTafsilis.AsNoTracking().FirstOrDefault(i => i.Id == id);
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
                return _db.OtherTafsilis.Max(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        public IQueryable<OtherTafsili> Select()
        {
            try
            {
                return _db.OtherTafsilis.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<OtherTafsili> Where(Expression<Func<OtherTafsili, bool>> predicate)
        {
            try
            {
                return _db.OtherTafsilis.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<OtherTafsili, TResult>> selector)
        {
            try
            {
                return _db.OtherTafsilis.Select(selector);
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

        ~OtherTafsiliRepository()
        {
            Dispose(false);
        }
    }
}