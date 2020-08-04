using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class UserRepository
    {
        private readonly gsharing_DamliEntities _db;

        public UserRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }
        public bool Add(User entity, bool autosave = true)
        {
            try
            {
                _db.Users.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }

                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Update(User entity)
        {
            try
            {
                _db.Users.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id)
        {
            try
            {
                var entity = FindById(id);
                _db.Entry(entity).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<User> Select()
        {
            try
            {
                return _db.Users.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<User> Where(Expression<Func<User, bool>> predicate)
        {
            try
            {
                return _db.Users.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<User, TResult>> selector)
        {
            try
            {
                return _db.Users.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public User FindById(int id)
        {
            try
            {
                return _db.Users.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public User FindByUsername(string username)
        {
            try
            {
                return _db.Users.FirstOrDefault(i => i.Username == username);
            }
            catch
            {
                return null;
            }
        }

        public bool Exists(int personnelId)
        {
            try
            {
                return _db.Users.Any(i => i.PersonnelId == personnelId);
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

        ~UserRepository()
        {
            Dispose(false);
        }
    }
}