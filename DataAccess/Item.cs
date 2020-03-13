using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public class Item
    {
        public string ID { get; set; }
        public string ItemName { get; set; }
        public decimal Cost { get; set; }
    }

    public class ItemCollection
    {
        public ItemCollection()
        {
            ItemCol = new List<Item>();
        }
        public List<Item> ItemCol { get; set; }

        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("ID,ITEM NAME, COST");

            foreach (Item i in ItemCol)
            {
                sb.AppendLine(string.Join(',', i.ID, i.ItemName, i.Cost.ToString()));
            }

            return sb.ToString();
        }
    }
}