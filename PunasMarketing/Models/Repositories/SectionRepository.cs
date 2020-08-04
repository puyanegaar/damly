using PunasMarketing.Models.DomainModel;
using System;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;

namespace PunasMarketing.Models.Repositories
{
    public class SectionRepository
    {
        private readonly gsharing_DamliEntities _db;

        public SectionRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Section entity, bool autosave = true)
        {
            try
            {
                _db.Sections.Add(entity);
                if (autosave)
                {
                    return Convert.ToBoolean(_db.SaveChanges());
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Section entity)
        {
            try
            {
                _db.Sections.Attach(entity);
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
        
        public IQueryable<Section> Select()
        {
            try
            {
                return _db.Sections.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Section> Where(Expression<Func<Section, bool>> predicate)
        {
            try
            {
                return _db.Sections.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Section, TResult>> selector)
        {
            try
            {
                return _db.Sections.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Section Find(int id)
        {
            try
            {
                return _db.Sections.Find(id);
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
                return _db.Sections.Max(i => i.Id);
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

        ~SectionRepository()
        {
            Dispose(false);
        }
    }
}