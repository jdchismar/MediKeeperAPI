using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataAccess;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediKeeperAPI.Controllers
{

    [ApiController]
    public class ItemController : ControllerBase
    {
        [HttpGet]
        [Route("api/v1/GetItems")]
        public ItemCollection GetItems()
        {
            return GetAllItems();
        }

        [HttpGet]
        [Route("api/v1/GetMaxItems")]
        public ItemCollection GetMaxItems()
        {
            ItemCollection items = GetAllItems();
            ItemCollection res = new ItemCollection();

            res.ItemCol = items.ItemCol.GroupBy(item => item.ItemName)
                .Select(group => group.OrderByDescending(groupElement => groupElement.Cost)
                .First()).ToList();

            return res;
        }

        [HttpGet]
        [Route("api/v1/GetMaxCostByItemName")]
        public Item GetMaxCostByItemName(string ItemName)
        {
            ItemCollection items = GetAllItems();
            ItemCollection res = new ItemCollection();

            List<Item> lsItem = items.ItemCol.GroupBy(item => item.ItemName)
                .Select(group => group.OrderByDescending(groupElement => groupElement.Cost)
                .First()).ToList();
            
            Item i = lsItem.Where(x => x.ItemName == ItemName).FirstOrDefault();

            return i;
        }

        [HttpPut]
        [Route("api/v1/Item")]
        public void UpdateItem(string id, string name, string cost)
        {
            Item item = new Item { ID = id, ItemName = name, Cost = Convert.ToDecimal(cost) };

            IDataAccess dao = new FlatfileDAO();

            dao.UpdateItem(item);
        }

        [HttpPost]
        [Route("api/v1/Item")]
        public void CreateItem(string id, string name, string cost)
        {
            Item item = new Item { ID = id, ItemName = name, Cost = Convert.ToDecimal(cost) };

            IDataAccess dao = new FlatfileDAO();

            dao.CreateItem(item);
        }

        [HttpDelete]
        [Route("api/v1/Item")]
        public void DeleteItem(string id)
        {
            IDataAccess dao = new FlatfileDAO();

            dao.DeleteItem(id);
        }

        private ItemCollection GetAllItems()
        {
            IDataAccess dao = new FlatfileDAO();
            return dao.GetItems();
        }
    }
}
