using System;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using PunasMarketing.Models.DomainModel;
using Action = PunasMarketing.Models.DomainModel.Action;

namespace PunasMarketing.Models.Repositories
{
    public class ActionRepository
    {
        private readonly gsharing_DamliEntities _db;

        public ActionRepository(gsharing_DamliEntities db)
        {
            _db = db;
        }

        public bool Add(Action entity, bool autosave = true)
        {
            try
            {
                _db.Actions.Add(entity);
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

        public bool Update(Action entity)
        {
            try
            {
                _db.Actions.Attach(entity);
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

        public IQueryable<Action> Select()
        {
            try
            {
                return _db.Actions.AsQueryable();
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<Action> Where(Expression<Func<Action, bool>> predicate)
        {
            try
            {
                return _db.Actions.Where(predicate);
            }
            catch
            {
                return null;
            }
        }

        public IQueryable<TResult> Select<TResult>(Expression<Func<Action, TResult>> selector)
        {
            try
            {
                return _db.Actions.Select(selector);
            }
            catch
            {
                return null;
            }
        }

        public Action Find(int id)
        {
            try
            {
                return _db.Actions.Find(id);
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
                return _db.Actions.Max(i => i.Id);
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

        ~ActionRepository()
        {
            Dispose(false);
        }
    }
}