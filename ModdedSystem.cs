using Terraria;
using Terraria.ModLoader;
using Terraria.UI;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

//set namespace to our current folder
namespace InventoryIndexingOverhaulFinal.Content.Items
{
    //create an instance of ModSystem called ModdedSystem
    public class ModdedSystem : ModSystem
    {
        //declare UI state
        internal static CustomInventoryUI CustomInventoryUI;
        internal static UserInterface CustomInventoryInterface;
        public override void Load()
        {
            if(!Main.dedServ)
            {
                //Instatiate it
                CustomInventoryUI = new CustomInventoryUI();
                CustomInventoryUI.Activate();
                CustomInventoryInterface = new UserInterface();
                CustomInventoryInterface.SetState(CustomInventoryUI);
            }
        }
        public override void ModifyInterfaceLayers(List<GameInterfaceLayer> UIlayers)//takes type spriteBatch for all Interface Layers
        {   
            //using Vanilla inventory hook
            //Vanilla: Inventory
            int inventoryIndex = UIlayers.FindIndex(layer => layer.Name.Equals("Vanilla: Radial Hotbars"));

            if(inventoryIndex != -1)
            {
                UIlayers.Insert(inventoryIndex + 1, new LegacyGameInterfaceLayer(
                    "InventoryIndexingOverhaul: CustomInventory",
                    delegate
                    {
                        if (Main.playerInventory)
                        {
                            CustomInventoryInterface.Draw(Main.spriteBatch, new GameTime());
                        }
                        return true;
                    },
                    InterfaceScaleType.UI
                ));
            }
        }
        public override void UpdateUI(GameTime gameT)
        {
            if (Main.playerInventory)
            {
                CustomInventoryInterface?.Update(gameT);
            }
        }
        //public override void PostDrawInterface(SpriteBatch spriteBatch)
        //{
            
        //}
    }
}