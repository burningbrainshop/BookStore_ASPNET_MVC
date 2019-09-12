using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Services
{
    public class StockService
    {
        private PurchaseRepository _purchaseRepository;
        private StockRepository _titleRepository;
        private Repository<StockDetail> _detailRepository;
        private ProductRepository _productRepository;

        public StockService()
        {
            _titleRepository = new StockRepository();
            _purchaseRepository = new PurchaseRepository();
            _detailRepository = new Repository<StockDetail>();
            _productRepository = new ProductRepository();
        }

        public List<StockTitleView> GetTitleList()
        {
            return _titleRepository.GetStockTitleList();
        }

        public void EditStockTitle(StockTitle entity)
        {
            _titleRepository.Edit(entity);
        }

        public StockDetailView GetStockDetails(int stockId)
        {
            var data = _titleRepository.GetStockDetails(stockId);
            return data;
        }

        public void DeleteStock(int stockId)
        {
            var entity = _titleRepository.GetById(stockId);
            var entitis = _detailRepository.SearchFor(s => s.StockId == stockId);
            foreach(var item in entitis.ToList())
            {
                int oldQuantity = GetOldQuantity(item);
                int oldReturnQuantity = GetOldReturnQuantity(item);
                UpdateProductQuantity(item, (oldQuantity - oldReturnQuantity) * 2);
            }            
            _titleRepository.Delete(entity);
        }

        public void EditItems(int stockDetailId, int quantity, int returnQuantity)
        {
            var entity = _detailRepository.GetById(stockDetailId);
            int oldQuantity = GetOldQuantity(entity);
            int oldReturnQuantity = GetOldReturnQuantity(entity);
            entity.Quantity = quantity;
            entity.ReturnQuantity = returnQuantity;
            _detailRepository.Edit(entity);
            ChangeAmount(entity.StockId);
            UpdateProductQuantity(entity, (oldQuantity - oldReturnQuantity));
        }

        public void ChangeAmount(int stockId)
        {
            int discount = GetDiscount(stockId);
            int amount = CountAmount(stockId);
            amount = (amount * (100 - discount)) / 100;

            StockTitle title = _titleRepository.GetById(stockId);
            title.Amount = amount;
            _titleRepository.Edit(title);
        }

        public int CountAmount(int stockId)
        {
            int amount = 0;
            var results = _detailRepository.SearchFor(p => p.StockId == stockId).Select(p => new { ProductAmount = (p.Cost * (p.Quantity - p.ReturnQuantity)) });
            foreach (var item in results)
            {
                amount += item.ProductAmount;
            }
            return amount;
        }

        public int GetDiscount(int stockId)
        {
            int discount = _titleRepository.GetDiscount(stockId);
            return discount;
        }

        public void ConvertPurchaseToStock(int purchaseId)
        {
            PurchaseDetailView purchaseDetails = _purchaseRepository.GetPurchaseDetails(purchaseId);
            StockTitle title = new StockTitle();
            title.WithPurchaseId = purchaseDetails.PurchaseId;
            title.ProviderId = purchaseDetails.ProviderId;
            title.Amount = purchaseDetails.Amount;
            title.InStockedOn = purchaseDetails.StockedOn;
            _titleRepository.Insert(title);

            foreach (var item in purchaseDetails.productItems)
            {
                StockDetail detail = new StockDetail();
                detail.StockId = title.StockId;
                detail.ProductId = item.ProductId;
                detail.Cost = item.Cost;
                detail.Quantity = item.Quantity;
                detail.ReturnQuantity = 0;
                _detailRepository.Insert(detail);

                UpdateProductQuantity(detail);
            }
        }

        public void UpdateProductQuantity(StockDetail entity, int oldQuantity = 0)
        {
            var data = _productRepository.GetById(entity.ProductId);
            data.Stock = data.Stock - oldQuantity + (entity.Quantity - entity.ReturnQuantity);
            _productRepository.Edit(data);
        }

        public int GetOldQuantity(StockDetail entity)
        {
            return _detailRepository.GetById(entity.Id) == null ? 0 : _detailRepository.GetById(entity.Id).Quantity;
        }

        public int GetOldReturnQuantity(StockDetail entity)
        {
            return _detailRepository.GetById(entity.Id) == null ? 0 : _detailRepository.GetById(entity.Id).ReturnQuantity;
        }
    }
}