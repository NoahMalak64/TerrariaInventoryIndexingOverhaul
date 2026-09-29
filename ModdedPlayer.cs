using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

//create 5 frames for the 10,000 frames
//total 50,000 frames

//set namespace to our current folder
namespace InventoryIndexingOverhaulFinal.Content.Items
{
    //create an instance of ModPlayer called ModdedPlayer
    public class ModdedPlayer : ModPlayer
    {
        //create inventory items
        public const int customInventoryStackCount = 5;
        public bool inventoryChanged = true;
        public struct ItemEntry
        {
            public int totalCount;
            public Item[] stacks;

            public ItemEntry(Item item)
            {
                stacks = new Item[customInventoryStackCount];
                Item i = item.Clone();
                totalCount = i.stack;
                stacks[0] = i;
                stacks[1] = new Item();
                stacks[1].TurnToAir();
                stacks[2] = new Item();
                stacks[2].TurnToAir();
                stacks[3] = new Item();
                stacks[3].TurnToAir();
                stacks[4] = new Item();
                stacks[4].TurnToAir();
            }
        }
        public Dictionary<int, ItemEntry> customInventory = new Dictionary<int, ItemEntry>();
        //first space tells us how many items/stacks are present
        public Item[] craftingWindow = new Item[20];
        //Initilaize values of array to zero
        //initiallize runs on character selection
        public override void Initialize()
        {
            customInventory = new Dictionary<int, ItemEntry>();

            for(int i = 0; i < craftingWindow.Length; i++)
            {
                craftingWindow[i] = new Item();
                craftingWindow[i].TurnToAir();
            }
        }

        public List<int> GetItemTypesForCategory(CustomInventorySlots.ItemCategory category)
        {
            List<int> result = new List<int>();
            foreach (int itemType in customInventory.Keys)
            {
                if (CustomInventorySlots.GetItemCategory(itemType) == category)
                    result.Add(itemType);
            }
            //sort if needed
            return result;
        }
        /*
        RemoveItemFromSlot(ref Item)
        Parameters
        Item: a reference to the item being removed
        removes the item and keeps structure of the inventory
        ONLY DELETES WHOLE STACKS FROM THE INVENTORY NO PARTIALS
        */
        public int RemoveItemFromSlot(ref Item item, int slot)
        {
            //check if the item exists maybe redundant?
            if (!customInventory[item.type])
            {
                return 0;
            }
            ItemEntry itemEntry = customInventory[item.type];
            //item is non stackable (can have modifiers)
            if(item.maxStack == 1)
            {
                //we must grab from the slot and move everything to fill
                //calc last stack
                int lastStack = itemEntry.totalCount/item.maxStack;
                itemEntry.stacks[slot].TurnToAir();
                itemEntry.totalCount--;
                //move everything
                for(int i = slot; i < lastStack; i++)
                {
                    itemEntry.stacks[i] = itemEntry.stacks[i+1];
                }
                customInventory[item.type] = itemEntry;
                inventoryChanged = true;
                return 1;

            }
            //item is stackable
            else
            {
                //grab from first available stack
                //redefine slot to be last slot
                slot = itemEntry.totalCount/item.maxStack;
                //stack amount is the last slot
                int stackTotal = itemEntry.stacks[slot].stack;
                itemEntry.stacks[slot].TurnToAir();
                //update total
                itemEntry.totalCount -= stackTotal;
                //if not a full stack grab from the next
                if(stackTotal < item.maxStack)
                {
                    slot--;
                    if (slot > 0)
                    {
                        itemEntry.stacks[slot].stack -= (itemEntry.stacks[slot].maxStack-stackTotal);
                        itemEntry.totalCount -= itemEntry.stacks[slot].maxStack-stackTotal;
                    }
                }
                customInventory[item.type] = itemEntry;
                inventoryChanged = true;
                return stackTotal;
            }
        }
        
        /*
        AddItemToSlot(reference Item)
        Parameters
        Item: a reference to the item being added
        Returns
        True: if all of that item could be added
        False: if not all of the item could be added
        */
        public bool AddItemToSlot(ref Item item)
        {
            //check if item is not present
            if (!customInventory.ContainsKey(item.type))
            {
                //add it to inventory
                ItemEntry newItemEntry = new ItemEntry(item);
                customInventory[item.type] = newItemEntry;
                //delete item
                item.TurnToAir();
                //return
                inventoryChanged = true;
                return true;
            }
            //if present add the item to existing stack
            //calculate next spot empty or partial stack
            ItemEntry itemEntry = customInventory[item.type];
            int slot = itemEntry.totalCount/item.maxStack; //int division to find slot
            //if all slots are full return
            if(slot == customInventoryStackCount)
            {
                return false;
            }
            inventoryChanged = true;
            //if its a single item
            if(item.maxStack == 1)
            {
                Item i = item.Clone();
                itemEntry.stacks[slot] = i; 
                itemEntry.totalCount++;
                customInventory[item.type] = itemEntry;
                item.TurnToAir();
                return true;
            }
            //if its a stackable item and there is room calc how many can be added to the current slot
            //calc if stacked will exceed limit
            int overflow = item.stack+itemEntry.stacks[slot].stack-item.maxStack;
            if(overflow > 0)
            {
                int added = itemEntry.stacks[slot].maxStack-itemEntry.stacks[slot].stack;
                //fill current slot
                itemEntry.stacks[slot].stack = item.maxStack;
                //now we only have the overflow left
                item.stack = overflow;
                //see if we can move to next slot
                slot++;
                if(slot == customInventoryStackCount)
                {
                    itemEntry.totalCount += added;
                    customInventory[item.type] = itemEntry;
                    return false;
                }
                
                //add it to the next slot
                itemEntry.stacks[slot] = item.Clone();
                itemEntry.totalCount += added+overflow;
                customInventory[item.type] = itemEntry;
                item.TurnToAir();
                return true;
            }
            else
            {
                if(itemEntry.stacks[slot].IsAir)
                {
                    itemEntry.stacks[slot] = item.Clone();
                }
                else
                {
                    itemEntry.stacks[slot].stack += item.stack;
                }
                //we can just stack them
                itemEntry.stacks[slot].stack += item.stack;
                //update totalCount
                customInventory[item.type] = itemEntry;
                item.TurnToAir();
                return true;
            }



        }
        public override bool OnPickup(Item item)
        {
            return !AddItemToSlot(ref item);
        }
    }
}