using UnityEngine;

[CreateAssetMenu(menuName = "Stat System/Stat Setup", fileName = "Default Stat Setup")]
public class Stat_SetupSO : ScriptableObject
{
    [Header("Resources")] 
    public float maxHealth = 100;
    public float healthRegen;
    
    [Header("Main")]
    public float strength;
    public float agility;
    public float intelligence;
    public float vitality;

    [Header("Offense - Physical Dmg")] 
    public float attackSpeed = 1;
    public float damage = 10;
    public float critRate;
    public float critDmg = 150;
    public float armorReduction;

    [Header("Offense - Elemental Dmg")] 
    public float fireDmg;
    public float iceDmg;
    public float lightningDmg;

    [Header("Defense - Physical Dmg")] 
    public float armor;
    public float evasion;
    
    [Header("Defense - Elemental Dmg")] 
    public float fireRes;
    public float iceRes;
    public float lightningRes;
}
