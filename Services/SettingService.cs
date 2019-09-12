using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;

namespace BookStore.Services
{
    public class SettingService
    {
        private IRepository _repository;

        public SettingService(string tableName)
        {
            _repository = RepositoryFactory.GetRepository(tableName);
        }

        //For UnitTest
        public SettingService()
        {
            //_repository = new FakeCategoryRepository();
        }

        public IQueryable GetAll()
        {
            return _repository.GetAll();
        }

        public object GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void InsertSetting(object entity)
        {
            _repository.Insert(entity);
        }

        public void EditSetting(object entity)
        {
            _repository.Edit(entity);
        }
    }
}