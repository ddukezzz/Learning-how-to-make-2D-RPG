using UnityEngine;
using System.Collections;

public class Enemy : Entity
{
    public Enemy_IdleState idleState;
    public Enemy_MoveState moveState;
    public Enemy_AttackState attackState;
    public Enemy_BattleState battleState;
    public Enemy_DeathState deathState;
    public Enemy_StunnedState  stunnedState;

    [Header("Battle details")] 
    public float battleMoveSpeed = 3f;
    public float attackDistance = 1f;
    public float battleTimeDuration = 5f;
    public float minRetreatDistance = 1f;
    public Vector2 retreatVelocity;

    [Header("Stunned state details")] 
    public float stunnedDuration = 1;
    public Vector2 stunnedVelocity = new Vector2(7, 7);
    [SerializeField] protected bool canBeStunned;
    
    [Header("Movement details")] 
    public float idleTime = 2;
    public float moveSpeed = 1.4f;
    [Range(0, 2)] public float moveSpeedAnimMultiplier = 1;

    [Header("Player detection")] 
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private Transform playerCheck;
    [SerializeField] private float playerCheckDistance = 10;
    
    public Transform player {get; private set;}

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalBattleMoveSpeed = battleMoveSpeed;
        float originalAnimSpeed = anim.speed;
        
        float speedMultiplier = 1 - slowMultiplier;
        
        moveSpeed = moveSpeed * speedMultiplier;
        battleMoveSpeed = battleMoveSpeed * speedMultiplier;
        anim.speed = anim.speed * speedMultiplier;
        
        yield return new WaitForSeconds(duration);
        
        moveSpeed = originalMoveSpeed;
        battleMoveSpeed = originalBattleMoveSpeed;
        anim.speed = originalAnimSpeed;
    }
    
    public void EnableCounterWindow(bool enable) => canBeStunned = enable;
    
    public override void EntityDeath()
    {
        base.EntityDeath();
        
        stateMachine.ChangeState(deathState);
    }

    private void HandlePlayerDeath()
    {
        stateMachine.ChangeState(idleState);
    }

    public void TryEnterBattleState(Transform player)
    {
        if (stateMachine.currentState == battleState)
        {
            return;
        }
        if (stateMachine.currentState == attackState)
        {
            return;
        }
        
        this.player = player;
        stateMachine.ChangeState(battleState);
    }

    public Transform GetPlayerReference()
    {
        if (player == null)
        {
            player = PlayerDetection().transform;
        }
        
        return player;
    }
    
    public RaycastHit2D PlayerDetection()
    {
        RaycastHit2D hit = Physics2D.Raycast(playerCheck.position, Vector2.right * facingDirection, playerCheckDistance,
            whatIsPlayer | whatIsGround);

        if (hit.collider == null || hit.collider.gameObject.layer != LayerMask.NameToLayer("Player"))
        {
            return default;
        }
        
        return hit;
    }
    
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * playerCheckDistance), playerCheck.position.y));
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * attackDistance), playerCheck.position.y));
        Gizmos.color = Color.green;
        Gizmos.DrawLine(playerCheck.position, new Vector3(playerCheck.position.x + (facingDirection * minRetreatDistance), playerCheck.position.y));
    }

    private void OnEnable()
    {
        Player.OnPlayerDeath += HandlePlayerDeath;
    }

    private void OnDisable()
    {
        Player.OnPlayerDeath -= HandlePlayerDeath;
    }
}
