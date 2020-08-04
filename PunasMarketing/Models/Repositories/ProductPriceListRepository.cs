using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class ProductPriceListRepository
    {
        private readonly gsharing_DamliEntities _db;

        public ProductPriceListRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(short productid, short PriceTypeId, decimal value, bool IsVisible, bool autoSave = true)
        {
            try
            {
                ProductPriceList entity = new ProductPriceList();
                entity.ProductId = productid;
                entity.PriceTypeId = PriceTypeId;
                entity.Price = value;
                entity.IsVisible = IsVisible;
                _db.ProductPriceLists.Add(entity);
                if (autoSave)
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

        public bool Update(ProductPriceList entity,bool autoSave = true)
        {
            try
            {
                _db.ProductPriceLists.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                if (autoSave)
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

        public bool Delete(ProductPriceList entity, bool autoSave = true)
        {
            try
            {
                _db.Entry(entity).State = EntityState.Deleted;
                if (autoSave)
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

        public IQueryable<ProductPriceList> Select()
        {
            try
            {
                return _db.ProductPriceLists.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<ProductPriceList> Where(Expression<Func<ProductPriceList, bool>> predicate)
        {
            try
            {
                return _db.ProductPriceLists.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<ProductPriceList, TResult>> selector)
        {
            try
            {
                return _db.ProductPriceLists.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public ProductPriceList Find(int id)
        {
            try
            {
                return _db.ProductPriceLists.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public decimal GetProductCustomerPrice(short productId)
        {
            try
            {
                return _db.ProductPriceLists.First(i => i.ProductId == productId && i.PriceTypeId == 0).Price;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public int GetMaxid()
        {
            try
            {
                return _db.ProductPriceLists.Max(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        public bool Save()
        {
            try
            {
                return Convert.ToBoolean(_db.SaveChanges());
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

        ~ProductPriceListRepository()
        {
            Dispose(false);
        }
    }
}