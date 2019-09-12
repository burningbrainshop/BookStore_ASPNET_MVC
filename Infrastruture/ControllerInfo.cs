using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Infrastruture
{
    public enum ControllerList
    {
        CategorySetting = 0,
        ProviderSetting = 0,
        OrderStatusSetting = 0,
        Provider = 1,
        Product = 2,
        Purchase = 3,
        Stock = 3,
        Order = 4,
        Role = 5,
        Manager = 5
    }

    public enum ControllerName
    {
        類別代碼設定 = 0,
        供應商管理 = 1,
        庫存商品管理 = 2,
        採購進貨管理 = 3,
        訂單管理 = 4,
        帳戶管理 = 5
    }
}