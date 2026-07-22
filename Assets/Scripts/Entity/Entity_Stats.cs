using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class Entity_Stats : MonoBehaviour
{
    public Stat_SetupSO defaultStatSetup;
    
    public Stat_ResourceGroup resources;
    public Stat_MainGroup main;
    public Stat_OffenseGroup offense;
    public Stat_DefenseGroup defense;

    public float GetElementalDamage(out ElementType element, float scaleFactor = 1f)
    {
        float fireDmg = offense.fireDmg.GetValue();
        float iceDmg = offense.iceDmg.GetValue();
        float lightningDmg = offense.lightningDmg.GetValue();
        float bonusElementalDmg = main.intelligence.GetValue(); // Bonus E.Dmg from Intelligence: +1 per INT

        float highestDmg = fireDmg;
        element = ElementType.Fire;
        if (iceDmg > highestDmg)
        {
            highestDmg = iceDmg;
            element = ElementType.Ice;
        }
        if (lightningDmg > highestDmg)
        {
            highestDmg = lightningDmg;
            element = ElementType.Lightning;
        }

        if (highestDmg <= 0)
        {
            element = ElementType.None;
            return 0;
        }
        
        float bonusFire = (element == ElementType.Fire) ? 0f : fireDmg * 0.5f;
        float bonusIce = (element == ElementType.Ice) ? 0f : iceDmg * 0.5f;
        float bonusLightning = (element == ElementType.Lightning) ? 0f : lightningDmg * 0.5f;
        
        float weakerElementalDmg = bonusFire + bonusIce + bonusLightning;
        float finalDmg = highestDmg + weakerElementalDmg + bonusElementalDmg;
        
        return finalDmg * scaleFactor;
    }

    public float GetElementalResistance(ElementType element)
    {
        float baseResistance = 0;
        float bonusResistance = main.intelligence.GetValue() * 0.5f; // Bonus resistance from intelligence: +0.5% per INT

        
        switch (element)
        { 
            case ElementType.Fire:
                baseResistance = defense.fireRes.GetValue(); 
                break;
            case ElementType.Ice:
                baseResistance = defense.iceRes.GetValue();
                break;
            case ElementType.Lightning:
                baseResistance = defense.lightningRes.GetValue();
                break;
            }
        float resistance = baseResistance + bonusResistance;
        float resistanceCap = 75f; // RES will be capped at 75%

        float finalResistance = Mathf.Clamp(resistance, 0, resistanceCap) / 100;
        return finalResistance;
    }

    public float GetMaxHP()
    {
        float baseMaxHP = resources.maxHealth.GetValue();
        float bonusMaxHP = main.vitality.GetValue() * 5; // Bonus health from Vitality: +5 per VIT
        
        float finalMaxHP = baseMaxHP + bonusMaxHP;
        
        return finalMaxHP;
    }
    
    public float GetPhysicalDmg(out bool isCrit, float scaleFactor = 1)
    {
        float baseDmg = offense.damage.GetValue();
        float bonusDmg = main.strength.GetValue();
        float totalBaseDmg = baseDmg + bonusDmg;
        
        float baseCritRate = offense.critRate.GetValue();
        float bonusCritRate = main.agility.GetValue() * 0.3f; // Bonus Crit rate from Agility: +0.3% per AGL
        float critRate = baseCritRate + bonusCritRate;
        
        float baseCritDmg = offense.critDmg.GetValue();
        float bonusCritDmg = main.strength.GetValue() * 1f; // Bonus Crit dmg from Strength: +1% per STR
        float critDmg = (baseCritDmg + bonusCritDmg) / 100; // Total Crit Dmg as multiplier (ex: 150 / 100 = 1.5f - multiplier)

        isCrit = Random.Range(0f, 1f) <= critRate;
        float finalDmg = isCrit ? totalBaseDmg * critDmg : totalBaseDmg;
        
        return finalDmg * scaleFactor;
    }

    public float GetArmorMitigation(float armorReduction)
    {
        float baseArmor = defense.armor.GetValue();
        float bonusArmor = main.vitality.GetValue(); // Bonus armor from Vitality: +1 per VIT
        float totalArmor = baseArmor + bonusArmor;
        
        float reductionMultiplier = Mathf.Clamp(1 - armorReduction, 0, 1);
        float effectiveArmor = totalArmor * reductionMultiplier;
        
        float mitigation = effectiveArmor / (effectiveArmor + 100);
        float mitigationCap = 0.8f; // Max DMG mitigation will be capped at 80%
        
        float finalMitigation = Mathf.Clamp(mitigation, 0, mitigationCap);
        
        return finalMitigation;
    }

    public float GetArmorReduction()
    {
        float finalReduction = offense.armorReduction.GetValue() / 100;
        
        return finalReduction;
    }

    public float GetEvasion()
    {
        float baseEvasion = defense.evasion.GetValue();
        float bonusEvasion = main.agility.GetValue() * 0.5f; // Bonus evasion from Agility: +0.5% per AGL

        float totalEvasion = baseEvasion + bonusEvasion;
        float evasionCap = 50f; // Max evasion will be capped at 50%

        float finalEvasion = Math.Clamp(totalEvasion, 0, evasionCap);
        
        return finalEvasion;
    }

    public Stat GetStatByType(StatType type)
    {
        switch (type)
        {
            case StatType.MaxHealth: return resources.maxHealth;
            case StatType.HealthRegen: return resources.healthRegen;
            
            case StatType.Strength: return main.strength;
            case StatType.Agility: return main.agility;
            case StatType.Intelligence: return main.intelligence;
            case StatType.Vitality: return main.vitality;
            
            case StatType.AttackSpeed: return offense.attackSpeed;
            case StatType.Damage: return offense.damage;
            case StatType.CritRate: return offense.critRate;
            case StatType.CritDmg: return offense.critDmg;
            case StatType.ArmorReduction: return offense.armorReduction;
            
            case StatType.FireDmg: return offense.fireDmg;
            case StatType.IceDmg: return offense.iceDmg;
            case StatType.LightningDmg: return offense.lightningDmg;
            
            case StatType.Armor: return defense.armor;
            case StatType.Evasion: return defense.evasion;
            
            case StatType.FireRes: return defense.fireRes;
            case StatType.IceRes: return defense.iceRes;
            case StatType.LightningRes: return defense.lightningRes;
            
            default: 
                Debug.LogWarning($"Stat type {type} not implemented");
                return null;
        }
    }

    [ContextMenu("Update Default Stats Setup")]
    public void ApplyDefaultStatSetup()
    {
        if (defaultStatSetup == null)
        {
            Debug.Log($"Stat setup not implemented");
            return;
        }
        
        resources.maxHealth.SetBaseValue(defaultStatSetup.maxHealth);
        resources.healthRegen.SetBaseValue(defaultStatSetup.healthRegen);
        
        main.strength.SetBaseValue(defaultStatSetup.strength);
        main.agility.SetBaseValue(defaultStatSetup.agility);
        main.intelligence.SetBaseValue(defaultStatSetup.intelligence);
        main.vitality.SetBaseValue(defaultStatSetup.vitality);
        
        offense.attackSpeed.SetBaseValue(defaultStatSetup.attackSpeed);
        offense.damage.SetBaseValue(defaultStatSetup.damage);
        offense.critRate.SetBaseValue(defaultStatSetup.critRate);
        offense.critDmg.SetBaseValue(defaultStatSetup.critDmg);
        offense.armorReduction.SetBaseValue(defaultStatSetup.armorReduction);
        
        offense.fireDmg.SetBaseValue(defaultStatSetup.fireRes);
        offense.iceDmg.SetBaseValue(defaultStatSetup.iceRes);
        offense.lightningDmg.SetBaseValue(defaultStatSetup.lightningRes);
        
        defense.armor.SetBaseValue(defaultStatSetup.armor);
        defense.evasion.SetBaseValue(defaultStatSetup.evasion);
        
        defense.fireRes.SetBaseValue(defaultStatSetup.fireRes);
        defense.iceRes.SetBaseValue(defaultStatSetup.iceRes);
        defense.lightningRes.SetBaseValue(defaultStatSetup.lightningRes);
    }
}
