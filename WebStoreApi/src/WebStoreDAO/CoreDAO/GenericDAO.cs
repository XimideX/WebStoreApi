using System;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using WebStoreModel.CoreModel;

namespace WebStoreDAO.CoreDAO
{
    public abstract class GenericDAO<GenericModel> where GenericModel : BaseModel
    {
        internal WebStoreContext _context;
        internal DbSet<GenericModel> dbSet;
        public GenericDAO(WebStoreContext context)
        {
            _context = context;
            dbSet = context.Set<GenericModel>();
        }

        public virtual IQueryable<GenericModel> Select(Expression<Func<GenericModel, bool>> filter = null,
           Func<IQueryable<GenericModel>, IOrderedQueryable<GenericModel>> orderBy = null,
           Func<IQueryable<GenericModel>, IQueryable<GenericModel>> addIncludes = null)
        {
            IQueryable<GenericModel> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (addIncludes == null) 
            {
                query = this.AddDefaultIncludes(query);
            } 
            else 
            {
                query = addIncludes(query);    
            }
            
            if (orderBy == null) orderBy = (list) => list.OrderBy(m => m.Id);
            return orderBy(query);
        }

        public IQueryable<GenericModel> SelectAll(Func<IQueryable<GenericModel>, IOrderedQueryable<GenericModel>> orderBy = null,
            Func<IQueryable<GenericModel>,IQueryable<GenericModel>> addIncludes = null)
        {
            Expression<Func<GenericModel, bool>> whereCondition = (model) => 1 == 1;
            return this.Select(whereCondition, orderBy, addIncludes);
        }

        public GenericModel Insert(GenericModel model)
        {
            _context.Add(model);
            return model;
        }

        public void Delete(GenericModel model)
        {
           if (_context.Entry(model).State == EntityState.Detached)
            {
                dbSet.Attach(model);
            }
            dbSet.Remove(model);
        }

        public virtual GenericModel SelectOneById(int id)
        {
            return this.Select(p => p.Id == id).FirstOrDefault();
        }

        public GenericModel Update(GenericModel model)
        {
            _context.Set<GenericModel>().Attach(model);
            _context.Entry(model).State = EntityState.Modified;
            return model;
        }

        public GenericModel InsertOrUpdate(GenericModel model)
        {
           if (model != null && model.Id > 0)
            {
                this.Update(model);
            }
            else
            {
                this.Insert(model);
            }
            return model;
        }

        protected virtual IQueryable<GenericModel> AddDefaultIncludes(IQueryable<GenericModel> query)
        {
            return query; 
        }
    }
}