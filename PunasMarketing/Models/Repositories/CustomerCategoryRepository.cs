using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class CustomerCategoryRepository
    {
        private readonly gsharing_DamliEntities _db;

        public CustomerCategoryRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(CustomerCategory entity, bool autosave = true)
        {
            try
            {
                _db.CustomerCategories.Add(entity);
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

        public bool Update(CustomerCategory entity)
        {
            try
            {
                _db.CustomerCategories.Attach(entity);
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

        public IQueryable<CustomerCategory> Select()
        {
            try
            {
                return _db.CustomerCategories.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<CustomerCategory> Where(Expression<Func<CustomerCategory, bool>> predicate)
        {
            try
            {
                return _db.CustomerCategories.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<CustomerCategory, TResult>> selector)
        {
            try
            {
                return _db.CustomerCategories.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public CustomerCategory Find(int id)
        {
            try
            {
                return _db.CustomerCategories.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public int GetMaxid()
        {
            try
            {
                return _db.CustomerCategories.Max(i => i.Id);
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

        ~CustomerCategoryRepository()
        {
            Dispose(false);
        }
    }
}