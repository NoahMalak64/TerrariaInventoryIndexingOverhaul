using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace InventoryIndexingOverhaulFinal.Content.Items
{
    //need to draw aggregate item num
    //need to allow for player interaction
    public class CustomInventorySlots : UIElement
    {
        public const float SCALEMULT = 1.5f;
        private int slotIndex;
        

        public CustomInventorySlots(int slotIndex)
        {
            this.slotIndex = slotIndex;
            float size = 54f* 1.5f * Main.inventoryScale;
            Width.Set(size, 0f);
            Height.Set(size, 0f);
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            ModdedPlayer moddedPlayer = Main.LocalPlayer.GetModPlayer<ModdedPlayer>();
            Item item = moddedPlayer.customInventory[slotIndex].stacks[0].Clone();












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
           
            if(IsMouseHovering && !item.IsAir)//what is ismousehovering
            {
                Main.HoverItem = item.Clone();
                Main.hoverItemName = item.Name;
            }

        }
        //OnLeftClick calls LeftClick
        public override void LeftClick(UIMouseEvent ev)
        {
            
        }

        public override void RightClick(UIMouseEvent ev)
        {
           
        }

        
    }

}