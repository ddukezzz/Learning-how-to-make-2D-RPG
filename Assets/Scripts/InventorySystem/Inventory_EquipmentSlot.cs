using System;
using UnityEngine;

[Serializable]
public class Inventory_EquipmentSlot
{
    public ItemType slotType;
    public Inventory_Item equippedItem;

    public Inventory_Item GetEquippedItem() => equippedItem;
    public bool HasItem() => equippedItem != null && equippedItem.itemData != null;
}
