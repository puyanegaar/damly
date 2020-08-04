using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class ProductCategoryRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public ProductCategoryRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(ProductCategory entity, bool autosave = true)
        {
            try
            {
                _db.ProductCategories.Add(entity);
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
        public bool Update(ProductCategory entity)
        {
            try
            {
                _db.ProductCategories.Attach(entity);
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

        public IQueryable<ProductCategory> Select()
        {
            try
            {
                return _db.ProductCategories.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<ProductCategory> Where(Expression<Func<ProductCategory, bool>> predicate)
        {
            try
            {
                return _db.ProductCategories.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<ProductCategory, TResult>> selector)
        {
            try
            {
                return _db.ProductCategories.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public ProductCategory Find(int id)
        {
            try
            {
                return _db.ProductCategories.Find(id);
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
                return _db.ProductCategories.Max(i => i.Id);
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
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        ~ProductCategoryRepository()
        {
            Dispose(false);
        }
    }
}