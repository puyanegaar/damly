using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace PunasMarketing.Models.Repositories
{
    public class FiscalRepository: IDisposable
    {
        private gsharing_DamliEntities _db;

        public FiscalRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }
        public bool Add(FiscalPeriod entity)
        {
            try
            {
                _db.FiscalPeriods.Add(entity);
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception e)
            {
                return false;
            }
        }
        public IQueryable<FiscalPeriod> Select()
        {
            try
            {
                return _db.FiscalPeriods.AsQueryable().OrderBy(m=>m.Id);
            }
            catch
            {
                return null;
            }
        }

        public bool Update(FiscalPeriod entity)
        {
            try
            {
                _db.FiscalPeriods.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool CheckClosed(short id)
        {
            try
            {
                var Fiscal = _db.FiscalPeriods.Find(id);
                if(Fiscal.IsClosed)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }
        }

        public FiscalPeriod Find (short id)
        {
            try
            {
                return _db.FiscalPeriods.Find(id);
            }
            catch(Exception e)
            {
                return null;
            }
        }

        public IQueryable<FiscalPeriod> Where(Expression<Func<FiscalPeriod, bool>> predicate)
        {
            try
            {
                return _db.FiscalPeriods.Where(predicate);
            }
            catch
            {
                return null;
            }
        }
        public short GetLastIdentity()
        {
            try
            {
                if (_db.FiscalPeriods.Any())
                    return _db.FiscalPeriods.OrderByDescending(p => p.Id).First().Id;
                else
                    return 0;
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

        ~FiscalRepository()
        {
            Dispose(false);
        }
    }
}