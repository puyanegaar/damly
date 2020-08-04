using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class SanadItemRepository
    {
        private gsharing_DamliEntities _db;

        public SanadItemRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool AddRange(IEnumerable<SanadItem> sanadItems, bool autoSave = true)
        {
            try
            {
                _db.SanadItems.AddRange(sanadItems);

                if (autoSave)
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

        public bool Add(SanadItem entity, bool autoSave)
        {
            try
            {
                _db.SanadItems.Add(entity);

                if (autoSave)
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


        public bool Add(int sanadId,short FiscalId,DateTime ConfirmDate, int? chequeId, string IssueTracking, short sarfaslId, int? TafsiliId, string Des, decimal Bed, decimal Bes, bool autosave = true)
        {
            try
            {
                SanadItem entity = new SanadItem();
                entity.SandId = sanadId;
                entity.ConfirmDate = ConfirmDate;
                entity.PeriodId = FiscalId;
                entity.ChequeId = chequeId;
                entity.IssueTrackingNum = IssueTracking;
                entity.SarfaslId = sarfaslId;
                entity.TafsiliId = TafsiliId;
                entity.Description = Des;
                entity.Bedeh = Bed;
                entity.Bestan = Bes;
                _db.SanadItems.Add(entity);
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

        public bool Update(int sanadItemId, int sanadId, short FiscalId,DateTime ConfirmDate, int? chequeId, string IssueTracking, short sarfaslId, int? TafsiliId, string Des, decimal Bed, decimal Bes, bool autosave = true)
        {
            try
            {
                var entity = Find(sanadItemId);
                entity.SandId = sanadId;
                entity.PeriodId = FiscalId;
                entity.ChequeId = chequeId;
                entity.IssueTrackingNum = IssueTracking;
                entity.SarfaslId = sarfaslId;
                entity.TafsiliId = TafsiliId;
                entity.Description = Des;
                entity.Bedeh = Bed;
                entity.Bestan = Bes;
                entity.ConfirmDate = ConfirmDate;
                _db.SanadItems.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
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
        public bool Update(SanadItem entity)
        {
            try
            {
                _db.SanadItems.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool Delete(int id, bool autosave = true)
        {
            try
            {
                var entity = Find(id);
                _db.Entry(entity).State = EntityState.Deleted;
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
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

        public SanadItem Find(int id)
        {
            try
            {
                return _db.SanadItems.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<SanadItem> Select()
        {
            try
            {
                return _db.SanadItems.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<SanadItem> Where(Expression<Func<SanadItem, bool>> predicate)
        {
            try
            {
                return _db.SanadItems.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

      

        public IQueryable<TResult> Select<TResult>(Expression<Func<SanadItem, TResult>> selector)
        {
            try
            {
                return _db.SanadItems.Select(selector);
            }
            catch
            {
                return null;
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
                if (_db != null)
                {
                    _db.Dispose();
                    _db = null;
                }
            }
        }

        ~SanadItemRepository()
        {
            Dispose(false);
        }
    }
}