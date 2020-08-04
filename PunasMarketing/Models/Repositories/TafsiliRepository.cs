using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class TafsiliRepository
    {
        private gsharing_DamliEntities _db;

        public TafsiliRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool AddRange(IEnumerable<Tafsili> tafsilis, bool autosave = true)
        {
            try
            {
                _db.Tafsilis.AddRange(tafsilis);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Add(Tafsili entity, bool autosave = true)
        {
            try
            {
                _db.Tafsilis.Add(entity);
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

    
        public bool Update(Tafsili entity)
        {
            try
            {
                _db.Tafsilis.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
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

        public Tafsili Find(int id)
        {
            try
            {
                return _db.Tafsilis.AsNoTracking().FirstOrDefault(i => i.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public Tafsili FindByOtherTafsiliId(short otherTafsiliId)
        {
            try
            {
                return _db.Tafsilis.AsNoTracking().FirstOrDefault(i => i.OtherTafsiliId == otherTafsiliId);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Tafsili> Select()
        {
            try
            {
                return _db.Tafsilis.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

      

        public IQueryable<Tafsili> Where(Expression<Func<Tafsili, bool>> predicate)
        {
            try
            {
                return _db.Tafsilis.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

    

        public IQueryable<TResult> Select<TResult>(Expression<Func<Tafsili, TResult>> selector)
        {
            try
            {
                return _db.Tafsilis.Select(selector);
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

        ~TafsiliRepository()
        {
            Dispose(false);
        }
    }
}