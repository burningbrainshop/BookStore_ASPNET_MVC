using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;
using System.Data.Entity.Infrastructure;

namespace BookStore.Repository
{
    public class StockRepository : Repository<StockTitle>
    {
        public List<StockTitleView> GetStockTitleList()
        {
            List<StockTitle> datas = GetAll().ToList();
            List<StockTitleView> titleList = new List<StockTitleView>();
            foreach (var item in datas)
            {
                _context.Entry(item).Reference(p => p.providerInfo);                
                titleList.Add(new StockTitleView { StockId = item.StockId, PurchaseId = item.WithPurchaseId, ProviderName = item.providerInfo.Name, Amount = item.Amount, InStockedOn = item.InStockedOn });
            }
            return titleList.ToList();
        }

        public StockDetailView GetStockDetails(int stockId)
        {
            var stock = _table.Find(stockId);
            _context.Entry(stock).Reference(s => s.providerInfo);
            _context.Entry(stock).Collection(s => s.itemDetail);

            StockDetailView stockDetail = new StockDetailView();
            stockDetail.StockId = stock.StockId;
            stockDetail.PurchaseId = stock.WithPurchaseId;
            stockDetail.ProviderId = stock.ProviderId;
            stockDetail.ProviderName = stock.providerInfo.Name;
            stockDetail.Amount = stock.Amount;
            stockDetail.InStockedOn = stock.InStockedOn;
            foreach(var item in stock.itemDetail)
            {
                _context.Entry(item).Reference(p => p.productInfo);
                stockDetail.productItems.Add(new ProductItemView { Id = item.Id, ProductId = item.ProductId, ProductName = item.productInfo.Name, Cost = item.Cost, Quantity = item.Quantity, ReturnQuantity = item.ReturnQuantity });
            }
            return stockDetail;
        }

        public int GetDiscount(int stockId)
        {
            var stock = GetById(stockId);
            _context.Entry(stock).Reference(p => p.providerInfo).Load();
            return stock.providerInfo.Discount;
        }
    }
}