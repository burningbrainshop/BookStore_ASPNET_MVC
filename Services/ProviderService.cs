using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;

namespace BookStore.Services
{
    public class ProviderService
    {
        private IRepository<Provider> _repository;

        public ProviderService()
        {
            _repository = new Repository<Provider>();
        }

        public IList<Provider> GetAll()
        {
            return _repository.GetAll().ToList();
        }

        public Provider GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void InsertProvider(Provider entity)
        {
            entity.ProviderCodeId = Convert.ToInt32(entity.ProviderCodeId);
            entity.CreatedOn = DateTime.Now;
            _repository.Insert(entity);
        }

        public void EditProvider(Provider entity)
        {
            _repository.Edit(entity);
        }
    }
}