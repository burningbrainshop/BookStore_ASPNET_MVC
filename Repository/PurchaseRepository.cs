using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Repository
{
    public class PurchaseRepository : Repository<PurchaseTitle>
    {
        public List<PurchaseTitleView> GetPurchaseTitleList()
        {           
            List<PurchaseTitle> datas = GetAll().ToList();
            List<PurchaseTitleView> titleList = new List<PurchaseTitleView>();
            foreach(var item in datas)
            {
                string flag = "N";
                _context.Entry(item).Reference(p => p.providerInfo);
                _context.Entry(item).Collection(s => s.stockInfo);
                
                foreach(var each in item.stockInfo)
                {
                    if (each != null)
                    {
                        if (each.StockId != 0 && each.StockId.ToString() != "0")
                            flag = each.StockId.ToString();
                    }
                }
                titleList.Add(new PurchaseTitleView { PurchaseId = item.PurchaseId, ProviderId = item.ProviderId, ProviderName = item.providerInfo.Name, Amount = item.Amount, StockedOn = item.StockedOn, CreatedOn = item.CreatedOn, StockCheck = flag });
            }
            return titleList.ToList();
        }

        public PurchaseDetailView GetPurchaseDetails(int purchaseId)
        {
            var purchase = _table.Find(purchaseId);
            _context.Entry(purchase).Reference(p => p.providerInfo);
            _context.Entry(purchase).Collection(pd => pd.itemDetail);

            PurchaseDetailView purchaseDetail = new PurchaseDetailView();
            purchaseDetail.PurchaseId = purchase.PurchaseId;
            purchaseDetail.ProviderId = purchase.ProviderId;
            purchaseDetail.ProviderName = purchase.providerInfo.Name;
            purchaseDetail.Amount = purchase.Amount;
            purchaseDetail.StockedOn = purchase.StockedOn;
            foreach(var item in purchase.itemDetail)
            {
                _context.Entry(item).Reference(x => x.productInfo);
                purchaseDetail.productItems.Add(new ProductItemView { Id = item.Id, ProductId = item.ProductId, ProductName = item.productInfo.Name, Cost = item.Cost, Quantity = item.Quantity, ReturnQuantity = 0 });
            }
            return purchaseDetail;
        }

        public int GetDiscount(int purchaseId)
        {
            var purchase = GetById(purchaseId);
            _context.Entry(purchase).Reference(p => p.providerInfo).Load();
            return purchase.providerInfo.Discount;
        }
    }
}