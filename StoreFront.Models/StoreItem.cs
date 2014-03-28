using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.Models
{
    public class StoreItem : Item
    {
        public bool IsAlchemy
        {
            get
            {
                if (this.Name.StartsWith("Alchemy"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
    }
}
