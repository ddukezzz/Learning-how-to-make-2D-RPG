using UnityEngine;
using System.Collections;

public class Entity_StatusHandler : MonoBehaviour
{
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Entity_Health entityHealth;
    private ElementType currentEffect = ElementType.None;

    [Header("Electrified effect details")] 
    [SerializeField] private GameObject lightningStrikeVfx;
    [SerializeField] private float currentCharge;
    [SerializeField] private float maximumCharge = 1;
    private Coroutine electrifiedCo;

    private void Awake()
    {
        entity = GetComponent<Entity>();
        entityStats = GetComponent<Entity_Stats>();
        entityHealth = GetComponent<Entity_Health>();
        entityVfx = GetComponent<Entity_VFX>();
    }

    public void ApplyStatusEffect(ElementType element, ElementalEffectData effectData)
    {
        if (element == ElementType.Ice && CanBeApplied(ElementType.Ice))
        {
            ApplyChilledEffect(effectData.chillDuration, effectData.chillSlowMultiplier);
        }

        if (element == ElementType.Fire && CanBeApplied(ElementType.Fire))
        {
            ApplyBurnedEffect(effectData.burnDuration, effectData.totalBurnDamage);
        }

        if (element == ElementType.Lightning && CanBeApplied(ElementType.Lightning))
        {
            ApplyElectrifiedEffect(effectData.shockDuration, effectData.shockDamage, effectData.shockCharge);
        }
    }

    public void ApplyElectrifiedEffect(float duration, float damage, float charge)
    {
        float lightningRes = entityStats.GetElementalResistance(ElementType.Lightning);
        float finalCharge = charge * (1 - lightningRes);
        
        currentCharge = currentCharge + finalCharge;

        if (currentCharge >= maximumCharge)
        {
            DoLightningStrike(damage);
            StopElectrifiedEffect();
            return;
        }

        if (electrifiedCo != null)
        {
            StopCoroutine(electrifiedCo);
        }
        
        electrifiedCo = StartCoroutine(ElectrifiedEffectCo(duration));
    }

    private void StopElectrifiedEffect()
    {
        currentEffect = ElementType.None;
        currentCharge = 0;
        entityVfx.StopAllVFX();
    }

    private void DoLightningStrike(float damage)
    {
        Instantiate(lightningStrikeVfx, transform.position, Quaternion.identity);
        entityHealth.ReduceHealth(damage);
    }

    private IEnumerator ElectrifiedEffectCo(float duration)
    {
        currentEffect = ElementType.Lightning;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Lightning);
        
        yield return new WaitForSeconds(duration);
        StopElectrifiedEffect();
    }

    public void ApplyBurnedEffect(float duration, float fireDamage)
    {
        float fireRes = entityStats.GetElementalResistance(ElementType.Fire);
        float finalDamage = fireDamage * (1 - fireRes);
        
        StartCoroutine(BurnedEffectCo(duration, fireDamage));
    }

    private IEnumerator BurnedEffectCo(float duration, float totalDamage)
    {
        currentEffect = ElementType.Fire;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Fire);

        int ticksPerSecond = 2;
        int tickCount = Mathf.RoundToInt(ticksPerSecond * duration);
        
        float damagePerTick = totalDamage / tickCount;
        float tickInterval = 1f / ticksPerSecond;

        for (int i = 0; i < tickCount; i++)
        {
            entityHealth.ReduceHealth(damagePerTick);
            yield return new WaitForSeconds(tickInterval);
        }
        
        currentEffect = ElementType.None;
    }
    
    public void ApplyChilledEffect(float duration, float slowMultiplier)
    {
        float iceRes = entityStats.GetElementalResistance(ElementType.Ice);
        float finalDuration = duration * (1  - iceRes);
        
        StartCoroutine(ChilledEffectCo(finalDuration, slowMultiplier));
    }

    private IEnumerator ChilledEffectCo(float duration, float slowMultiplier)
    {
        entity.slowDownEntity(duration, slowMultiplier);
        currentEffect = ElementType.Ice;
        entityVfx.PlayOnStatusVfx(duration, ElementType.Ice);

        yield return new WaitForSeconds(duration);
        currentEffect = ElementType.None;
    }

    public bool CanBeApplied(ElementType element)
    {
        if (element == ElementType.Lightning && currentEffect == ElementType.Lightning)
        {
            return true;
        }
        
        return currentEffect == ElementType.None;
    }
}
