using UnityEngine;

public class Object_Chest : MonoBehaviour , IDamageable
{
    private Rigidbody2D rb => GetComponentInChildren<Rigidbody2D>();
    private Animator anim => GetComponentInChildren<Animator>();
    private Entity_VFX fx => GetComponent<Entity_VFX>();
    
    [Header("Open Details")]
    [SerializeField] private Vector2 knockback;
    
    public bool TakeDamage(float damage, float elementalDamge, ElementType element, Transform damageDealer)
    {
        fx.PlayOnDamageVfx();
        anim.SetBool("openchest", true);
        rb.linearVelocity = knockback;

        rb.angularVelocity = Random.Range(-200, 200);
        
        // Drop items

        return true;
    }
}
