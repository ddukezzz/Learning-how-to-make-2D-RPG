using UnityEngine;

public class Skill_TimeEcho : Skill_Base
{
    [SerializeField] private GameObject timeEchoPrefab;
    [SerializeField] private float timeEchoDuration;

    [Header("Attack Upgrades")] 
    [SerializeField] private int maxAttacks = 3;
    [SerializeField] private float duplicateChance = 0.3f;

    [Header("Heal Wisp Upgrades")] 
    [SerializeField] private float dmgPercentHealed = 0.3f;
    [SerializeField] private float cooldownReducedInSeconds;

    public float GetPercentOfDamageHealed()
    {
        if (ShouldBeWisp() == false) return 0;
        return dmgPercentHealed;
    }

    public float GetCooldownReducedInSeconds()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_CooldownWisp) return 0;
        return cooldownReducedInSeconds;
    }

    public bool CanRemoveNegativeEffects()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp;
    }
    
    public bool ShouldBeWisp()
    {
        return upgradeType == SkillUpgradeType.TimeEcho_HealWisp ||
               upgradeType == SkillUpgradeType.TimeEcho_CleanseWisp ||
               upgradeType == SkillUpgradeType.TimeEcho_CooldownWisp;
    }
    
    public float GetDuplicationChance()
    {
        if (upgradeType != SkillUpgradeType.TimeEcho_ChanceToMultiply) return 0;
        
        return duplicateChance;
    }
    
    public int GetMaxAttackS()
    {
        if (upgradeType == SkillUpgradeType.TimeEcho_SingleAttack ||
            upgradeType == SkillUpgradeType.TimeEcho_ChanceToMultiply)
            return 1;
        if (upgradeType == SkillUpgradeType.TimeEcho_MultiAttack)
            return maxAttacks;

        return 0;
    }
    
    public float GetTimeEchoDuration()
    {
        return timeEchoDuration;
    }
    
    public override void TryUseSkill()
    {
        if (CanUseSkill() == false) return;
        
        CreateTimeEcho();
        SetSkillOnCooldown();
    }
    
    public void CreateTimeEcho(Vector3 targetPosition = default(Vector3))
    {
        GameObject timeEcho = Instantiate(timeEchoPrefab, transform.position, Quaternion.identity);
        timeEcho.GetComponent<SkillObject_TimeEcho>().SetupEcho(this);
    }
}
