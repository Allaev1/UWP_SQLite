using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataBase
{
    public class Product
    {
        public int Id { set; get; }
        public string ProductName { set; get; }
        public decimal UnitPrice { set; get; }
        public int CategoryId { set; get; }
        public bool Discontinued { set; get; }
    }
}
