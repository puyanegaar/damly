using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class OrderItemRepository
    {
        private gsharing_DamliEntities _db;

        public OrderItemRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(OrderItem entity, bool autosave = true)
        {
            try
            {
                _db.OrderItems.Add(entity);
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

        public bool AddRange(IEnumerable<OrderItem> items, bool autosave = true)
        {
            try
            {
                _db.OrderItems.AddRange(items);
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

        public bool Update(OrderItem entity)
        {
            try
            {
                _db.OrderItems.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool UpdateMainUnitCountRange(Dictionary<int, double> items)
        {
            try
            {
                foreach (var item in items)
                {
                    OrderItem orderItem = _db.OrderItems.Find(item.Key);
                    if (orderItem != null)
                    {
                        orderItem.MainUnitCount = item.Value;
                        _db.OrderItems.Attach(orderItem);
                        _db.Entry(orderItem).State = EntityState.Modified;
                    }
                }

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

        public IQueryable<OrderItem> Select()
        {
            try
            {
                return _db.OrderItems.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<OrderItem> Where(Expression<Func<OrderItem, bool>> predicate)
        {
            try
            {
                return _db.OrderItems.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<OrderItem, TResult>> selector)
        {
            try
            {
                return _db.OrderItems.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public OrderItem Find(int id)
        {
            try
            {
                return _db.OrderItems.Find(id);
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

        ~OrderItemRepository()
        {
            Dispose(false);
        }

        public bool ItemsAreValid(IEnumerable<OrderItem> orderItems, out string message)
        {
            message = "";

            try
            {
                foreach (var orderItem in orderItems)
                {
                    var orderItemProduct = orderItem.Product;

                    if (!orderItemProduct.IsAvailable)
                    {
                        message = $"کالا با شناسه {orderItemProduct.Id}، غیرفعال است.";
                        return false;
                    }

                    if (!orderItemProduct.IsSellable)
                    {
                        message = $"کالا با شناسه {orderItemProduct.Id}، غیرقابل فروش است.";
                        return false;
                    }

                    if(orderItem.MainUnitCount <= 0)
                    {
                        message = "امکان تأیید سفارش با تعداد کالای مساوی یا کمتر از صفر وجود ندارد.";
                        return false;
                    }

                    double orderableCount = orderItemProduct.Inventory - orderItemProduct.PendingsCount;
                    if (orderableCount < orderItem.MainUnitCount)
                    {
                        message = $"موجودی کالا با شناسه {orderItemProduct.Id}، ناکافی است.";
                        return false;
                    }
                }

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}