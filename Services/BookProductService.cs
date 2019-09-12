using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Services
{
    public class BookProductService
    {
        private Repository<CategorySetting> _categorySettingRepository;
        private ProductRepository _productRepository;
        private NextBuyRepository _nextbuyRepository;

        public BookProductService()
        {
            _categorySettingRepository = new Repository<CategorySetting>();
            _productRepository = new ProductRepository();
            _nextbuyRepository = new NextBuyRepository();
        }

        public List<CategorySetting> GetCategory()
        {
            return _categorySettingRepository.GetAll().ToList();
        }

        public List<Product> ProductsListByCategory(int categoryId)
        {
            return _productRepository.ProductsListByCategory(categoryId).ToList();
        }

        public Product GetProduct(int productId)
        {
            return _productRepository.GetById(productId);
        }

        public List<NextBuyView> GetNextBuyProducts(int memberId)
        {
            return _nextbuyRepository.GetNextBuyProducts(memberId).ToList();
        }

        public void InsertNextBuyProducts(int memberId, int productId)
        {
            var data = _nextbuyRepository.SearchFor(n => n.MemberId == memberId && n.ProductId == productId).FirstOrDefault();
            if (data == null)
            {
                NextBuy nextBuy = new NextBuy();
                nextBuy.MemberId = memberId;
                nextBuy.ProductId = productId;
                nextBuy.CreatedOn = DateTime.Now;
                _nextbuyRepository.Insert(nextBuy);
            }
        }

        public void DeleteNextBuyProducts(int memberId, int productId)
        {
            var entity = _nextbuyRepository.SearchFor(n => n.MemberId == memberId && n.ProductId == productId).FirstOrDefault();
            _nextbuyRepository.Delete(entity);
        }
    }
}