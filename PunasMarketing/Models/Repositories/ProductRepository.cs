using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Policy;
using System.Web;
using System.Web.Http;
using AutoMapper;
using PunasMarketing.Areas;
using PunasMarketing.Areas.WebApi;
using PunasMarketing.Areas.WebApi.Models;

namespace PunasMarketing.Models.Repositories
{
    public class ProductRepository : IDisposable
    {
        private readonly gsharing_DamliEntities _db;

        public ProductRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Product entity, bool autosave = true)
        {
            try
            {
                _db.Products.Add(entity);

                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }

                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }


        public bool Update(Product entity)
        {
            try
            {
                _db.Products.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public double GetLivePendingCount(short productId, bool update = false)
        {
            try
            {
                var pendingCount = _db.FactorItems.Where(
                    i => i.ProductId == productId &&
                         !i.Factor.IsBuy &&
                         !i.Factor.IsPreFactor &&
                         !i.Factor.IsDeleted &&
                         !i.Factor.InvoiceId.HasValue).Sum(i => i.MainUnitCount);

                if (update)
                {
                    Product product = _db.Products.Find(productId);
                    if (product != null)
                    {
                        product.PendingsCount = pendingCount ?? 0;
                        _db.SaveChanges();
                    }
                }

                return pendingCount ?? 0;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        public Dictionary<short, double> GetLivePendingCount(IEnumerable<short> productIds, bool update = false)
        {
            Dictionary<short, double> result = new Dictionary<short, double>();

            try
            {
                foreach (var productId in productIds)
                {
                    var pendingCount = _db.FactorItems.Where(
                        i => i.ProductId == productId &&
                             !i.Factor.IsBuy &&
                             !i.Factor.IsPreFactor &&
                             !i.Factor.IsDeleted &&
                             !i.Factor.InvoiceId.HasValue).Sum(i => i.MainUnitCount);

                    result.Add(productId, pendingCount ?? 0);

                    if (update)
                    {
                        Product product = _db.Products.Find(productId);
                        if (product != null)
                        {
                            product.PendingsCount = pendingCount ?? 0;
                        }
                    }
                }

                if (update)
                {
                    _db.SaveChanges();
                }

                return result;
            }
            catch (Exception ex)
            {
                return result;
            }
        }

        public bool UpdateRange(IEnumerable<Product> entities)
        {
            try
            {
                foreach (var item in entities)
                {
                    var local = _db.Set<Product>().Local.FirstOrDefault(f => f.Id == item.Id);
                    if (local != null)
                    {
                        _db.Entry(local).State = EntityState.Detached;
                    }

                    //_db.Entry(item).State = EntityState.Modified;

                    _db.Products.Attach(item);
                    _db.Entry(item).State = EntityState.Modified;
                }

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public bool UpdateTafsili(short productId, string name)
        {
            try
            {
                var result = _db.UpdateProductTafsili(productId, name);
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

        public bool CodeExists(string code)
        {
            try
            {
                return _db.Products.Any(m => m.ProductCode.Trim().Equals(code.Trim()));
            }
            catch
            {
                return false;
            }
        }

        public bool NameExists(string name)
        {
            try
            {
                return _db.Products.Any(m => m.Name.Equals(name.Trim()));
            }
            catch
            {
                return false;
            }
        }

        public IQueryable<Product> Select()
        {
            try
            {
                return _db.Products.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Product> Where(Expression<Func<Product, bool>> predicate)
        {
            try
            {
                return _db.Products.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<sp_products_with_discount_foreach3_Result> GetProductsWithDiscount(
            int? customerId, short? lastId, int? count, short? productId, short? productCategoryId)
        {
            try
            {
                return _db.sp_products_with_discount_foreach3(customerId, lastId, count, productId, productCategoryId).AsQueryable();
            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Product, TResult>> selector)
        {
            try
            {
                return _db.Products.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Product Find(short id)
        {
            try
            {
                return _db.Products.AsNoTracking().FirstOrDefault(m => m.Id == id);
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
                return _db.Products.Max(i => i.Id);
            }
            catch
            {
                return 0;
            }
        }

        public int GetLastIdentity()
        {
            try
            {
                if (_db.Products.Any())
                {
                    return _db.Products.OrderByDescending(p => p.Id).First().Id;
                }
                else
                {
                    return 0;
                }
            }
            catch
            {
                return -1;
            }
        }

        public double GetProductInventory(short id)
        {
            return _db.Products.Where(a => a.Id == id).Select(a => a.Inventory).FirstOrDefault();
        }

        public bool CheckInventory(short id, double inventory)
        {
            try
            {
                var product = Find(id);
                if (product.Inventory >= inventory)
                {
                    return true;
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

        public bool UpdateProductPending(short productId, double count, bool plus = true)
        {
            try
            {
                var product = Find(productId);

                if (plus)
                {
                    product.PendingsCount += count;
                }
                else
                {
                    product.PendingsCount -= count;
                }

                return Update(product);
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool ProductInventory(short productId, double count, bool plus = true)
        {
            try
            {
                var product = Find(productId);

                if (plus)
                {
                    product.Inventory += count;
                }
                else
                {
                    product.Inventory -= count;
                }

                Update(product);
                return true;
            }
            catch (Exception e)
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

        ~ProductRepository()
        {
            Dispose(false);
        }
    }
}