using UnityEngine;

public class EnemyState : EntityState
{
    protected Enemy enemy;
    
    public EnemyState(Enemy enemy, StateMachine stateMachine, string animBoolName) : base(stateMachine, animBoolName)
    {
        this.enemy = enemy;

        rb = enemy.rb;
        anim = enemy.anim;
        stats = enemy.stats;
    }

    public override void UpdateAnimationParameters()
    {
        base.UpdateAnimationParameters();
        
        float battleSpeedAnimMultiplier = enemy.battleMoveSpeed / enemy.moveSpeed;
        
        anim.SetFloat("battleSpeedAnimMultiplier", battleSpeedAnimMultiplier);
        anim.SetFloat("moveSpeedAnimMultiplier", enemy.moveSpeedAnimMultiplier);
        anim.SetFloat("xVelocity", rb.linearVelocity.x);
    }
}
