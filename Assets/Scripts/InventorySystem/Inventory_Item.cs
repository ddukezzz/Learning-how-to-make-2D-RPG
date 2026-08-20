using System;
using System.Text;
using UnityEngine;

[Serializable]
public class Inventory_Item
{
    private string itemId;
    
    public ItemDataSO itemData;
    public int stackSize = 1;

    public ItemModifier[] modifiers {get; private set;}
    public ItemEffect_DataSO itemEffect;
    
    public int buyPrice {get; private set;}
    public float sellPrice {get; private set;}
    
    public Inventory_Item(ItemDataSO itemData)
    {
        this.itemData = itemData;
        itemEffect = itemData.itemEffect;
        buyPrice = itemData.itemPrice;
        sellPrice =  itemData.itemPrice * 0.7f;
        
        modifiers = EquipmentData()?.modifiers;
        itemId = itemData.itemName + " - " + Guid.NewGuid();
    }

    public void AddModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.AddModifier(mod.value, itemId);
        }
    }

    public void RemoveModifiers(Entity_Stats playerStats)
    {
        foreach (var mod in modifiers)
        {
            Stat statToModify = playerStats.GetStatByType(mod.statType);
            statToModify.RemoveModifier(itemId);
        }
    }

    public void AddItemEffect(Player player) => itemEffect?.Subscribe(player);
    
    public void RemoveItemEffect() => itemEffect?.Unsubscribe();
    
    private EquipmentDataSO EquipmentData()
    {
        if (itemData is EquipmentDataSO equipment)
        {
            return equipment;
        }

        return null;
    }
    
    public bool CanAddStack() => stackSize < itemData.maxStackSize;
    
    public void AddStack() => stackSize += 1;

    public void RemoveStack()  => stackSize -= 1;
    
    public string GetItemInfo()
    {
        StringBuilder sb = new StringBuilder();
        
        if (itemData.itemType == ItemType.Material)
        {
            sb.AppendLine("");
            sb.AppendLine("Used for Crafting!");
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }

        if (itemData.itemType == ItemType.Consumable)
        {
            sb.AppendLine("");
            sb.AppendLine(itemEffect.effectDescription);
            sb.AppendLine("");
            sb.AppendLine("");
            return sb.ToString();
        }
        
        sb.AppendLine("");

        foreach (var mod in modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+ " + modValue + " " + modType);
        }

        if (itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique Effect: ");
            sb.AppendLine(itemEffect.effectDescription);
        }
        
        sb.AppendLine("");
        sb.AppendLine("");
        
        return sb.ToString();
    }

    private string GetStatNameByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return "Max Health";
            case StatType.HealthRegen: return "Health Regeneration";
            case StatType.Strength: return "Strength";
            case StatType.Agility: return "Agility";
            case StatType.Intelligence: return "Intelligence";
            case StatType.Vitality: return "Vitality";
            case StatType.AttackSpeed: return "Attack Speed";
            case StatType.Damage: return "Damage";
            case StatType.CritRate: return "Crit Rate";
            case StatType.CritDmg: return "Crit Damage";
            case StatType.ArmorReduction: return "Armor Reduction";
            case StatType.FireDmg: return "Fire Damage";
            case StatType.IceDmg: return "Ice Damage";
            case StatType.LightningDmg: return "Lightning Damage";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";
            case StatType.FireRes: return "Fire Resistance";
            case StatType.IceRes: return "Ice Resistance";
            case StatType.LightningRes: return "Lightning Resistance";
            default: return "Unknown Stat";
        }
    }

    private bool IsPercentStat(StatType type)
    {
        switch (type)
        {
            case StatType.CritRate:
            case StatType.CritDmg:
            case StatType.ArmorReduction:
            case StatType.FireRes:
            case StatType.IceRes:
            case StatType.LightningRes:
            case StatType.AttackSpeed:
            case StatType.Evasion:    
                return true;
            default: return false;
        }
    }
}
