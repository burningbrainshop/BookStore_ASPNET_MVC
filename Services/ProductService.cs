using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;

namespace BookStore.Services
{
    public class ProductService
    {
        private ProductRepository _repository;

        public ProductService()
        {
            _repository = new ProductRepository();
        }

        public List<Product> GetAll(string categoryId, string providerId, string onSale, string timeValue)
        {
            return _repository.GetDatasByConditions(categoryId, providerId, onSale, timeValue).ToList();
        }

        public Product GetById(int id)
        {
            return _repository.GetById(id);
        }

        public void InsertProduct(Product entity)
        {
            entity.CreatedOn = DateTime.Now;
            _repository.Insert(entity);
        }

        public void EditProduct(Product entity, HttpPostedFileBase imageFile, string updateImg)
        {
            if (imageFile != null)
            {
                entity.ImageType = imageFile.ContentType;
                entity.ImageData = new byte[imageFile.ContentLength];
                imageFile.InputStream.Read(entity.ImageData, 0, imageFile.ContentLength);
                _repository.Edit(entity);
            }
            else
            {
                if (!string.IsNullOrEmpty(updateImg) && updateImg.Equals("noupdate"))
                {
                    var data = _repository.GetById(entity.ProductId);
                    data.Name = entity.Name;
                    data.CategoryId = entity.CategoryId;
                    data.ProviderId = entity.ProviderId;
                    data.Price = entity.Price;
                    data.Cost = entity.Cost;
                    data.StandardStock = entity.StandardStock;
                    data.OnSale = entity.OnSale;
                    data.Description = entity.Description;
                    _repository.Edit(data);
                }                 
            }
        }

        public List<CategorySetting> GetCategoryByProvider(int providerId)
        {
            var datas = _repository.GetCategoryByProvider(providerId);
            return datas;
        }

        public List<Product> GetProductsByCategory(int providerId, int categoryId)
        {
            var datas = _repository.GetProductsByCategory(providerId, categoryId);
            return datas.ToList();
        }

        public List<Product> GetProductByKey(string term)
        {
            return _repository.GetProductByKey(term).ToList();
        }
    }
}