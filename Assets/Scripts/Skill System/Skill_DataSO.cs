using UnityEngine;
using System;

[CreateAssetMenu(menuName = "RPG Setup/Skill Data", fileName = "Skill data - ")]
public class Skill_DataSO : ScriptableObject
{
    [Header("Skill Description")]
    public string displayName;
    [TextArea] public string description;
    public Sprite icon;
    
    [Header("Unlock & Upgrade")]
    public int cost;
    public bool unlockedByDefault;
    public SkillType skillType;
    public UpgradeData upgradeData;
    // Skill type that you should Unlock
}

[Serializable]
public class UpgradeData
{
    public SkillUpgradeType upgradeType;
    public float cooldown;
    public DamageScaleData damageScaleData;
}
