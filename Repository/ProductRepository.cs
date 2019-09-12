using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;
using LinqKit;

namespace BookStore.Repository
{
    public class ProductRepository : Repository<Product>
    {
        public List<CategorySetting> GetCategoryByProvider(int providerId)
        {            
            IEnumerable<IGrouping<int, Product>> groups = _table.Where(p => p.ProviderId == providerId).OrderBy(p => p.CategoryId).GroupBy(p => p.CategoryId);
            IEnumerable<Product> product = groups.SelectMany(group => group);
            List<Product> list = product.ToList();

            int checkId = 0;
            List<CategorySetting> categoryList = new List<CategorySetting>();
            foreach (var item in list)
            {                
                _context.Entry(item).Reference("categoryInfo");
                if (!checkId.Equals(item.CategoryId))
                {
                    checkId = item.CategoryId;
                    categoryList.Add(new CategorySetting { CategoryId = item.CategoryId, CategoryName = item.categoryInfo.CategoryName });
                }
            }
            return categoryList;
        }

        public List<Product> GetDatasByConditions(string categoryId, string providerId, string onSale, string timeValue)
        {
            var predicate = PredicateBuilder.True<Product>();

            if (!string.IsNullOrEmpty(categoryId)) { predicate = predicate.And(p => p.CategoryId.Equals(Convert.ToInt32(categoryId))); }
            if (!string.IsNullOrEmpty(providerId)) { predicate = predicate.And(p => p.ProviderId.Equals(Convert.ToInt32(providerId))); }
            if (!string.IsNullOrEmpty(onSale)) { predicate = predicate.And(p => p.OnSale.Equals(onSale)); }
            if (!string.IsNullOrEmpty(timeValue)) {
                var sDate = GetStartDate(timeValue);
                var eDate = DateTime.Now.AddDays(1);
                predicate = predicate.And(p => p.CreatedOn >= sDate && p.CreatedOn < eDate); 
            }
            return _table.Where(predicate.Compile()).ToList();
        }

        public List<Product> GetProductsByCategory(int providerId, int categoryId)
        {
            return SearchFor(p => p.ProviderId == providerId && p.CategoryId == categoryId).ToList();
        }

        public List<Product> ProductsListByCategory(int categoryId)
        {
            return SearchFor(p => p.CategoryId == categoryId && p.OnSale == "Y").ToList();
        }

        public List<Product> GetProductByKey(string term)
        {
            return SearchFor(p => p.Name.Contains(term)).ToList();
        }

        private DateTime GetStartDate(string timeValue)
        { 
            DateTime sDate = new DateTime();
            switch(timeValue)
            {
                case "1":
                    sDate = DateTime.Now.AddMonths(-3);
                    break;
                case "2":
                    sDate = DateTime.Now.AddMonths(-6);
                    break;
                case "3":
                    sDate = DateTime.Now.AddMonths(-12);
                    break;
                case "4":
                    sDate = DateTime.Now.AddMonths(-24);
                    break;
                default:
                    sDate = DateTime.Now.AddMonths(-3);
                    break;
            }
            return sDate;
        }
    }
}