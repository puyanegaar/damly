using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using PunasMarketing.Models.LocalModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class SanadRepository
    {
        private gsharing_DamliEntities _db;

        public SanadRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Sanad entity, bool autoSave = true)
        {
            try
            {
                _db.Sanads.Add(entity);

                if (autoSave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool AddReceive(PaymentModel receive,DateTime date, int UserId, bool autosave = true)
        {
            try
            {
                Sanad entity = new Sanad();
                entity.Id = receive.SanadId;
                entity.Description = receive.Description;
                entity.PeriodId = receive.FiscalId;
                entity.CreatedDate = date;
                entity.ConfirmDate = date;
                entity.IsConfirmed = true;
                entity.TotalAmount = receive.TotalAmount.Value;
                entity.CreatedByUserId = UserId;
                entity.IsAutomatic = true;
                entity.SanadTypeId = (byte)SandType.Receive;
                _db.Sanads.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool AddPayment(PaymentModel receive,DateTime date, int UserId, bool autosave = true)
        {
            try
            {
                Sanad entity = new Sanad();
                entity.Id = receive.SanadId;
                entity.Description = receive.Description;
                entity.PeriodId = receive.FiscalId;
                entity.CreatedDate = date;
                entity.ConfirmDate = date;
                entity.IsConfirmed = true;
                entity.TotalAmount = receive.TotalAmount.Value;
                entity.CreatedByUserId = UserId;
                entity.IsAutomatic = true;
                entity.SanadTypeId = (byte)SandType.Payment;
                _db.Sanads.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return false;
                }
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpdateReceive(PaymentModel receive,ref DateTime date)
        {
            try
            {
                var entity = Find(receive.SanadId,receive.FiscalId);
                date = entity.ConfirmDate;
                entity.Description = receive.Description;
                entity.IsConfirmed = true;
                entity.TotalAmount = receive.TotalAmount.Value;
                entity.IsAutomatic = true;
                entity.SanadTypeId = (byte)SandType.Receive;
                _db.Sanads.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

        public bool UpdatePayment(PaymentModel payment, ref DateTime date)
        {
            try
            {
                var entity = Find(payment.SanadId,payment.FiscalId);
                date = entity.ConfirmDate;
                entity.Description = payment.Description;
                entity.IsConfirmed = true;
                entity.TotalAmount = payment.TotalAmount.Value;
                entity.IsAutomatic = true;
                entity.SanadTypeId = (byte)SandType.Payment;
                _db.Sanads.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch
            {
                return false;
            }
        }

  
        public bool Update(Sanad entity)
        {
            try
            {
                _db.Sanads.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool Delete(Sanad sanad, out bool isUsed)
        {
            isUsed = false;

            try
            {
                _db.Entry(sanad).State = EntityState.Deleted;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public bool Delete(int id,short fiscalId, out bool isUsed)
        {
            isUsed = false;

            try
            {
                var entity = Find(id, fiscalId);
                _db.Entry(entity).State = EntityState.Deleted;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public bool DeleteRange(IEnumerable<Sanad> sanads)
        {
            try
            {
                _db.Sanads.RemoveRange(sanads);

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception ex)
            {
                return false;
            }
        }

        public Sanad Find(int id,short Fiscal)
        {
            try
            {
                return _db.Sanads.Find(id,Fiscal);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Sanad> Select()
        {
            try
            {
                return _db.Sanads.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Sanad> Where(Expression<Func<Sanad, bool>> predicate)
        {
            try
            {
                return _db.Sanads.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Sanad, TResult>> selector)
        {
            try
            {
                return _db.Sanads.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<PunasMarketing.Models.DomainModel.SanadDetail_Result> GetSanadDetail(short Fiscal, int SanadId)
        {
            try
            {

                var Result = _db.SanadDetail(Fiscal, SanadId).ToList();
                return Result;

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public int GetLastIdentity(short Fiscal)
        {
            try
            {
                var sanad = _db.Sanads.Where(m => m.PeriodId == Fiscal);
                if (sanad.Any())
                {
                    return sanad.OrderByDescending(p => p.Id).First().Id;
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

        ~SanadRepository()
        {
            Dispose(false);
        }
    }
}