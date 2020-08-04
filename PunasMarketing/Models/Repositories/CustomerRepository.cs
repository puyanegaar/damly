using PunasMarketing.Models.DomainModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class CustomerRepository
    {
        private readonly gsharing_DamliEntities _db;

        public CustomerRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Customer entity, bool autosave = true)
        {
            try
            {
                _db.Customers.Add(entity);

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

        public bool Update(Customer entity)
        {
            try
            {
                _db.Customers.Attach(entity);
                _db.Entry(entity).State = EntityState.Modified;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception e)
            {
                return false;
            }
        }

        public bool UpadteTafsili(int customerId, string name)
        {
            try
            {
                var result = _db.UpdateCustomerTafsili(customerId, name);
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

        public IQueryable<Customer> Select()
        {
            try
            {
                return _db.Customers.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Customer> Where(Expression<Func<Customer, bool>> predicate)
        {
            try
            {
                return _db.Customers.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Customer, TResult>> selector)
        {
            try
            {
                return _db.Customers.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public bool BusinessNameExists(string businessName)
        {
            try
            {
                return _db.Customers.Any(i => string.Equals(i.BusinessName.Trim(), businessName.Trim()));
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
                return _db.Customers.Any(i => string.Equals(i.Mobile1.Trim(), mobile1.Trim()));
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
                return _db.Customers.Any(i => string.Equals(i.CodeMelli.Trim(), codeMelli.Trim()));
            }
            catch
            {
                return false;
            }
        }

        public Customer Find(int id)
        {
            try
            {
                return _db.Customers.AsNoTracking().FirstOrDefault(i => i.Id == id);
            }
            catch
            {
                return null;
            }
        }

        public Customer FindByUsername(string username)
        {
            try
            {
                return _db.Customers.FirstOrDefault(i => i.Username == username);
            }
            catch
            {
                return null;
            }
        }

        public List<CustomerlReport_Result> GetFinancialReport(short fiscalId, int customerId)
        {
            try
            {
                return _db.CustomerlReport(fiscalId, customerId).ToList();
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public bool Exists(int id)
        {
            try
            {
                return _db.Customers.Any(i => i.Id == id);
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
                if (_db.Customers.Any())
                {
                    return _db.Customers.OrderByDescending(p => p.Id).First().Id;
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

        public bool Login(string userName, string Password, ref int userId)
        {
            try
            {
                var user = _db.Customers.FirstOrDefault(m => m.Username.Equals(userName));
                if (user != null)
                {
                    string Truepass = user.Password.DecryptString("#+Samoosh!Group@+Generate$pas+#");
                    string TrueKey = user.SaltCode.DecryptString("#+Samoosh!Group@+Generate$key+#");
                    string Receivepass = Utilities.EncodePassword(Password, TrueKey);
                    if (Receivepass.Trim() == Truepass.Trim())
                    {
                        userId = user.Id;
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
            catch (Exception e)
            {
                return false;
            }
        }
        public int GetMaxid()
        {
            try
            {
                return _db.Customers.Max(i => i.Id);
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

        ~CustomerRepository()
        {
            Dispose(false);
        }
    }
}