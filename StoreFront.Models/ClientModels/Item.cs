using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.Models
{
    //Changing the protection level of the item class in order to isolate it in the models library
    //The seperate library should isolate our unchangeable code into it's own independent library
    public class Item
    {
        public string Name { get; set; }

        public int ShelfLife { get; set; }

        public int Worth { get; set; }
    }
}
