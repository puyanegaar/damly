using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PunasMarketing.Models.Repositories
{
    public class OfferRepository
    {
        private readonly gsharing_DamliEntities _db;

        public OfferRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public short Add(Offer entity, bool autosave = true)
        {

            _db.Offers.Add(entity);
            if (autosave)
            {
                _db.SaveChanges();
            }

            return entity.Id;

        }

        public bool Update(Offer entity)
        {
            try
            {
                _db.Offers.Attach(entity);
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
                File.Delete(HttpContext.Current.Server.MapPath("/Content/Upload/Image/Offers/" + entity.ImageName));
                _db.Entry(entity).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public IQueryable<Offer> Select()
        {
            try
            {
                return _db.Offers.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Offer> Where(Expression<Func<Offer, bool>> predicate)
        {
            try
            {
                return _db.Offers.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Offer, TResult>> selector)
        {
            try
            {
                return _db.Offers.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Offer Find(int id)
        {
            try
            {
                return _db.Offers.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public decimal GetDiscountFromOffers(short productId, int customerId, double productCount,
            out Product giftProduct, out double giftCount)
        {
            giftProduct = null;
            giftCount = 0;

            try
            {
                var product = _db.Products.Find(productId);
                if (product == null)
                {
                    return 0;
                }

                var customerCategoryId = _db.Customers.Find(customerId)?.CustomerCategoryId;

                var offerItem = _db.OfferItems.FirstOrDefault(i =>
                    i.ProductId == productId &&
                    i.Offer.IsActive &&
                    i.Offer.StartDate <= DateTime.Now &&
                    i.Offer.ExpDate >= DateTime.Now);

                if (offerItem == null)
                {
                    return 0;
                }

                var customerCats = offerItem.Offer.ForCustomerCategories.Split(',');
                if (customerCats.Any())
                {
                    if (!customerCategoryId.HasValue || !customerCats.Contains(customerCategoryId.Value.ToString()))
                    {
                        return 0;
                    }
                }

                switch (offerItem.OfferTypeId)
                {
                    case 1:
                        return offerItem.DiscountAmount ?? 0;

                    case 2:
                        var percent = offerItem.DiscountPercent ?? 0;
                        return product.ProductPriceLists.First(i => i.PriceTypeId == 0).Price * percent / 100;

                    case 3:
                        if (productCount >= offerItem.MinProductCount)
                        {
                            giftCount = offerItem.GiftProductCount ?? 0;
                            if (giftCount > 0)
                            {
                                giftProduct = offerItem.Product;
                            }
                        }
                        return 0;

                    default:
                        return 0;
                }
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public void Save()
        {
            _db.SaveChanges();
        }

        public int GetMaxid()
        {
            try
            {
                return _db.Offers.Max(i => i.Id);
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

        ~OfferRepository()
        {
            Dispose(false);
        }
    }
}