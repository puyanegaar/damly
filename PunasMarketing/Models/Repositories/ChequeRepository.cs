using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class ChequeRepository
    {
        private readonly gsharing_DamliEntities _db;

        public ChequeRepository(gsharing_DamliEntities db)
        {
            _db =db;
        }

        public bool Add(Cheque entity, bool autosave = true)
        {
            try
            {
                _db.Cheques.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }

                return true;
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool Update(Cheque entity, bool autosave = true)
        {
            try
            {
                _db.Cheques.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public bool Delete(int id, out bool isUsed, bool autosave = true)
        {
            isUsed = false;

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
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public IQueryable<Cheque> Select()
        {
            try
            {
                return _db.Cheques.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Cheque> Where(Expression<Func<Cheque, bool>> predicate)
        {
            try
            {
                return _db.Cheques.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Cheque, TResult>> selector)
        {
            try
            {
                return _db.Cheques.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Cheque Find(int id)
        {
            try
            {
                return _db.Cheques.AsNoTracking().FirstOrDefault(i => i.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public ObjectResult<GetPendingCheques_Result> GetPendingCheques(bool isReceive)
        {
            try
            {
                return _db.GetPendingCheques(isReceive);
            }
            catch
            {
                return null;
            }
        }

        public ObjectResult<string> GetChequeCounterParty(int chequeId)
        {
            try
            {
                return _db.GetChequeCounterPartyName(chequeId);
            }
            catch
            {
                return null;
            }
        }

        public bool ChequeNumExists(string chequeNum)
        {
            try
            {
                return _db.Cheques.Any(i => i.ChequeNum.Trim().Equals(chequeNum.Trim()));
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
                if (_db.Cheques.Any())
                {
                    return _db.Cheques.OrderByDescending(p => p.Id).First().Id;
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

        public bool Save()
        {
            try
            {
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception e)
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
                _db?.Dispose();
            }
        }

        ~ChequeRepository()
        {
            Dispose(false);
        }
    }
}