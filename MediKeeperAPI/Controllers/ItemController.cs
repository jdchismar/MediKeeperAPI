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
        private IDataAccess dao;

        public ItemController()
        {
            dao = new FlatfileDAO();
        }

        [HttpGet]
        [Route("api/v1/GetItems")]
        public ItemCollection GetItems()
        {
            return GetAllItems();
        }

        [HttpGet]
        [Route("api/v1/GetMaxItems")]
        public IActionResult GetMaxItems()
        {
            try
            {
                ItemCollection items = GetAllItems();
                ItemCollection res = new ItemCollection();

                res.ItemCol = items.ItemCol.GroupBy(item => item.ItemName)
                    .Select(group => group.OrderByDescending(groupElement => groupElement.Cost)
                    .First()).ToList();

                return Ok(res);
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        [Route("api/v1/GetMaxCostByItemName")]
        public IActionResult GetMaxCostByItemName(string ItemName)
        {
            ItemCollection items = GetAllItems();
            ItemCollection res = new ItemCollection();

            try
            {
                List<Item> lsItem = items.ItemCol.GroupBy(item => item.ItemName)
                    .Select(group => group.OrderByDescending(groupElement => groupElement.Cost)
                    .First()).ToList();

                Item i = lsItem.Where(x => x.ItemName == ItemName).FirstOrDefault();
                return Ok(i);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }            
        }

        [HttpPut]
        [Route("api/v1/Item")]
        public IActionResult Update(string id, string name, string cost)
        {
            Item item = new Item { ID = id, ItemName = name, Cost = Convert.ToDecimal(cost) };
            try
            {               
                dao.UpdateItem(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost]
        [Route("api/v1/Item")]
        public IActionResult Create(string id, string name, string cost)
        {
            Item item = new Item { ItemName = name, Cost = Convert.ToDecimal(cost) };

            try
            {
                dao.CreateItem(item);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        [Route("api/v1/Item")]
        public IActionResult Delete(string id)
        {
            try
            { 
                dao.DeleteItem(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private ItemCollection GetAllItems()
        {
            return dao.GetItems();
        }
    }
}
