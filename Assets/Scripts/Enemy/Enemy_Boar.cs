using UnityEngine;

public class Enemy_Boar : Enemy , ICounterable
{
    public bool canBeCountered { get => canBeStunned; }

    protected override void Awake()
    {
        base.Awake();

        idleState = new Enemy_IdleState(this, stateMachine, "idle");
        moveState = new Enemy_MoveState(this, stateMachine, "move");
        attackState = new Enemy_AttackState(this, stateMachine, "attack");
        battleState = new Enemy_BattleState(this, stateMachine, "battle");
        deathState = new Enemy_DeathState(this, stateMachine, "idle");
        stunnedState = new Enemy_StunnedState(this, stateMachine, "stunned");
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);
    }
    
    public void HandleCounter()
    {
        if (canBeCountered == false)
        {
            return;
        }
        
        stateMachine.ChangeState(stunnedState);
    }
}
