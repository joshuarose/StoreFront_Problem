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
            return Items.Where(i => i.Name.StartsWith("Alchemy")).ToList();
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
            for (var i = 0; i < Items.Count; i++)
            {
                int decrementor = 1;
                int maxWorth = 50;
                bool alchemyItem = false;

                if (Items[i].Name.StartsWith("Alchemy"))
                {
                    alchemyItem = true;
                    decrementor = 2;
                    maxWorth = 100;
                    Items[i].Name = Items[i].Name.Replace("Alchemy", "").Trim();
                }

                if (Items[i].Name != "Gold" && Items[i].Name != "Helium")
                {
                    if (Items[i].Worth > 0)
                    {
                        if (Items[i].Name != "Cadmium")
                        {
                            Items[i].Worth = Items[i].Worth - decrementor;
                        }
                    }
                }
                else
                {
                    if (Items[i].Worth < maxWorth)
                    {
                        Items[i].Worth = Items[i].Worth + 1;

                        if (Items[i].Name == "Helium")
                        {
                            if (Items[i].ShelfLife < 11)
                            {
                                if (Items[i].Worth < maxWorth)
                                {
                                    Items[i].Worth = Items[i].Worth + 1;
                                }
                            }

                            if (Items[i].ShelfLife < 6)
                            {
                                if (Items[i].Worth < maxWorth)
                                {
                                    Items[i].Worth = Items[i].Worth + 1;
                                }
                            }
                        }
                    }
                }

                if (Items[i].Name != "Cadmium")
                {
                    Items[i].ShelfLife = Items[i].ShelfLife - 1;
                }

                if (Items[i].ShelfLife < 0)
                {
                    if (Items[i].Name != "Gold")
                    {
                        if (Items[i].Name != "Helium")
                        {
                            if (Items[i].Worth > 0)
                            {
                                if (Items[i].Name != "Cadmium")
                                {
                                    Items[i].Worth = Items[i].Worth - decrementor;
                                }
                            }
                        }
                        else
                        {
                            Items[i].Worth = Items[i].Worth - Items[i].Worth;
                        }
                    }
                    else
                    {
                        if (Items[i].Worth < maxWorth)
                        {
                            Items[i].Worth = Items[i].Worth + 1;
                        }
                    }
                }
            }
        }
    }
}
