using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess
{
    public interface IDataAccess
    {
        ItemCollection GetItems();
        void UpdateItem(Item itemIn);
        void CreateItem(Item itemIn);
        void DeleteItem(string id);
    }
}
