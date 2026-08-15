using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_StatSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Player_Stats playerStats;
    private RectTransform rect;
    private UI ui;
    
    [SerializeField] private StatType statSlotType;
    [SerializeField] private TextMeshProUGUI statName;
    [SerializeField] private TextMeshProUGUI statValue;

    private void OnValidate()
    {
        gameObject.name = "UI_Stat - " + GetStatNameByType(statSlotType);
        statName.text = GetStatNameByType(statSlotType);
    }

    private void Awake()
    {
        ui = GetComponentInParent<UI>();
        rect = GetComponent<RectTransform>();
        playerStats = FindFirstObjectByType<Player_Stats>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(true, rect, statSlotType);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        ui.statToolTip.ShowToolTip(false, null);
    }

    public void UpdateStatValue()
    {
        Stat statToUpdate = playerStats.GetStatByType(statSlotType);

        if (statToUpdate == null && statSlotType != StatType.ElementalDmg)
        {
            Debug.Log($"No {statSlotType} found for the player");
            return;
        }

        float value = 0;

        switch (statSlotType)
        {
            // Main Stats
            case StatType.Strength: value = playerStats.main.strength.GetValue(); break;
            case StatType.Agility: value = playerStats.main.agility.GetValue(); break;
            case StatType.Intelligence: value = playerStats.main.intelligence.GetValue(); break;
            case StatType.Vitality: value = playerStats.main.vitality.GetValue(); break;
            
            // Offense Stats
            case StatType.Damage: value = playerStats.GetBaseDmg(); break;
            case StatType.CritRate: value = playerStats.GetCritRate(); break;
            case StatType.CritDmg: value = playerStats.GetCritDmg(); break;
            case StatType.ArmorReduction: value = playerStats.GetArmorReduction() * 100; break;
            case StatType.AttackSpeed: value = playerStats.offense.attackSpeed.GetValue() * 100; break;
            
            // Defense Stats
            case StatType.MaxHealth: value = playerStats.GetMaxHP(); break;
            case StatType.HealthRegen: value = playerStats.resources.healthRegen.GetValue(); break;
            case StatType.Evasion: value = playerStats.GetEvasion(); break;
            case StatType.Armor: value = playerStats.GetBaseArmor(); break;
            
            // Elemental Dmg Stats
            case StatType.FireDmg: value = playerStats.offense.fireDmg.GetValue(); break;
            case StatType.IceDmg: value = playerStats.offense.iceDmg.GetValue(); break;
            case StatType.LightningDmg: value = playerStats.offense.lightningDmg.GetValue(); break;
            case StatType.ElementalDmg: value = playerStats.GetElementalDamage(out ElementType element, 1); break;
            
            // Elemental Resistance Stats
            case StatType.FireRes: value = playerStats.GetElementalResistance(ElementType.Fire) * 100; break;
            case StatType.IceRes: value = playerStats.GetElementalResistance(ElementType.Ice) * 100; break;
            case StatType.LightningRes: value = playerStats.GetElementalResistance(ElementType.Lightning) * 100; break;
        }

        statValue.text = IsPercentStat(statSlotType) ? value + "%" : value.ToString();
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
            case StatType.ElementalDmg: return "Elemental Damage";
            case StatType.Armor: return "Armor";
            case StatType.Evasion: return "Evasion";
            case StatType.FireRes: return "Fire Resistance";
            case StatType.IceRes: return "Ice Resistance";
            case StatType.LightningRes: return "Lightning Resistance";
            default: return "Unknown Stat";
        }
    }
}
