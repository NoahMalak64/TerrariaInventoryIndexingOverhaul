
using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using System.Collections.Generic;

//set namespace to our current folder
namespace InventoryIndexingOverhaulFinal.Content.Items
{
    //displays the categoryUI options
    public class CategoryUI : UIState
    {
        int numRarities = 23;
        int numCategories = 9;
        int topOffset = 200;
        int leftOffset = 200;
        public enum  {
            
        }
        public override void OnInitialize()
        {
            //23 rarities + calamity
            //0-18
            float scale = Main.inventoryScale;
            float slotSize = 54f*1.5f*scale;
            float padding = 4f*scale;
            float box = slotSize+padding;

            float leftOffset = 20f + (10*box);
            float topOffset = 10f;

            //draw icons
            int columns = 10;
            int rows = 1;
            //10 buttons to draw
            for(int i = 0; i < numRarities; i++)
            {
                //find what row to draw on
                int row = i / columns;
                //draw in remaining column
                int col = i % columns;

                //changed to slot as player instance unavailable at mod runtime i guess
                CategoryButton button = new CategoryButton(i, ButtonAssets(i));

                //set its positioning
                button.Left.Set(leftOffset + col * box, 0f);
                button.Top.Set(topOffset + rows * box, 0f);
                //add it 
                Append(button);
            }
            columns = 8;
            //draw item category
            for(int i = 0; i < 10; i++)
            {
                //find what row to draw on
                int col = i%columns;
                //changed to button
                CategoryButton button = new CategoryButton(i);

                //set its positioning
                button.Left.Set(leftOffset + col * box, 0f);
                button.Top.Set(topOffset + 1 * box, 0f);
                //add it 
                Append(button);
            }
            
        }
    }
}