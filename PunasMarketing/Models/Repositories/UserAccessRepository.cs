using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class UserAccessRepository
    {
        private readonly gsharing_DamliEntities _db;

        public UserAccessRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(short ActionId, int UserId, bool autosave = true)
        {
            try
            {
                UserAccess entity = new UserAccess();
                entity.ActionId = ActionId;
                entity.UserId = UserId;
                _db.UserAccesses.Add(entity);
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

        public bool Update(UserAccess entity)
        {
            try
            {
                _db.UserAccesses.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(UserAccess entity, bool autoSave = true)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Deleted;
                if (autoSave)
                    return Convert.ToBoolean(_db.SaveChanges());
                else
                    return true;
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
        public IQueryable<UserAccess> Select()
        {
            try
            {
                return _db.UserAccesses.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<UserAccess> Where(Expression<Func<UserAccess, bool>> predicate)
        {
            try
            {
                return _db.UserAccesses.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<UserAccess, TResult>> selector)
        {
            try
            {
                return _db.UserAccesses.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public UserAccess Find(int id)
        {
            try
            {
                return _db.UserAccesses.Find(id);
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
            catch (Exception e)
            {
                return false;
            }
        }

        public int GetMaxid()
        {
            try
            {
                return _db.UserAccesses.Max(i => i.Id);
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
                _db?.Dispose();
            }
        }

        ~UserAccessRepository()
        {
            Dispose(false);
        }
    }
}