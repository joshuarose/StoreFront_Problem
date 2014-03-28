using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting;
using System.Text;
using System.Threading.Tasks;
using StoreFront.Models;
using NUnit.Framework;

namespace StoreFront.Tests
{
    public class Item_Tests
    {
        public Item ItemUnderTest { get; set; }
        public Store StoreUnderTest { get; set; }

        [SetUp]
        public void SetUp()
        {
            StoreUnderTest = new Store();
            ItemUnderTest = StoreUnderTest.GetFirstItem();
        }

        //All items have a ShelfLife value which denotes the number of days we have to sell the item
        [Test]
        public void Item_Has_ShelfLife()
        {
            Assert.That(ItemUnderTest, Has.Property("ShelfLife"));  
        } 
        //All items have a Worth value which denotes how valuable the item is
        [Test]
        public void Item_Has_Worth()
        {
            Assert.That(ItemUnderTest, Has.Property("Worth"));
        }
        /*
        At the end of each day our system lowers both values for every item
        Is this currently UpdateWorth? Is the business logic to run UpdateWorth at the end of the business day.
        Let's create a process end of day method to bundle up this logic
         */
        [Test]
        public void Item_Worth_Less_At_Days_End()
        {
            var firstItem = StoreUnderTest.GetFirstItem();

            var beforeItemWorth = firstItem.Worth;
            StoreUnderTest.RunEndOfDay();
            var afterItemWorth = firstItem.Worth;

            Assert.IsTrue(beforeItemWorth > afterItemWorth);
        }

        [Test]
        public void Item_ShelfLife_Less_At_Days_End()
        {
            var beforeItemWorth = ItemUnderTest.Worth;
            StoreUnderTest.RunEndOfDay();
            var afterItemWorth = ItemUnderTest.Worth;

            Assert.IsTrue(beforeItemWorth > afterItemWorth);
        }

        //Once the shelf life date has passed, Worth degrades twice as fast
        [Test]
        public void Worth_Degradation_Doubles_With_Zero_ShelfLife()
        {
            var beginWorth = ItemUnderTest.Worth;
            StoreUnderTest.RunEndOfDay();
            var afterWorth = ItemUnderTest.Worth;
            var initialDifference = beginWorth - afterWorth;
            ItemUnderTest.ShelfLife = 0;
            var beginWorthDayTwo = ItemUnderTest.Worth;
            StoreUnderTest.RunEndOfDay();
            var afterWorthDayTwo = ItemUnderTest.Worth;
            var afterDifference = beginWorthDayTwo - afterWorthDayTwo;

            Assert.AreEqual(initialDifference *2,afterDifference);
        }
        //The Worth of an item is never negative
        [Test]
        public void Worth_is_never_negative()
        {
            ItemUnderTest.Worth = 0;
            StoreUnderTest.RunEndOfDay();
            Assert.IsTrue(ItemUnderTest.Worth >= 0);
        }

        //"Gold" actually increases in Worth the older it gets
        [Test]
        public void Gold_Value_Increases_With_Time()
        {
            var gold = StoreUnderTest.GetItemByName("Gold").First();
            gold.Worth = 30;
            StoreUnderTest.RunEndOfDay();
            Assert.IsTrue(gold.Worth > 30);
        }

        //The Worth of an item is never more than 50 - except Cadmium
        [Test]
        public void Worth_Never_More_Than_Fifty()
        {
            ItemUnderTest.Worth = 50;
            StoreUnderTest.RunEndOfDay();
            Assert.IsTrue(ItemUnderTest.Worth <= 50);
        }

        //"Cadmium" is rare, has a worth of 80, and will never decrease in Worth
        //TODO: What about Alchemy Cadmium? I'm assuming that the 80 rule still applies
        [Test]
        public void Cadmium_Value_Never_Changes_From_Eighty()
        {
            var cadmium = StoreUnderTest.GetItemByName("Cadmium").First();
            StoreUnderTest.RunEndOfDay();
            Assert.AreEqual(80, cadmium.Worth);
        }

        /*
            Helium", like gold, increases in Worth as it's ShelfLife value changes; 
         *  Worth increases by 2 when there are 10 days or less and by 3 when there 
         *  are 5 days or less but Worth drops to 0 once the ShelfLife is passed.
         */

        [Test]
        public void Helium_Value_ShelfLife_Ten()
        {
            var helium = StoreUnderTest.GetItemByName("Helium").First();
            helium.Worth = 5;
            helium.ShelfLife = 10;
            StoreUnderTest.RunEndOfDay();
            Assert.AreEqual(helium.Worth, 7);
        }

        [Test]
        public void Helium_Value_ShelfLife_Five()
        {
            var helium = StoreUnderTest.GetItemByName("Helium").First();
            helium.Worth = 5;
            helium.ShelfLife = 5;
            StoreUnderTest.RunEndOfDay();
            Assert.AreEqual(helium.Worth, 8);
        }

        [Test]
        public void Helium_Value_ShelfLife_Zero()
        {
            var helium = StoreUnderTest.GetItemByName("Helium").First();
            helium.Worth = 5;
            helium.ShelfLife = 0;
            StoreUnderTest.RunEndOfDay();
            Assert.AreEqual(helium.Worth, 0);
        }

        //"Alchemy" items degrade in Worth twice as fast as normal items
        [Test]
        public void Alchemy_Items_Degrade_Twice_As_Fast()
        {
            var alchemyItem = StoreUnderTest.GetAlchemyItems().First();
            var beginWorth = alchemyItem.Worth;
            StoreUnderTest.RunEndOfDay();
            Assert.AreEqual(alchemyItem.Worth, beginWorth -2);
        }

        //"Alchemy" items have a maximum worth of 100
        [Test]
        public void Alchemy_Items_Have_Max_Worth_Of_OneHundred()
        {
            //This may be cheating as the requirements state not to change the item or items code
            //If this is the case we could overload the store constructor to take in a mock item array
            var alchemyGold = new Item() {Name = "Alchemy Gold", Worth = 50, ShelfLife = 3};
            StoreUnderTest.AddItem(alchemyGold);
            StoreUnderTest.RunEndOfDay();
            Assert.AreEqual(alchemyGold.Worth, 51);
        }
    }
}
