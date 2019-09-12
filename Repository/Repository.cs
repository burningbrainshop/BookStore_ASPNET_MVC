using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Data;
using System.Data.Linq;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.ComponentModel.DataAnnotations;
using BookStore.Repository;
using BookStore.Models;


namespace BookStore.Repository
{
    public class Repository<T> : IRepository<T>, IRepository where T : class
    {
        protected DbContext _context = new BookStoreContext();
        protected DbSet<T> _table;

        public Repository()
        {
            _table = _context.Set<T>();
        }

        public virtual void Insert(T entity)
        {
            _table.Add(entity);
            MakeChange();
        }

        public virtual void Delete(T entity)
        {
            _table.Remove(entity);
            MakeChange();
        }

        public virtual void DeleteAll(IEnumerable<T> entities)
        {
            _table.RemoveRange(entities);
            MakeChange();
        }

        public virtual void Edit(T entity)
        {
            DbEntityEntry<T> entry = _context.Entry(_table.Attach(entity));
            DbPropertyValues values = entry.GetDatabaseValues();
            foreach (var propertyName in entry.CurrentValues.PropertyNames)
            {
                if (!propertyName.Equals("CreatedOn"))
                {
                    //if (!values[propertyName].Equals(entry.CurrentValues[propertyName]))
                    //{
                        entry.Property(propertyName).IsModified = true;
                    //}
                }
            }
            MakeChange();
        }

        public virtual IQueryable<T> SearchFor(System.Linq.Expressions.Expression<Func<T, bool>> exp)
        {
            return _table.Where(exp);
        }

        public virtual IQueryable<T> GetAll()
        {
            return _table;
        }

        public virtual T GetById(int id)
        {
            //return _table.Find(id);
            
            var keyProperty = typeof(T).GetProperties().Where(k => k.GetCustomAttributes(typeof(KeyAttribute), true).Length == 1);
            var entityParameter = Expression.Parameter(typeof(T), "entity");
            var expression = Expression.Lambda<Func<T, bool>>(
                Expression.Equal(
                    Expression.Property(
                        entityParameter,
                        keyProperty.FirstOrDefault().Name
                    ),
                    Expression.Constant(id)
                ),
                new[] { entityParameter }
            );
            return GetAll().Where(expression).FirstOrDefault();
        }

        protected void MakeChange()
        {
            _context.SaveChanges();
        }

        public void Insert(object entity)
        {
            Insert((T)entity);
        }

        public void Delete(object entity)
        {
            Delete((T)entity);
        }

        public void DeleteAll(IEnumerable<object> entitis)
        {
            DeleteAll((IEnumerable<T>)entitis);
        }

        public void Edit(object entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            MakeChange();
        }

        public IQueryable SearchFor(System.Linq.Expressions.Expression<Func<object, bool>> exp)
        {
            return SearchFor(exp);
        }

        IQueryable IRepository.GetAll()
        {
            return GetAll();
        }

        object IRepository.GetById(int id)
        {
            return GetById(id);
        }
    }
}