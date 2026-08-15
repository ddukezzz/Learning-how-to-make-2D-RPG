using System;
using UnityEngine;

public class Entity_Combat : MonoBehaviour
{
    public event Action<float> OnDoingPhysicalDmg;
    
    private Entity_VFX vfx;
    private Entity_Stats stats;

    public DamageScaleData basicAttackScale;
    
    [Header("Target detection")] 
    [SerializeField] private Transform targetCheck;
    [SerializeField] private float targetCheckRadius = 1;
    [SerializeField] private LayerMask whatIsTarget;
    
    private void Awake()
    {
        vfx = GetComponent<Entity_VFX>();
        stats = GetComponent<Entity_Stats>();
    }

    public void PerformAttack()
    {
        GetDetectedColliders();

        foreach (var target in GetDetectedColliders())
        {
            IDamageable damageable = target.GetComponent<IDamageable>();
            
            if (damageable == null) continue;

            AttackData attackData = stats.GetAttackData(basicAttackScale);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physicalDmg = attackData.physicalDamage;
            float elementalDamage = attackData.elementalDamage;
            ElementType element = attackData.element;
            
            bool targetGotHit = damageable.TakeDamage(physicalDmg, elementalDamage, element, transform);

            if (element != ElementType.None)
            {
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);
            }
            
            if (targetGotHit)
            {
                OnDoingPhysicalDmg?.Invoke(physicalDmg);
                vfx.CreateOnHitVFX(target.transform, attackData.isCrit, element);
            }
        }
    }
    
    protected Collider2D[] GetDetectedColliders()
    {
        return Physics2D.OverlapCircleAll(targetCheck.position, targetCheckRadius, whatIsTarget);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(targetCheck.position, targetCheckRadius);
    }
}
