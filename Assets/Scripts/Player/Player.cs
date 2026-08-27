using System;
using System.Collections;
using UnityEngine;

public class Player : Entity
{
    public static Player instance;
    public static event Action OnPlayerDeath;
    
    public UI ui {get; private set;}
    public Player_VFX vfx {get; private set;}
    public PlayerInputSet input{get; private set;}
    public Entity_Health health {get; private set;}
    public Player_SkillManager skillManager {get; private set;}
    public Entity_StatusHandler statusHandler {get; private set;}
    public Player_Combat combat {get; private set;}
    public Inventory_Player inventory {get; private set;}
    public Player_Stats stats {get; private set;}
    
    public Player_IdleState idleState {get; private set;}
    public Player_MoveState moveState {get; private set;}
    public Player_JumpState jumpState {get; private set;}
    public Player_FallState fallState {get; private set;}
    public Player_WallSlideState wallSlideState {get; private set;}
    public Player_WallJumpState wallJumpState {get; private set;}
    public Player_DashState dashState {get; private set;}
    public Player_BasicAttackState basicAttackState  {get; private set;}
    public Player_JumpAttackState jumpAttackState  {get; private set;}
    public Player_DeadState deadState {get; private set;}
    public Player_CounterAttackState counterAttackState {get; private set;}
    public Player_SwordThrowState swordThrowState {get; private set;}
    public Player_DomainExpansionState  domainExpansionState {get; private set;}
    
    [Header("Attack details")] 
    public Vector2[] attackVelocity;
    public Vector2 jumpAttackVelocity;
    public float attackVelocityDuration = 0.1f;
    public float comboResetTime = 1;
    private Coroutine queueAttackCo;

    [Header("Ultimate Ability Details")] 
    public float riseSpeed = 25;
    public float riseMaxDistance = 3;
    
    [Header("Movement details")] 
    public float moveSpeed;
    public float jumpForce = 5;
    public Vector2 wallJumpDirection;
    public float inAirMultiplier = 0.7f; // value: 0 -> 1
    public float wallSlideSlowMultiplier = 0.7f; // value: 0 -> 1
    [Space]
    public float dashDuration = 0.25f;
    public float dashSpeed = 20f;
    
    public Vector2 moveInput  {get; private set;}
    public Vector2 mousePosition {get; private set;}

    protected override void Awake()
    {
        base.Awake();
        instance = this;
        
        ui = FindAnyObjectByType<UI>();
        vfx = GetComponent<Player_VFX>();
        health = GetComponent<Entity_Health>();
        skillManager = GetComponent<Player_SkillManager>();
        statusHandler = GetComponent<Entity_StatusHandler>();
        combat = GetComponent<Player_Combat>();
        inventory = GetComponent<Inventory_Player>();
        stats = GetComponent<Player_Stats>();
        
        input = new  PlayerInputSet();
        ui.SetupControlsUI(input);

        idleState = new Player_IdleState(this, stateMachine, "Idle");
        moveState = new Player_MoveState(this, stateMachine, "Move");
        jumpState = new Player_JumpState(this, stateMachine, "jumpFall");
        fallState = new Player_FallState(this, stateMachine, "jumpFall");
        wallSlideState = new Player_WallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new Player_WallJumpState(this, stateMachine, "jumpFall");
        dashState = new Player_DashState(this, stateMachine, "Dash");
        basicAttackState = new Player_BasicAttackState(this, stateMachine, "basicAttack");
        jumpAttackState = new Player_JumpAttackState(this, stateMachine, "jumpAttack");
        deadState = new Player_DeadState(this, stateMachine, "Dead");
        counterAttackState = new Player_CounterAttackState(this, stateMachine, "counterAttack");
        swordThrowState = new Player_SwordThrowState(this, stateMachine, "swordThrow");
        domainExpansionState = new Player_DomainExpansionState(this, stateMachine, "jumpFall");
    }

    protected override void Start()
    {
        base.Start();
        
        stateMachine.Initialize(idleState);
    }
    
    public void TeleportPlayer(Vector2 position) => transform.position = position;

    protected override IEnumerator SlowDownEntityCo(float duration, float slowMultiplier)
    {
        float originalMoveSpeed = moveSpeed;
        float originalJumpForce = jumpForce;
        float originalAnimSpeed = anim.speed;
        Vector2 originalWallJump = wallJumpDirection;
        Vector2 originalJumpAttack = jumpAttackVelocity;
        Vector2[] originalAttackVelocity = new Vector2[attackVelocity.Length];
        Array.Copy(attackVelocity, originalAttackVelocity, attackVelocity.Length);
        
        float speedMultiplier = 1 - slowMultiplier;
        
        moveSpeed *= speedMultiplier;
        jumpForce *= speedMultiplier;
        anim.speed *= speedMultiplier;
        wallJumpDirection *= speedMultiplier;
        jumpAttackVelocity *= speedMultiplier;

        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] *= speedMultiplier;
        }
        
        yield return new WaitForSeconds(duration);
        
        moveSpeed = originalMoveSpeed;
        jumpForce = originalJumpForce;
        anim.speed = originalAnimSpeed;
        wallJumpDirection = originalWallJump;
        jumpAttackVelocity = originalJumpAttack;
        
        for (int i = 0; i < attackVelocity.Length; i++)
        {
            attackVelocity[i] = originalAttackVelocity[i];
        }
    }

    public override void EntityDeath()
    {
        base.EntityDeath();
        
        OnPlayerDeath?.Invoke();
        stateMachine.ChangeState(deadState);
    }

    public void EnterAttackStateWithDelay()
    {
        if (queueAttackCo != null)
        {
            StopCoroutine(queueAttackCo);
        }

        queueAttackCo = StartCoroutine(EnterAttackStateWithDelayCo());
    }

    private IEnumerator EnterAttackStateWithDelayCo()
    {
        yield return new WaitForEndOfFrame();
        stateMachine.ChangeState(basicAttackState);
    }

    private void TryInteract()
    {
        Transform closest = null;
        float closestDistance = Mathf.Infinity;
        Collider2D[] objectsAround = Physics2D.OverlapCircleAll(transform.position, 1f);

        foreach (var target in objectsAround)
        {
            IInteractable interactable = target.GetComponent<IInteractable>();
            if (interactable == null) continue;
            
            float distance = Vector2.Distance(transform.position, target.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = target.transform;
            }
        }

        if (closest == null) return;
        
        closest.GetComponent<IInteractable>().Interact();
    }
    
    private void OnEnable()
    {
        input.Enable();
        
        input.Player.Mouse.performed += ctx => mousePosition = ctx.ReadValue<Vector2>();
        
        input.Player.Movement.performed += ctx => moveInput = ctx.ReadValue<Vector2>();
        input.Player.Movement.canceled += ctx => moveInput = Vector2.zero;
        
        input.Player.Spell.performed += ctx => skillManager.shard.TryUseSkill();
        input.Player.Spell.performed += ctx => skillManager.timeEcho.TryUseSkill();

        input.Player.Interact.performed += ctx => TryInteract();

        input.Player.QuickItemSlot_1.performed += ctx => inventory.TryUseQuickItemInSlot(1);
        input.Player.QuickItemSlot_2.performed += ctx => inventory.TryUseQuickItemInSlot(2);
    }

    private void OnDisable()
    {
        input.Disable();
    }
}
