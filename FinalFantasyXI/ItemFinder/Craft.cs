using System.Collections.Generic;
using FinalFantasyXI.Soul;

namespace FinalFantasyXI.ItemFinder
{
    public class Craft
    {
        public List<CraftingRecipe> CraftingRecipes { get; set; }

        public Objective GoGetItem(string itemName, int quantity)
        {
            // Is it in my inventory current inventory
            // return
            // Is it in any storage place
            // Go get from storage
            // Can I craft it?
            var recipe = CraftingRecipes.FindAll(x => x.Name == itemName);
            if (recipe.Count > 0)
            {
                return new Objective {Name = "CraftLumber"};
                // Do I have crafting materials?
                // Go get crafting materials
            }
            // Can I buy it from npc?
            // Go to NPC and buy the item.

            return new Objective();
        }
    }

    public class CraftingRecipe
    {
        public string Name { get; set; }
    }
}