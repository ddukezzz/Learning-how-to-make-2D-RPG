using UnityEngine;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Heal On Doing Damage", fileName = "Item Effect data - Heal On Doing Physical Dmg")]
public class ItemEffect_HealOnDoingDamage : ItemEffect_DataSO
{
    [SerializeField] private float percentHealedOnAttack = 0.2f;

    public override void Subscribe(Player player)
    {
        base.Subscribe(player);
        player.combat.OnDoingPhysicalDmg += HealOnDoingDamage;
    }

    public override void Unsubscribe()
    {
        base.Unsubscribe();
        player.combat.OnDoingPhysicalDmg -= HealOnDoingDamage;
        player = null;
    }
    
    private void HealOnDoingDamage(float damage)
    {
        player.health.IncreaseHealth(damage * percentHealedOnAttack);
    }
}
