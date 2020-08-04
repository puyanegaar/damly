using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class OrderRepository
    {
        private gsharing_DamliEntities _db;

        public OrderRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Order entity, bool autosave = true)
        {
            try
            {
                _db.Orders.Add(entity);
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

        public bool Update(Order entity)
        {
            try
            {
                _db.Orders.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception ex)
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

        public IQueryable<Order> Select()
        {
            try
            {
                return _db.Orders.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Order> Where(Expression<Func<Order, bool>> predicate)
        {
            try
            {
                return _db.Orders.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Order, TResult>> selector)
        {
            try
            {
                return _db.Orders.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Order Find(int id)
        {
            try
            {
                return _db.Orders.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public int GetLastId()
        {
            try
            {
                return _db.Orders.Max(i => i.Id);
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

        ~OrderRepository()
        {
            Dispose(false);
        }
    }
}