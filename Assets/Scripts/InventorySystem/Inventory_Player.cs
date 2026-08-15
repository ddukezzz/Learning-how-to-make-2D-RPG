using System.Collections.Generic;
using UnityEngine;

public class Inventory_Player : Inventory_Base
{
    private Player player;
    public List<Inventory_EquipmentSlot> equipList;

    protected override void Awake()
    {
        base.Awake();
        player = GetComponent<Player>();
    }

    public void TryEquipItem(Inventory_Item item)
    {
        var inventoryItem = FindItem(item.itemData);
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
        RemoveItem(itemToEquip);
    }

    public void UnequipItem(Inventory_Item itemToUnequip, bool replacingItem = false)
    {
        if (CanAddItem() == false && replacingItem == false)
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
