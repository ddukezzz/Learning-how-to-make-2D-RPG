using UnityEngine;

public abstract class EntityState
{
    protected StateMachine stateMachine;
    protected string AnimBoolName;

    protected Animator anim;
    protected Rigidbody2D rb;
    protected Entity_Stats stats;
    
    protected float stateTimer;
    protected bool triggerCalled;

    public EntityState(StateMachine stateMachine, string animBoolName)
    {
        this.stateMachine = stateMachine;
        this.AnimBoolName = animBoolName;
    }
    
    public virtual void Enter()
    {
        anim.SetBool(AnimBoolName, true);
        triggerCalled = false;
    }

    public virtual void Update()
    {
        stateTimer -= Time.deltaTime;
        UpdateAnimationParameters();
    }

    public virtual void Exit()
    {
        anim.SetBool(AnimBoolName, false);
    }

    public void AnimationTrigger()
    {
        triggerCalled = true;
    }

    public virtual void UpdateAnimationParameters()
    {
        
    }

    public void SyncAttackSpeed()
    {
        float attackSpeed = stats.offense.attackSpeed.GetValue();
        anim.SetFloat("attackSpeedMultiplier", attackSpeed);
    }
}
