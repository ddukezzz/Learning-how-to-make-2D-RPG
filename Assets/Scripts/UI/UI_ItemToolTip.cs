using System.Text;
using TMPro;
using UnityEngine;

public class UI_ItemToolTip : UI_ToolTip
{
    [SerializeField] private TextMeshProUGUI itemName;
    [SerializeField] private TextMeshProUGUI itemType;
    [SerializeField] private TextMeshProUGUI itemInfo;

    public void ShowToolTip(bool show, RectTransform targetRect, Inventory_Item itemToShow)
    {
        base.ShowToolTip(show, targetRect);

        itemName.text = itemToShow.itemData.itemName;
        itemType.text = itemToShow.itemData.itemType.ToString();
        itemInfo.text = GetItemInfo(itemToShow);
    }

    public string GetItemInfo(Inventory_Item item)
    {
        if (item.itemData.itemType == ItemType.Material)
        {
            return "Used for Crafting!";
        }

        if (item.itemData.itemType == ItemType.Consumable)
        {
            return item.itemData.itemEffect.effectDescription;
        }

        StringBuilder sb = new StringBuilder();

        sb.AppendLine("");

        foreach (var mod in item.modifiers)
        {
            string modType = GetStatNameByType(mod.statType);
            string modValue = IsPercentStat(mod.statType) ? mod.value.ToString() + "%" : mod.value.ToString();
            sb.AppendLine("+ " + modValue + " " + modType);
        }

        if (item.itemEffect != null)
        {
            sb.AppendLine("");
            sb.AppendLine("Unique Effect: ");
            sb.AppendLine(item.itemEffect.effectDescription);
        }
        
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
