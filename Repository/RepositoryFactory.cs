using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using BookStore.Models;

namespace BookStore.Repository
{
    public class RepositoryFactory
    {
        private static IRepository _repositoy;

        public static IRepository GetRepository(string tableName)
        {
            switch (tableName)
            {
                case "CategorySetting":
                    _repositoy = new Repository<CategorySetting>();
                    break;
                case "ProviderSetting":
                    _repositoy = new Repository<ProviderSetting>();
                    break;
                case "OrderStatusSetting":
                    _repositoy = new Repository<OrderStatusSetting>();
                    break;
                default:
                    _repositoy = null;
                    break;
            }

            return _repositoy;
        }
    }
}