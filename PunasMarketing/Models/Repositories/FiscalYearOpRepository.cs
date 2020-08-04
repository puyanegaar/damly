using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Transactions;
using System.Web;

namespace PunasMarketing.Models.Repositories
{
    public class FiscalYearOpRepository
    {
        private gsharing_DamliEntities _db;

        public FiscalYearOpRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool AddSanad(Sanad entity, bool autoSave = true)
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
            catch
            {
                return false;
            }
        }

        public bool AddFiscal(FiscalPeriod entity, bool autoSave = true)
        {
            try
            {
                _db.FiscalPeriods.Add(entity);

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

        public SanadItem AddSanadItem(int sanadId, short FiscalId, DateTime ConfirmDate, int? chequeId, string IssueTracking, short sarfaslId, int? TafsiliId, string Des, decimal Bed, decimal Bes, bool autosave = true)
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
                return entity;
            }
            catch
            {
                return null;
            }
        }

        public FiscalPeriod FiscalFind(short id)
        {
            try
            {
                return _db.FiscalPeriods.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public bool CloseFinanYearOp(IEnumerable<SanadItem> Tavazon, IEnumerable<SanadItem> sanadItems, FiscalPeriod Fiscal, Sanad CloseSanad, Sanad TavaZonSanad, Sanad OpenSanad, short CurrentFiscalId)
        {
            using (DbContextTransaction transaction = _db.Database.BeginTransaction())
            {
                try
                {
                    _db.Sanads.Add(TavaZonSanad);
                    _db.SaveChanges();
                    foreach (var TavaonItem in Tavazon)
                    {
                        var Item = AddSanadItem(TavaZonSanad.Id, CurrentFiscalId, TavaZonSanad.ConfirmDate, null, "", TavaonItem.SarfaslId, TavaonItem.TafsiliId, TavaonItem.Description, TavaonItem.Bedeh, TavaonItem.Bestan);
                        _db.SanadItems.Add(Item);
                    }
                    _db.SaveChanges();
                    _db.Sanads.Add(CloseSanad);
                    _db.SaveChanges();
                    var CloseItemList = sanadItems.Where(m => m.PeriodId == CurrentFiscalId);
                    foreach (var CloseItem in CloseItemList)
                    {
                        var Item = AddSanadItem(CloseSanad.Id, CurrentFiscalId, TavaZonSanad.ConfirmDate, null, "", CloseItem.SarfaslId, CloseItem.TafsiliId, CloseItem.Description, CloseItem.Bedeh, CloseItem.Bestan);
                        _db.SanadItems.Add(Item);
                    }
                    _db.SaveChanges();
                    var date = Fiscal.ClosedDate;
                    Fiscal.ClosedDate = null;
                    _db.FiscalPeriods.Add(Fiscal);
                    _db.SaveChanges();
                    _db.Sanads.Add(OpenSanad);
                    _db.SaveChanges();
                    var OpenItemList = sanadItems.Where(m => m.PeriodId == Fiscal.Id);
                    foreach (var OpenItem in OpenItemList)
                    {
                        var Item = AddSanadItem(OpenSanad.Id, Fiscal.Id, TavaZonSanad.ConfirmDate, null, "", OpenItem.SarfaslId, OpenItem.TafsiliId, OpenItem.Description, OpenItem.Bedeh, OpenItem.Bestan);
                        _db.SanadItems.Add(Item);
                    }
                    _db.SaveChanges();
                    var CF = _db.FiscalPeriods.Find(CurrentFiscalId);
                    CF.IsClosed = true;
                    CF.ClosedDate = date;
                    _db.FiscalPeriods.Attach(CF);
                    _db.Entry(CF).State = EntityState.Modified;
                    _db.SaveChanges();
                    transaction.Commit();
                    return true;

                }
                catch
                {
                    transaction.Rollback();
                    return false;
                }
            }
        }

        public int BackUpDb(string DbName,string BackupName, string Location)
        {

            try
            {
                var DBNameParameter = new SqlParameter("DBName", DbName);
                var LocationParameter = new SqlParameter("Location", Location);
                var BackupNameParameter = new SqlParameter("BackupName",BackupName);
              
                var result = _db.Database.ExecuteSqlCommand(TransactionalBehavior.DoNotEnsureTransaction, "EXEC sp_Backup_Database @DBName,@Location,@BackupName", DBNameParameter,LocationParameter,BackupNameParameter);
                return 1;
            }
            catch (Exception e)
            {
                return -1;
            }

        }

        public FiscalPeriod GetFiscalObject(short Id,string Name,DateTime StartDate,DateTime EndDate,DateTime CloseDate,byte Vat)
        {
            FiscalPeriod fiscal = new FiscalPeriod();
            fiscal.Id = Id;
            fiscal.Name = Name;
            fiscal.StartDate = StartDate;
            fiscal.EndDate = EndDate;
            fiscal.IsClosed = false;
            fiscal.Vat = Vat;
            fiscal.ClosedDate = CloseDate;
            return fiscal;
        }

        public Sanad GetSanadObject(int Id,short FiscalId,string Description,byte sanadTypeId,int UserId,DateTime CC,decimal TotalAmount)
        {
            Sanad s = new Sanad();
            s.Id = Id;
            s.PeriodId = FiscalId;
            s.Description = Description;
            s.IsConfirmed = true;
            s.IsAutomatic = true;
            s.SanadTypeId = sanadTypeId;
            s.TotalAmount = TotalAmount;
            s.CreatedByUserId = UserId;
            s.ConfirmDate = CC;
            s.CreatedDate = CC;
            return s;
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

        ~FiscalYearOpRepository()
        {
            Dispose(false);
        }
    }
}