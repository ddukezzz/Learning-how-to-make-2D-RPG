using System;
using UnityEngine;

public class SkillObject_Base : MonoBehaviour
{
    [SerializeField] private GameObject onHitVfx;
    [Space]
    [SerializeField] protected LayerMask whatIsEnemy;
    [SerializeField] protected Transform targetCheck;
    [SerializeField] protected float checkRadius = 1;

    protected Rigidbody2D rb;
    protected Animator anim;
    protected Entity_Stats playerStats;
    protected DamageScaleData damageScaleData;
    protected ElementType usedElement;
    protected bool targetGotHit;
    protected Transform lastTarget;

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponentInChildren<Animator>();
    }
    
    protected void DamageEnemiesInRadius(Transform t, float radius)
    {
        foreach (var target in EnemiesAround(t, radius))
        {
            IDamageable damageable = target.GetComponent<IDamageable>();

            if (damageable == null) continue;

            AttackData attackData = playerStats.GetAttackData(damageScaleData);
            Entity_StatusHandler statusHandler = target.GetComponent<Entity_StatusHandler>();

            float physicalDmg = attackData.physicalDamage;
            float elementalDmg = attackData.elementalDamage;
            ElementType element = attackData.element;
            
            targetGotHit = damageable.TakeDamage(physicalDmg, elementalDmg, element, transform);

            if (element != ElementType.None)
            {
                statusHandler?.ApplyStatusEffect(element, attackData.effectData);
            }

            if (targetGotHit)
            {
                lastTarget = target.transform;
                Instantiate(onHitVfx, target.transform.position, Quaternion.identity);
            }

            usedElement = element;
        }
    }

    protected Transform FindClosetTarget()
    {
        Transform target = null;
        float closetDistance = Mathf.Infinity;

        foreach (var enemy in EnemiesAround(transform, 10))
        {
            float distance = Vector2.Distance(transform.position, enemy.transform.position);

            if (distance < closetDistance)
            {
                target = enemy.transform;
                closetDistance = distance;
            }
        }
        
        return target;
    }

protected Collider2D[] EnemiesAround(Transform t, float radius)
    {
        return Physics2D.OverlapCircleAll(t.position, radius, whatIsEnemy);
    }

    protected void OnDrawGizmos()
    {
        if (targetCheck == null)
        {
            targetCheck = transform;
        }
        
        Gizmos.DrawWireSphere(transform.position, checkRadius);
    }
}
