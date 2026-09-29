
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;

//set namespace to our current folder
namespace InventoryIndexingOverhaulFinal.Content.Items
{
    //displays the customInventoryUI
    //keeps track of what search modifiers there currently on
    //also keeps a list of the itemtypes(ids) that are a result of the modifiers
    public class CustomInventoryUI : UIState
    {
        private ItemCategory currentCategory;
        private int Rarity;
        private List<Item.type> displayItems;
        public override void OnInitialize()
        {
            //10 columns
            int columns = 10;
            float scale = Main.inventoryScale;
            float slotSize = 54f*1.5f*scale;
            float padding = 4f*scale;
            float box = slotSize+padding;

            float leftOffset = 20f + (10*box);
            float topOffset = 10f;

            //draw icons

            //20 frames to draw
            for(int i = 0; i < displayItems.Count; i++)
            {
                //find what row to draw on
                int row = i / columns;
                //draw in remaining column
                int col = i % columns;

                //changed to slot as player instance unavailable at mod runtime i guess
                CraftingWindowSlot slot = new CraftingWindowSlot(i);

                //set its positioning
                slot.Left.Set(leftOffset + col * box, 0f);
                slot.Top.Set(topOffset + row * box, 0f);
                //add it 
                Append(slot);
            }
            
        }

        private static readonly Dictionary<int, ItemCategory> _categoryCache = new();

        public static ItemCategory GetItemCategory(int itemType)
        {
            if (itemType <= ItemID.None) return ItemCategory.Misc;

            if (_categoryCache.TryGetValue(itemType, out var cached))
                return cached;

            // ContentSamples has a live-loaded Item per type, cheap after the first load
            Item sample = ContentSamples.ItemsByType[itemType];
            ItemCategory category = ClassifyItem(sample);
            _categoryCache[itemType] = category;
            return category;
        }
        public enum ItemCategory 
        {
            Weapon, Tool, Armor, Accessory, Ore, Bar, Block, Potion, Misc
        }

        public static ItemCategory ClassifyItem(Item item)
        {
            // Tools
            if (item.pick > 0 || item.axe > 0 || item.hammer > 0)
                return ItemCategory.Tool;

            // Weapons
            if (item.damage > 0 && item.useStyle > 0)
                return ItemCategory.Weapon;

            // Armor
            if (item.headSlot >= 0 || item.bodySlot >= 0 || item.legSlot >= 0)
                return ItemCategory.Armor;

            // Accessories
            if (item.accessory)
                return ItemCategory.Accessory;

            // Potions & Buffs
            if (item.consumable && (item.buffType > 0 || item.healLife > 0 || item.healMana > 0))
                return ItemCategory.Potion;

            //Placeables (Blocks, Ores, Bars)
            if (item.createTile >= 0)
            {
                if (TileID.Sets.Ore[item.createTile])
                    return ItemCategory.Ore;

                if (item.createTile == TileID.MetalBars || item.Name.EndsWith("Bar"))
                    return ItemCategory.Bar;

                if (Main.tileSolid[item.createTile])
                    return ItemCategory.Block;
            }
            // Fallback
            return ItemCategory.Misc;
        }

        
    }
}
