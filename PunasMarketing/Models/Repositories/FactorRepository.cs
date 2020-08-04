using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.ViewModels.Home;

namespace PunasMarketing.Models.Repositories
{
    public class FactorRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public FactorRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public int Add(Factor entity, bool autosave = true)
        {
            try
            {
                _db.Factors.Add(entity);
                if (autosave)
                {
                    _db.SaveChanges();
                }

                return entity.Id;
            }
            catch (Exception e)
            {
                return -1;
            }
        }
        public bool Update(Factor entity)
        {
            try
            {
                _db.Factors.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception e)
            {

                return false;
            }
        }

        public bool Delete(int id, short periodId, out bool isUsed)
        {
            isUsed = false;

            try
            {
                var entity = Find(id, periodId);
                _db.Entry(entity).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public IQueryable<Factor> Select()
        {
            try
            {
                return _db.Factors.AsQueryable();
            }
            catch
            {
                return null;
            }
        }
        //public IQueryable<view_BuyFactor> SelectView()
        //{
        //    try
        //    {
        //        return _db.view_BuyFactor.AsQueryable();
        //    }
        //    catch
        //    {
        //        return null;
        //    }
        //}
        public IQueryable<Factor> Where(Expression<Func<Factor, bool>> predicate)
        {
            try
            {
                return _db.Factors.Where(predicate);
            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Factor, TResult>> selector)
        {
            try
            {
                return _db.Factors.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Factor Find(int id, short periodId)
        {
            try
            {
                return _db.Factors.SingleOrDefault(a => a.Id == id && a.PeriodId == periodId);
            }
            catch
            {
                return null;
            }
        }

        public int GetLastIdentity(short Fiscal)
        {
            try
            {
                var factor = _db.Factors.Where(m => m.PeriodId == Fiscal);
                if (factor.Any())
                {
                    return factor.OrderByDescending(p => p.Id).First().Id;
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

        ~FactorRepository()
        {
            Dispose(false);
        }
    }
}