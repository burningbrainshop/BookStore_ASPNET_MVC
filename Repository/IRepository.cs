using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using System.Linq.Expressions;

namespace BookStore.Repository
{
    public interface IRepository
    {
        void Insert(object entity);
        void Delete(object entity);
        void DeleteAll(IEnumerable<object> entities);
        void Edit(object entity);
        IQueryable SearchFor(Expression<Func<object, bool>> exp);
        IQueryable GetAll();
        object GetById(int id);
    }

    public interface IRepository<T>
    {
        void Insert(T entity);
        void Delete(T entity);
        void DeleteAll(IEnumerable<T> entities);
        void Edit(T entity);
        IQueryable<T> SearchFor(Expression<Func<T, bool>> exp);
        IQueryable<T> GetAll();
        T GetById(int id);
    }
}