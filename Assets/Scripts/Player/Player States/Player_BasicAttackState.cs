using Unity.VisualScripting;
using UnityEngine;

public class Player_BasicAttackState : PlayerState
{
    private float attackVelocityTimer;
    private float lastTimeAttack;
    private bool comboAttackQueued;
    
    private const int FirstComboIndex = 1; // Combo starts with Index no.1, this parameter is used in the Animator.
    private int attackDirection;
    private int comboIndex = 1;
    private int comboLimit = 3;
    
    public Player_BasicAttackState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
        if (comboLimit != player.attackVelocity.Length)
        {
            Debug.LogWarning("Combo limit adjusted according to attack velocity array!!!");
            comboLimit = player.attackVelocity.Length;
        }
    }

    public override void Enter()
    {
        base.Enter();
        comboAttackQueued = false;
        ResetComboIndex();
        SyncAttackSpeed();
        
        // Define attack direction according to input
        attackDirection = player.moveInput.x != 0 ? (int)(player.moveInput.x) : player.facingDirection;
        
        anim.SetInteger("basicAttackIndex", comboIndex);
        ApplyAttackVelocity();
    }

    public override void Update()
    {
        base.Update();
        HandleAttackVelocity();

        if (input.Player.Attack.WasPressedThisFrame())
        {
            QueueNextAttack();
        }

        if (triggerCalled)
        {
            HandleStateExit();
        }
    }
    
    public override void Exit()
    {
        base.Exit();
        comboIndex++;
        lastTimeAttack = Time.time;
    }
    
    private void HandleStateExit()
    {
        if (comboAttackQueued)
        {
            anim.SetBool(AnimBoolName, false);
            player.EnterAttackStateWithDelay();
        }
        else
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    private void QueueNextAttack()
    {
        if (comboIndex < comboLimit)
        {
            comboAttackQueued = true;
        }
    }

    private void HandleAttackVelocity()
    {
        attackVelocityTimer -= Time.deltaTime;

        if (attackVelocityTimer < 0)
        {
            player.SetVelocity(0, rb.linearVelocity.y);
        }
    }

    private void ApplyAttackVelocity()
    {
        Vector2 attackVelocity = player.attackVelocity[comboIndex - 1];
        
        attackVelocityTimer = player.attackVelocityDuration;
        player.SetVelocity(attackVelocity.x * attackDirection, attackVelocity.y);
    }
    
    private void ResetComboIndex()
    {
        if (Time.time > lastTimeAttack + player.comboResetTime)
        {
            comboIndex = FirstComboIndex;
        }
        
        if (comboIndex > comboLimit)
        {
            comboIndex = FirstComboIndex;
        }
    }
}
