using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Repository
{
    public class NextBuyRepository : Repository<NextBuy>
    {
        public List<NextBuyView> GetNextBuyProducts(int memberId)
        {
            var datas = SearchFor(n => n.MemberId == memberId).ToList();
            List<NextBuyView> nextList = new List<NextBuyView>();
            foreach(var item in datas)
            {
                _context.Entry(item).Reference(n => n.productInfo);
                nextList.Add(new NextBuyView { nextId = item.NextId, productId = item.productInfo.ProductId, productName = item.productInfo.Name, productPrice = item.productInfo.Price });
            }
            return nextList.ToList();
        }
    }
}