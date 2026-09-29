using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

//set namespace to our current folder
namespace InventoryIndexingOverhaulFinal.Content.Items
{
    public class CraftingWindowSlot : UIElement
    {
        public const float SCALEMULT = 1.5f;
        private int slotIndex;

        public CraftingWindowSlot(int slotIndex)
        {
            this.slotIndex = slotIndex;
            float size = 54f* 1.5f * Main.inventoryScale;
            Width.Set(size, 0f);
            Height.Set(size, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            ModdedPlayer moddedPlayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
            Item item = moddedPlayer.craftingWindow[slotIndex];

            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = new Vector2(dimensions.X, dimensions.Y);
            float scale = Main.inventoryScale*1.5f;
            Texture2D background = TextureAssets.InventoryBack.Value;
            spriteBatch.Draw(
                background,
                position,
                null,
                Color.White,
                0f,
                Vector2.Zero,
                scale,
                SpriteEffects.None,
                0f
            );
            if (!item.IsAir)
            {
                Main.instance.LoadItem(item.type);
                Texture2D itemTexture = TextureAssets.Item[item.type].Value;
                Vector2 center = position + new Vector2(background.Width * scale / 2f, background.Height * scale / 2f);

                spriteBatch.Draw(
                    itemTexture,
                    center,
                    null,
                    Color.White,
                    0f,
                    itemTexture.Size() / 2f,
                    scale,
                    SpriteEffects.None,
                    0f
                );
            }
           
            if(IsMouseHovering && !item.IsAir)
            {
                Main.HoverItem = item.Clone();
                Main.hoverItemName = item.Name;
            }

        }
        //OnLeftClick calls LeftClick
        public override void LeftClick(UIMouseEvent ev)
        {
            ModdedPlayer moddedPlayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
            Player player = moddedPlayer.Player;
           
            //check if we are swapping air with air
            if(Main.mouseItem.IsAir && moddedPlayer.craftingWindow[slotIndex].IsAir)
            {
                return;
            }
            //check if slot is empty
            else if(moddedPlayer.craftingWindow[slotIndex].IsAir && !Main.mouseItem.IsAir)
            {
                moddedPlayer.craftingWindow[slotIndex] = Main.mouseItem.Clone();
                Main.mouseItem = new Item();
                Main.mouseItem.TurnToAir();
                Console.WriteLine("test");
            }
            //if mouseItem is empty
            else if (Main.mouseItem.IsAir && !moddedPlayer.craftingWindow[slotIndex].IsAir)
            {
                Main.mouseItem = moddedPlayer.craftingWindow[slotIndex].Clone();
                moddedPlayer.craftingWindow[slotIndex] = new Item();
            }
            //if both have items
            else
            {
                //check if they are the same item and is stackable
                if ((Main.mouseItem.type == moddedPlayer.craftingWindow[slotIndex].type) && moddedPlayer.craftingWindow[slotIndex].maxStack > 1)
                {
                    int stack = Main.mouseItem.stack + moddedPlayer.craftingWindow[slotIndex].stack;
                    if(stack > moddedPlayer.craftingWindow[slotIndex].maxStack)
                    {
                        //if either has fullstack swap items
                        if(Main.mouseItem.stack == moddedPlayer.craftingWindow[slotIndex].maxStack || moddedPlayer.craftingWindow[slotIndex].stack == moddedPlayer.craftingWindow[slotIndex].maxStack)
                        {
                            //swap
                            Item cursorItem = Main.mouseItem;
                            Main.mouseItem = moddedPlayer.craftingWindow[slotIndex];
                            moddedPlayer.craftingWindow[slotIndex] = cursorItem;
                        }
                        //no fullstacks
                        else
                        {
                            //calculate how much is left in cursor
                            stack = stack - moddedPlayer.craftingWindow[slotIndex].maxStack;
                            //max out inventory stack
                            moddedPlayer.craftingWindow[slotIndex].stack = moddedPlayer.craftingWindow[slotIndex].maxStack;  
                            //set mouse to be the remainder
                            Main.mouseItem.stack = stack;                      
                        }
                    }
                    //if it fits, just stackem
                    else
                    {
                        moddedPlayer.craftingWindow[slotIndex].stack = stack;
                        Main.mouseItem = new Item();
                    }
                }
                else
                {
                    //swap with cursorItem
                    Item cursorItem = Main.mouseItem;
                    Main.mouseItem = moddedPlayer.craftingWindow[slotIndex];
                    moddedPlayer.craftingWindow[slotIndex] = cursorItem;
                }
            }
            player.inventory[58] = Main.mouseItem;
            Recipe.FindRecipes();

        }

        public override void RightClick(UIMouseEvent ev)
        {
            ModdedPlayer moddedPlayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
            Player player = moddedPlayer.Player;
            // both air — do nothing
            if (moddedPlayer.craftingWindow[slotIndex].IsAir && Main.mouseItem.IsAir)
            {
                return;
            }
            // slot empty, cursor has item — place it
            else if (moddedPlayer.craftingWindow[slotIndex].IsAir)
            {
                moddedPlayer.craftingWindow[slotIndex] = Main.mouseItem.Clone();
                Main.mouseItem = new Item();
            }
            // slot has item, cursor empty — grab one
            else if (Main.mouseItem.IsAir)
            {
                Main.mouseItem = moddedPlayer.craftingWindow[slotIndex].Clone();
                Main.mouseItem.stack = 1;
                moddedPlayer.craftingWindow[slotIndex].stack--;
                if (moddedPlayer.craftingWindow[slotIndex].stack == 0)
                {
                    moddedPlayer.craftingWindow[slotIndex] = new Item();
                }
            }
            // both have items
            else
            {
                // same item type and stackable — take one
                if (moddedPlayer.craftingWindow[slotIndex].type == Main.mouseItem.type && moddedPlayer.craftingWindow[slotIndex].maxStack > 1)
                {
                    if (Main.mouseItem.stack + 1 <= Main.mouseItem.maxStack)
                    {
                        Main.mouseItem.stack++;
                        moddedPlayer.craftingWindow[slotIndex].stack--;
                        if (moddedPlayer.craftingWindow[slotIndex].stack == 0)
                        {
                            moddedPlayer.craftingWindow[slotIndex] = new Item();
                        }
                    }
                }
                // different items or non-stackable — swap
                else
                {
                    Item temp = Main.mouseItem;
                    Main.mouseItem = moddedPlayer.craftingWindow[slotIndex];
                    moddedPlayer.craftingWindow[slotIndex] = temp;
                }
            }
            player.inventory[58] = Main.mouseItem;
            Recipe.FindRecipes();
        }
        public override void MouseOver(UIMouseEvent ev)
        {
            ModdedPlayer moddedPlayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
            
        }
        public override void MouseOut(UIMouseEvent ev)
        {
            ModdedPlayer moddedPlayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
        }
        
    }
}