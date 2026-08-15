using System;
using UnityEngine;
using UnityEngine.UI;

public class Entity_Health : MonoBehaviour , IDamageable
{
    public event Action OnTakingDamage;
    
    private Entity entity;
    private Entity_VFX entityVfx;
    private Entity_Stats entityStats;
    private Slider healthBar;

    [SerializeField] protected float currentHealth;
    public bool isDead { get; private set; }
    protected bool canTakeDamage = true;

    [Header("Health Regen")] 
    [SerializeField] private float regenInterval = 1f;
    [SerializeField] private bool canRegenerateHealth = true;
    public float lastDamageTaken {get ; private set;}

    [Header("On Damage Knockback")] 
    [SerializeField] private Vector2 knockbackPower = new Vector2(1.5f, 2.5f);
    [SerializeField] private Vector2 heavyKnockbackPower = new Vector2(7, 7);
    [SerializeField] private float knockbackDuration = 0.2f;
    [SerializeField] private float heavyKnockbackDuration = 0.5f;

    [Header("On Damage Knockback")] 
    [SerializeField] private float heavyDamageThreshold = 0.3f; // % of Health Loss to consider damage as heavy
    
    protected virtual void Awake()
    {
        entity = GetComponent<Entity>();
        entityVfx = GetComponent<Entity_VFX>();
        entityStats = GetComponent<Entity_Stats>();
        healthBar = GetComponentInChildren<Slider>();

        SetupHP();
    }

    private void SetupHP()
    {
        if (entityStats == null) return;
        
        currentHealth = entityStats.GetMaxHP();
        UpdateHealthBar();
        InvokeRepeating(nameof(RegenerateHealth), 0, regenInterval);
    }

    public virtual bool TakeDamage(float damage, float elementalDamage, ElementType element, Transform damageDealer)
    {
        if (isDead || canTakeDamage == false)
        {
            return false;
        }

        if (AttackEvade())
        {
            Debug.Log($"{gameObject.name} evaded!");
            return false;
        }
        
        Entity_Stats attackerStats = damageDealer.GetComponent<Entity_Stats>();
        float armorReduction = attackerStats != null ? attackerStats.GetArmorReduction() : 0;
        float mitigation = entityStats != null ? entityStats.GetArmorMitigation(armorReduction) : 0;
        float resistance = entityStats != null ? entityStats.GetElementalResistance(element) : 0;
        
        float physicalDamageTaken = damage * (1 - mitigation);
        float elementalDamageTaken = elementalDamage * (1 - resistance);
        
        TakeKnockback(damageDealer, physicalDamageTaken);
        ReduceHealth(physicalDamageTaken + elementalDamageTaken);

        lastDamageTaken = physicalDamageTaken + elementalDamageTaken;
        
        OnTakingDamage?.Invoke();
        return true;
    }
    
    public void SetCanTakeDamage(bool canTakeDamage) => this.canTakeDamage = canTakeDamage;

    private bool AttackEvade()
    {
        if (entityStats == null) return false;
        else return UnityEngine.Random.Range(0, 100) < entityStats.GetEvasion();
    }

    private void RegenerateHealth()
    {
        if (canRegenerateHealth == false)
        {
            return;
        }

        float regenAmount = entityStats.resources.healthRegen.GetValue();
        IncreaseHealth(regenAmount);
    }

    public void IncreaseHealth(float healAmount)
    {
        if (isDead)
        {
            return;
        }
        
        float newHealth = currentHealth + healAmount;
        float maxHealth = entityStats.GetMaxHP();

        currentHealth = Mathf.Min(newHealth, maxHealth);
        UpdateHealthBar();
    }

    public void ReduceHealth(float damage)
    {
        entityVfx?.PlayOnDamageVfx();
        currentHealth -= damage;
        UpdateHealthBar();

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    protected virtual void Die()
    {
        isDead = true;
        entity.EntityDeath();
    }
    
    public float GetHealthPercent() => currentHealth / entityStats.GetMaxHP();

    public void SetHealthToPercent(float percent)
    {
        currentHealth = entityStats.GetMaxHP() * Mathf.Clamp01(percent);
        UpdateHealthBar();
    }

    private void UpdateHealthBar()
    {
        if (healthBar == null)
        {
            return;
        }
        
        healthBar.value = currentHealth / entityStats.GetMaxHP();
    }
    
    private void TakeKnockback(Transform damageDealer, float finalDamage)
    {
        Vector2 knockback = CalculateKnockback(finalDamage, damageDealer);
        float duration = CalculateDuration(finalDamage);
        
        entity?.ReceiveKnockback(knockback, duration);
    }

    private Vector2 CalculateKnockback(float damage, Transform damageDealer)
    {
        int direction = transform.position.x > damageDealer.position.x ? 1 : -1;
        
        Vector2 knockback = IsHeavyDamage(damage) ? heavyKnockbackPower : knockbackPower;

        knockback.x = knockback.x * direction;

        return knockback;
    }
    
    private float CalculateDuration(float damage) => IsHeavyDamage(damage) ? heavyKnockbackDuration : knockbackDuration;

    private bool IsHeavyDamage(float damage)
    {
        if (entityStats == null) return false;
        else return damage / entityStats.GetMaxHP() > heavyDamageThreshold;
    }
}
