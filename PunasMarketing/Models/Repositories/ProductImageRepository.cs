using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.Repositories
{
    public class ProductImageRepository:IDisposable
    {
        private readonly gsharing_DamliEntities _db;
        public ProductImageRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(ProductImage entity, bool autosave = true)
        {
            try
            {
                _db.ProductImages.Add(entity);
                if (autosave)
                    return Convert.ToBoolean(_db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        public ProductImage Find(int id)
        {
            try
            {
                return _db.ProductImages.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public bool Delete(int id, bool autoSave = true)
        {
            try
            {

                var entity = _db.ProductImages.Find(id);
                _db.Entry(entity).State = EntityState.Deleted;
                if (autoSave)
                {

                    bool result = Convert.ToBoolean(_db.SaveChanges());
                    if (result)
                    {
                        try
                        {

                            if (File.Exists(AppDomain.CurrentDomain.BaseDirectory + "Content\\Upload\\Image\\Products\\" + entity.ImageName) == true)
                            {
                                File.Delete(AppDomain.CurrentDomain.BaseDirectory + "Content\\Upload\\Image\\Products\\" + entity.ImageName);
                            }
                        }
                        catch { }
                    }
                    return result;
                }
                else
                    return false;


            }
            catch
            {
                return false;
            }
        }
        public int GetLastIdentity()
        {
            try
            {
                if (_db.ProductImages.Any())
                    return _db.ProductImages.OrderByDescending(p => p.Id).First().Id;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }

        public bool SaveChange()
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
        public IQueryable<ProductImage> Select()
        {
            try
            {
                return _db.ProductImages.AsQueryable();
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<ProductImage> Where(System.Linq.Expressions.Expression<Func<ProductImage, bool>> predicate)
        {
            try
            {
                return _db.ProductImages.Where(predicate);
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
                if (this._db != null)
                {
                    this._db.Dispose();
                }
            }
        }
        ~ProductImageRepository()
        {
            Dispose(false);
        }
    }
}