using TMPro;
using UnityEngine;

public class UI_StatToolTip : UI_ToolTip
{
    private Player_Stats playerStats;
    private TextMeshProUGUI statToolTipText;

    protected override void Awake()
    {
        base.Awake();
        playerStats = FindFirstObjectByType<Player_Stats>();
        statToolTipText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void ShowToolTip(bool show, RectTransform targetRect, StatType statType)
    {
        base.ShowToolTip(show, targetRect);
        statToolTipText.text = GetStatTextByType(statType);
    }

    public string GetStatTextByType(StatType type)
    {
        switch (type)
        {
            // Main Stats
            case StatType.Strength:
                return "Increases Physical Dmg by 1 per Point" +
                       "\n Increases Crit Dmg by 1% per Point";
            case StatType.Agility:
                return "Increases Crit Rate by 0.3% per Point" +
                       "\n Increases Evasion by 0.5% per Point";
            case StatType.Intelligence:
                return "Increases Elemental RES by 0.5% per Point" +
                       "If all Elements have 0 Dmg, the Bonus will not be applied";
            case StatType.Vitality:
                return "Increases Max HP by 5 per Point" +
                       "\n Increases Armor by 1 per Point";
            
            // Physical Dmg Stats
            case StatType.Damage:
                return "Determines the Physical DMG of your attacks";
            case StatType.CritRate:
                return "Chance for your attacks to deal Critical Hit";
            case StatType.CritDmg:
                return "Increases DMG dealt by your Critical Hit";
            case StatType.ArmorReduction:
                return "Percentage of Armor that will be ignored by your attacks";
            case StatType.AttackSpeed:
                return "Determines how quickly you can attack";
            
            // Defense Stats
            case StatType.MaxHealth:
                return "Determines how much total Health you have";
            case StatType.HealthRegen:
                return "Amount of Health restored per Second";
            case StatType.Armor:
                return "Reduces incoming PhysicalDmg";
            case StatType.Evasion:
                return "Chance to completely avoid attacks";
            
            // Elemental Dmg Stats
            case StatType.FireDmg:
                return "Your attacks can deal Fire DMG";
            case StatType.IceDmg:
                return "Your attacks can deal Ice DMG";
            case StatType.LightningDmg:
                return "Your attacks can deal Lightning DMG";
            case StatType.ElementalDmg:
                return "Elemental DMG combines all three elements" +
                       "\n The highest element applies corresponding Element Status Effect and full DMG" +
                       "\n The other 2 elements contribute 50% of their DMG as bonus";
            
            // Elemental Res Stats
            case StatType.FireRes:
                return "Reduces incoming Fire DMG";
            case StatType.IceRes:
                return "Reduces incoming Ice DMG";
            case StatType.LightningRes:
                return "Reduces incoming Lightning DMG";
            
            default:
                return "No tooltip available for this stat";
        }
    }
}
