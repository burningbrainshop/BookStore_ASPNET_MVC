using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Services
{
    public class PurchaseService
    {
        private PurchaseRepository _titleRepository;
        private Repository<PurchaseDetail> _detailRepository;
        private ProductRepository _productRepository;
        private Repository<StockTitle> _stockRepository;

        public PurchaseService()
        {
            _titleRepository = new PurchaseRepository();
            _detailRepository = new Repository<PurchaseDetail>();
            _productRepository = new ProductRepository();
            _stockRepository = new Repository<StockTitle>();
        }

        public List<PurchaseTitleView> GetTitleList()
        {
            return _titleRepository.GetPurchaseTitleList();
        }

        public void InsertPurchaseTitle(PurchaseTitle entity)
        {
            entity.CreatedOn = DateTime.Now;
            _titleRepository.Insert(entity);
        }

        public void EditPurchaseTitle(PurchaseTitle entity)
        {
            _titleRepository.Edit(entity);
        }

        public bool DeletePurchase(int purchaseId)
        {
            var data = _stockRepository.SearchFor(s => s.WithPurchaseId == purchaseId).FirstOrDefault();
            if (data == null)
            {
                var entity = _titleRepository.GetById(purchaseId);
                _titleRepository.Delete(entity);
                return true;
            }
            else
                return false;    
        }

        public PurchaseDetailView GetPurchaseDetails(int purchaseId)
        {
            var data = _titleRepository.GetPurchaseDetails(purchaseId);
            return data;
        }

        public List<PurchaseDetail> GetDetails(int purchaseId)
        {
            return _detailRepository.SearchFor(p => p.PurchaseId == purchaseId).ToList();
        }

        public void InsertItems(int purchaseId, int productId, int cost, int quantity)
        {
            PurchaseDetail detail = new PurchaseDetail();
            detail.PurchaseId = purchaseId;
            detail.ProductId = productId;
            detail.Cost = cost;
            detail.Quantity = quantity;
            _detailRepository.Insert(detail);
            ChangeAmount(purchaseId);
        }

        public void DeleteItems(int purchaseDetailId)
        {
            var entity = _detailRepository.GetById(purchaseDetailId);
            _detailRepository.Delete(entity);
            ChangeAmount(entity.PurchaseId);
        }

        public void EditItems(int purchaseDetailId, int cost, int quantity)
        {
            var entity = _detailRepository.GetById(purchaseDetailId);
            entity.Cost = cost;
            entity.Quantity = quantity;
            _detailRepository.Edit(entity);
            ChangeAmount(entity.PurchaseId);
        }

        public void ChangeAmount(int purchaseId)
        {
            int discount = GetDiscount(purchaseId);
            int amount = CountAmount(purchaseId);
            amount = (amount * (100 - discount)) / 100;

            PurchaseTitle title = _titleRepository.GetById(purchaseId);
            title.Amount = amount;
            _titleRepository.Edit(title);
        }

        public int CountAmount(int purchaseId)
        {
            int amount = 0;
            var results = _detailRepository.SearchFor(p => p.PurchaseId == purchaseId).Select(p => new { ProductAmount = (p.Cost * p.Quantity) });
            foreach(var item in results)
            {
                amount += item.ProductAmount;
            }
            return amount;
        }

        public int GetDiscount(int purchaseId)
        {
            int discount = _titleRepository.GetDiscount(purchaseId);
            return discount;
        }
    }
}