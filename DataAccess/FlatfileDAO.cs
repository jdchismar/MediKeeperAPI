using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess
{
    public class FlatfileDAO : IDataAccess
    {
        public ItemCollection GetItems()
        {
            ItemCollection res = new ItemCollection();
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(location);

            string[] lines = System.IO.File.ReadAllLines(string.Format(@"{0}\DataStore\{1}", directory, @"items.txt"));
            bool firstrow = true;

            foreach (string line in lines)
            {
                if (firstrow)
                {
                    firstrow = false;
                    continue;
                }
                    
                res.ItemCol.Add(new Item
                {
                    ID = line.Split(',')[0],
                    ItemName = line.Split(',')[1],
                    Cost = Decimal.Parse(line.Split(',')[2])
                });
            }

            return res;
        }

        public void UpdateItem(Item itemIn)
        {
            ItemCollection res = GetItems();
            res.ItemCol[res.ItemCol.FindIndex(ind => ind.ID.Equals(itemIn.ID))] = itemIn;
            WriteToFile(res);
        }

        public void CreateItem(Item itemIn)
        {
            ItemCollection res = GetItems();
            //Increment the highest Id
            int id = res.ItemCol.Select(m => Convert.ToInt32(m.ID)).Max();
            id++;
            itemIn.ID = (id).ToString().PadLeft(3, '0');

            res.ItemCol.Add(itemIn);
            WriteToFile(res);
        }

        public void DeleteItem(string id)
        {
            ItemCollection res = GetItems();
            Item itemRm = res.ItemCol.Find(r => r.ID == id);
            res.ItemCol.Remove(itemRm);
            WriteToFile(res);
        }

        private void WriteToFile(ItemCollection objIn)
        {
            var location = System.Reflection.Assembly.GetEntryAssembly().Location;
            var directory = System.IO.Path.GetDirectoryName(location);
            string path = string.Format(@"{0}\DataStore\{1}", directory, @"items.txt");

            System.IO.File.WriteAllText(path, objIn.ToString());
        }

    }
}
