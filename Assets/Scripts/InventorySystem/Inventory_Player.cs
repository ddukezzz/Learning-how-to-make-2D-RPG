using System;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    public event Action<int> OnQuickSlotUsed;
    public int gold = 10000;
    
    public List<Inventory_EquipmentSlot> equipList;
    public Inventory_Storage storage { get; private set; }

    [Header("Quick Item Slots")] 
    public Inventory_Item[] quickItems = new Inventory_Item[2];

    protected override void Awake()
    {
        base.Awake();
        storage = FindFirstObjectByType<Inventory_Storage>();
    }

    public void SetQuickItemInSlot(int slotNumber, Inventory_Item itemToSet)
    {
        quickItems[slotNumber - 1] = itemToSet;
        TriggerUpdateUI();
    }

    public void TryUseQuickItemInSlot(int passedSlotNumber)
    {
        int slotNumber = passedSlotNumber - 1;
        var itemToUse = quickItems[slotNumber];

        if (itemToUse == null) return;
        
        TryUseItem(itemToUse);

        if (FindItem(itemToUse) == null)
        {
            quickItems[slotNumber] = FindSameItem(itemToUse);
        }
        
        TriggerUpdateUI();
        OnQuickSlotUsed?.Invoke(slotNumber);
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item);
        var matchingSlots = equipList.FindAll(slot => slot.slotType == item.itemData.itemType);
        
        // Try to find Empty Slot and equip Item
        foreach (var slot in matchingSlots)
        {
            if (slot.HasItem() == false)
            {
                EquipItem(inventoryItem, slot);
                return;
            }
        }
        
        // No Empty Slot? Replace First one
        var slotToReplace = matchingSlots[0];
        var itemToUnequip = slotToReplace.equippedItem;
        
        UnequipItem(itemToUnequip, slotToReplace != null);
        EquipItem(inventoryItem, slotToReplace);
    }

    private void EquipItem(Inventory_Item itemToEquip, Inventory_EquipmentSlot slot)
    {
        float savedHealthPercent = player.health.GetHealthPercent();
        
        slot.equippedItem = itemToEquip;
        slot.equippedItem.AddModifiers(player.stats);
        slot.equippedItem.AddItemEffect(player);
        
        player.health.SetHealthToPercent(savedHealthPercent);
        RemoveOneItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem(itemToUnequip) == false && replacingItem == false)
        {
            Debug.Log("No Space!");
            return;
        }
        
        float savedHealthPercent = player.health.GetHealthPercent();

        var slotToUnequip = equipList.Find(slot => slot.equippedItem == itemToUnequip);
        if (slotToUnequip != null) slotToUnequip.equippedItem = null;
        
        itemToUnequip.RemoveModifiers(player.stats);
        itemToUnequip.RemoveItemEffect();
        
        player.health.SetHealthToPercent(savedHealthPercent);
        AddItem(itemToUnequip);
    }
}
