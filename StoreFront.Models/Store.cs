using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StoreFront.Models
{
    public class Store
    {
        private IList<Item> Items = new List<Item>
                                    {
                                        new Item {Name = "Aluminum Shackles", ShelfLife = 10, Worth = 20},
                                        new Item {Name = "Gold", ShelfLife = 2, Worth = 50},
                                        new Item {Name = "Plutonium Pinball Parts", ShelfLife = 5, Worth = 7},
                                        new Item {Name = "Cadmium", ShelfLife = 0, Worth = 80},
                                        new Item {Name = "Helium", ShelfLife = 15, Worth = 38},
                                        new Item {Name = "Alchemy Iron", ShelfLife = 3, Worth = 75}
                                    };

        public List<Item> GetItemByName(string name)
        {
            return Items.Where(i => i.Name == name).ToList();
        }

        public Item GetFirstItem()
        {
            return Items.FirstOrDefault();
        }

        public List<Item> GetAlchemyItems()
        {
            return Items.Where(i => i.Name.StartsWith(Constants.ALCHEMY)).ToList();
        }

        public void AddItem(Item item)
        {
            Items.Add(item);
        }

        public void RunEndOfDay()
        {
            //End of day provides a public api that will run all current and future end of day operations
            UpdateWorth();
        }

        //This takes the Items array and updates the values
        private void UpdateWorth()
        {
            foreach (var item in Items)
            {
                item.RunDailyUpdate();
            }
        }
    }
}
