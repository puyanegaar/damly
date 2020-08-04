using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class PersonnelRepository
    {
        private readonly gsharing_DamliEntities _db;

        public PersonnelRepository(gsharing_DamliEntities db)
        {
            _db =db;
        }

        public bool Add(Personnel entity, bool autosave = true)
        {
            try
            {
                _db.Personnels.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }

                return true;
            }
            catch
            {
                return false;
            }
        }

        public List<PersonnelReport_Result> GetFinancialReport(short fiscalId, int personnelId)
        {
            try
            {
                return _db.PersonnelReport(fiscalId, personnelId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Update(Personnel entity)
        {
            try
            {
                _db.Personnels.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;
                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch(Exception e)
            {
                return false;
            }
        }

        public bool UpadteTafsili(int personnelId, string name)
        {
            try
            {
                var result = _db.UpdatePersonnelTafsili(personnelId, name);
                return Convert.ToBoolean(result);
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

        public IQueryable<Personnel> Select()
        {
            try
            {
                return _db.Personnels.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Personnel> Where(Expression<Func<Personnel, bool>> predicate)
        {
            try
            {
                return _db.Personnels.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Personnel, TResult>> selector)
        {
            try
            {
                return _db.Personnels.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Personnel Find(int id)
        {
            try
            {
                return _db.Personnels.AsNoTracking().FirstOrDefault(i => i.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public bool CodeExisits(string code)
        {
            try
            {
                return _db.Personnels.Any(m => m.PersonnelCode.Trim().Equals(code.Trim()));
            }
            catch
            {
                return false;
            }
        }

        public bool CodeMelliExists(string codeMelli)
        {
            try
            {
                return _db.Personnels.Any(m => m.CodeMelli.Trim().Equals(codeMelli.Trim()));
            }
            catch
            {
                return false;
            }
        }

        public bool Mobile1Exists(string mobile1)
        {
            try
            {
                return _db.Personnels.Any(m => m.Mobile1.Trim().Equals(mobile1.Trim()));
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
                if (_db.Personnels.Any())
                    return _db.Personnels.OrderByDescending(p => p.Id).First().Id;
                else
                    return 0;
            }
            catch
            {
                return -1;
            }
        }


        public bool Login(string userName,string Password, ref User userout)
        {
            try
            {
                var user = _db.Users.FirstOrDefault(m => m.Username.Equals(userName));
                if (user != null)
                {
                    string Truepass = user.Password.DecryptString("#+Samoosh!Group@+Generate$pas+#");
                    string TrueKey = user.SaltCode.DecryptString("#+Samoosh!Group@+Generate$key+#");
                    string Receivepass = Utilities.EncodePassword(Password,TrueKey);
                    if(Receivepass.Trim()==Truepass.Trim())
                    {
                        userout = user;
                        return true;
                    }
                    else
                    {
                        return false;
                    }
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

        public int GetMaxid()
        {
            try
            {
                return _db.Personnels.Max(i => i.Id);
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

        ~PersonnelRepository()
        {
            Dispose(false);
        }
    }
}