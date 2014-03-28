using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.Models
{
    public static class Extensions
    {
        public static void RunDailyUpdate(this Item item)
        {
            var currentName = item.Name;
            int decrementor = 1;
            int maxWorth = 50;

            if (item.Name.StartsWith(Constants.ALCHEMY))
            {
                currentName = currentName.Replace(Constants.ALCHEMY, string.Empty).Trim();
                ++decrementor;
                maxWorth *= 2;
            }

            switch (currentName)
            {
                case Constants.GOLD:
                    if (item.Worth < maxWorth)
                        item.Worth++;
                    break;
                case Constants.HELIUM:
                    if (item.ShelfLife > 5 && item.Worth < maxWorth)
                        item.Worth += 2;
                    else if (item.ShelfLife > 0 && item.ShelfLife <= 5 && item.Worth < maxWorth)
                        item.Worth += 3;
                    else if (item.ShelfLife <= 0)
                        item.Worth = 0;
                    break;
                case Constants.CADMIUM:
                    break;
                default:
                    if (item.Worth > 0 && item.ShelfLife > 0)
                        item.Worth -= decrementor;
                    else if (item.Worth > 0 && item.ShelfLife <= 0)
                        item.Worth -= (decrementor*2);
                    break;
            }

            item.ShelfLife--;
        }

        public static void UpdateShelfLife(this Item item)
        {
            
        }

        public static bool IsAlchemy(this Item item)
        {
            if (item.Name.StartsWith("Alchemy"))
                return true;

            return false;
        }
    }
}
