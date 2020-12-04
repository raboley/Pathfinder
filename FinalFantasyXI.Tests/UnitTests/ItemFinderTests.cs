using System.Collections.Generic;
using FinalFantasyXI.ItemFinder;
using FinalFantasyXI.Soul;
using Xunit;

namespace FinalFantasyXI.Tests.UnitTests
{
    public class ItemFinderTests
    {
        [Fact]
        public void GoGetItemReturnsCraftingObjective()
        {
            var want = new Objective {Name = "CraftLumber"};
            var recipes = new List<CraftingRecipe>()
            {
                new CraftingRecipe
                {
                    Name = "Lumber",
                }
            };

            var itemFinder = new ItemFinder.Craft {CraftingRecipes = recipes};

            var got = itemFinder.GoGetItem("Lumber", 1);

            Assert.Equal(want, got);
        }
    }
}