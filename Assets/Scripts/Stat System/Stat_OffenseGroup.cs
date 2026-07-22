using UnityEngine;
using System;

[Serializable]
public class Stat_OffenseGroup
{
    public Stat attackSpeed;
    
    // Physical DMG
    public Stat damage;
    public Stat critDmg;
    public Stat critRate;
    public Stat armorReduction;
    
    // Elemental DMG
    public Stat fireDmg;
    public Stat iceDmg;
    public Stat lightningDmg;
}
