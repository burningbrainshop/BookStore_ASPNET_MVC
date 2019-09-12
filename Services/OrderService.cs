using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Repository;
using BookStore.Models;
using BookStore.ViewModels;

namespace BookStore.Services
{
    public class OrderService
    {
        private OrderRepository _titleRepository;
        private Repository<OrderDetail> _detailRepository;
        private ProductRepository _productRepository;

        public OrderService()
        {
            _titleRepository = new OrderRepository();
            _detailRepository = new Repository<OrderDetail>();
            _productRepository = new ProductRepository();
        }

        public int InsertOrder(List<Cart> entities, Member member)
        {
            try
            {
                Order orderTitle = new Order();
                orderTitle.MemberId = member.MemberId;
                orderTitle.StatusId = 1;
                orderTitle.CreatedOn = DateTime.Now;
                _titleRepository.Insert(orderTitle);

                int amount = 0;
                foreach (var item in entities)
                {
                    try
                    {
                        OrderDetail orderDetail = new OrderDetail();
                        orderDetail.OrderId = orderTitle.OrderId;
                        orderDetail.ProductId = item.productCart.ProductId;
                        orderDetail.Price = item.productCart.Price;
                        orderDetail.Quantity = item.quantity;
                        _detailRepository.Insert(orderDetail);

                        amount += item.quantity * item.productCart.Price;

                        UpdateOrderAmount(orderTitle.OrderId, amount);
                        UpdateProductStock(item);
                    }
                    catch (Exception ex)
                    {
                        break;
                    }
                }
                return orderTitle.OrderId;
            }
            catch(Exception ex)
            {
                return 0;
            }
        }

        public void UpdateOrderAmount(int orderId, int amount)
        {
            var order = _titleRepository.GetById(orderId);
            order.Amount = amount;
            _titleRepository.Edit(order);
        }

        public void UpdateProductStock(Cart cart)
        {
            var data = _productRepository.GetById(cart.productCart.ProductId);
            data.Stock -= cart.quantity;
            data.SaledQuantity += cart.quantity;
            _productRepository.Edit(data);
        }

        public List<OrderTitleView> GetOrderTitleList(int periodTimeValue, int memberId = 0)
        {
            var datas = _titleRepository.GetOrderTitleList(memberId, periodTimeValue);
            return datas;
        }

        public void OrderStatusEdit(int orderId, int statusId)
        {
            var entity = _titleRepository.GetById(orderId);
            entity.StatusId = statusId;
            _titleRepository.Edit(entity);
        }

        public OrderDetailsView GetOrderDetails(int orderId)
        {
            var data = _titleRepository.GetOrderDetails(orderId);
            return data;
        }
    }
}