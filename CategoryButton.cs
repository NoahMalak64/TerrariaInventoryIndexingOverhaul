using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Terraria.GameContent;
using Terraria.ID;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using ReLogic.Content;

//set namespace to our current folder
namespace InventoryIndexingOverhaulFinal.Content.Items
{
    //the UI element for each category button
    public class CategoryButton : UIElement
    {
        public const float SCALEMULT = 1.5f;
        private int slotIndex;
        private readonly CustomInventoryUI.ItemCategory category;
        private Asset<Texture2D> icon;

        public CategoryButton(ItemCategory c, Asset<Texture2D> i)
        {
            category = c;
            icon = i;
        
        
            float size = 54f* 1.5f * Main.inventoryScale;
            Width.Set(size, 0f);
            Height.Set(size, 0f);

            icon = ModContent.Request<Texture2D>(
                "InventoryIndexingOverhaulFinal/Assets/weapons",
                AssetRequestMode.ImmediateLoad
            );
        }

        protected override void DrawSelf(SpriteBatch spriteBatch)
        {
            CalculatedStyle dimensions = GetDimensions();
            Vector2 position = new Vector2(dimensions.X, dimensions.Y);
            float scale = Main.inventoryScale*1.5f;
            Texture2D iconTexture = icon.Value;
            Vector2 center = position + new Vector2(dimensions.Width/2f, dimensions.Height/2f);

            spriteBatch.Draw(
                iconTexture,
                center,
                null,
                Color.White,
                0f,
                iconTexture.Size()/2f,
                scale,
                SpriteEffects.None,
                0f
            );

        }
        //
        public override void LeftClick(UIMouseEvent ev)
        {
            
        }

        public override void RightClick(UIMouseEvent ev)
        {
        
        }
        public override void MouseOver(UIMouseEvent ev)
        {
            
        }
        public override void MouseOut(UIMouseEvent ev)
        {
            
        }
        
    }
}