using System;
using System.Data.Entity;
using System.Linq;
using PunasMarketing.Models.DomainModel;

namespace PunasMarketing.Models.Repositories
{
    public class JobTitleRepository : IDisposable
    {
        private gsharing_DamliEntities _db;

        public JobTitleRepository(gsharing_DamliEntities db)
        {
            _db = db; 
        }

        public bool Add(JobTitle entity, bool autosave = true)
        {
            try
            {
                _db.JobTitles.Add(entity);
                if (autosave)
                    return Convert.ToBoolean(_db.SaveChanges());
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }
        public bool Update(JobTitle entity)
        {
            try
            {
                _db.JobTitles.Attach(entity);
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
                _db.Entry(entity).State = EntityState.Deleted;

                return Convert.ToBoolean(_db.SaveChanges());
            }
            catch (Exception ex)
            {
                isUsed = Utilities.IsUsedInDb(ex);

                return false;
            }
        }

        public IQueryable<JobTitle> Select()
        {
            try
            {
                return _db.JobTitles.AsQueryable();
            }
            catch
            {
                return null;
            }
        }
        public IQueryable<JobTitle> Where(System.Linq.Expressions.Expression<Func<JobTitle, bool>> predicate)
        {
            try
            {
                return _db.JobTitles.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(System.Linq.Expressions.Expression<Func<JobTitle, TResult>> selector)
        {
            try
            {
                return _db.JobTitles.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public JobTitle Find(int id)
        {
            try
            {
                return _db.JobTitles.Find(id);
            }
            catch
            {
                return null;
            }
        }

        public int GetMaxid()
        {
            try
            {
                return _db.JobTitles.Max(i => i.Id);
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
                if (this._db != null)
                {
                    this._db.Dispose();
                    this._db = null;
                }
            }
        }

        ~JobTitleRepository()
        {
            Dispose(false);
        }
    }
}