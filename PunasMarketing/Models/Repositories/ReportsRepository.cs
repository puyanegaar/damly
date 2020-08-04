using PunasMarketing.Models.DomainModel;
using PunasMarketing.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PunasMarketing.Models.Repositories
{
    public class ReportsRepository : IDisposable
    {
        private gsharing_DamliEntities _db;
        public ReportsRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public IEnumerable<PunasMarketing.Models.DomainModel.journalReport_Result> GetJournalList(short Fiscal, DateTime StartDate, DateTime EndDate)
        {
            try
            {

                return _db.journalReport(Fiscal, StartDate, EndDate).AsQueryable();

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public IEnumerable<PunasMarketing.Models.DomainModel.journalTotalAccountsReport__Result> GetJournalTAList(short Fiscal, DateTime StartDate, DateTime EndDate)
        {
            try
            {

                return _db.journalTotalAccountsReport_(Fiscal, StartDate, EndDate).AsQueryable();

            }
            catch (Exception e)
            {
                return null;
            }
        }
        public int GetMandehForLedgerReport(byte type, short SarfaslId, short Fiscal, DateTime FromDate, DateTime ToDate, int? TafsiliId, ref decimal TotalBedeh, ref decimal TotalBestan)
        {
            try
            {
                var TotalBed = new System.Data.Entity.Core.Objects.ObjectParameter("TotalBedeh", typeof(decimal));
                var TotalBes = new System.Data.Entity.Core.Objects.ObjectParameter("TotalBestan", typeof(decimal));
                var result = _db.GetMondehForLedger(type, SarfaslId, Fiscal, FromDate, ToDate, TafsiliId, TotalBed, TotalBes);
                TotalBedeh = (decimal)TotalBed.Value;
                TotalBestan = (decimal)TotalBes.Value;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int GetTotalResultBySarfasl(short SarfaslId, short Fiscal, ref decimal TotalBedeh, ref decimal TotalBestan)
        {
            try
            {
                var TotalBed = new System.Data.Entity.Core.Objects.ObjectParameter("TotalBed", typeof(decimal));
                var TotalBes = new System.Data.Entity.Core.Objects.ObjectParameter("TotalBes", typeof(decimal));
                var result = _db.GetTotalResultByTafsili(Fiscal, SarfaslId, TotalBed, TotalBes);
                TotalBedeh = (decimal)TotalBed.Value;
                TotalBestan = (decimal)TotalBes.Value;
                return 1;
            }
            catch (Exception)
            {
                return 0;
            }
        }

        public int GetTotalResultByTafsili(int TafsiliId, short Fiscal, ref decimal TotalBedeh, ref decimal TotalBestan)
        {
            try
            {
                var TotalBed = new System.Data.Entity.Core.Objects.ObjectParameter("TotalBed", typeof(decimal));
                var TotalBes = new System.Data.Entity.Core.Objects.ObjectParameter("TotalBes", typeof(decimal));
                var result = _db.GetTotalResultByTafsili(Fiscal, TafsiliId, TotalBed, TotalBes);
                TotalBedeh = (decimal)TotalBed.Value;
                TotalBestan = (decimal)TotalBes.Value;
                return 1;
            }
            catch (Exception e)
            {
                return 0;
            }
        }

        public IQueryable<PunasMarketing.Models.DomainModel.GetTafsiliForLedgerReport_Result> GetTafsiliResult(short SarfaslId)
        {
            try
            {
                return _db.GetTafsiliForLedgerReport(SarfaslId).AsQueryable();

            }
            catch
            {
                return null;
            }
        }

        public IQueryable<PunasMarketing.Models.DomainModel.GetLedgerReport_Result> GetLedgerReport(byte type, short SarfaslId, short Fiscal, DateTime FromDate, DateTime ToDate, int? TafsiliId)
        {
            try
            {
                return _db.GetLedgerReport(type, SarfaslId, Fiscal, FromDate, ToDate, TafsiliId).AsQueryable();

            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<PunasMarketing.Models.DomainModel.GetOtherDaraei_Result> GetOtherDaraei()
        {
            try
            {

                return _db.GetOtherDaraei().ToList();

            }
            catch (Exception e)
            {
                return null;
            }
        }

        public int GetTafsiliId(int? personelId, int? CustomerId, int? SupplierId, byte? Type)
        {
            try
            {
                int Id = (personelId ?? 0) + (CustomerId ?? 0) + (SupplierId ?? 0);
                int T = 0;
                switch (Type)
                {
                    case 1:
                        T = _db.Tafsilis.Where(m => m.PersonnelId == Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault().Id;
                        break;
                    case 2:
                        T = _db.Tafsilis.Where(m => m.CustomerId == Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault().Id;
                        break;
                    case 3:
                        T = _db.Tafsilis.Where(m => m.SupplierId == Id && m.SarfaslId == (short)SarfaslEnums.HesabPardakhtani).FirstOrDefault().Id;
                        break;

                };
                return T;
            }
            catch
            {
                return 0;
            }

        }
        public IEnumerable<PunasMarketing.Models.DomainModel.GetOtherBedhi_Result> GetOtherBedhi()
        {
            try
            {

                return _db.GetOtherBedhi().ToList();

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

        ~ReportsRepository()
        {
            Dispose(false);
        }
    }


}