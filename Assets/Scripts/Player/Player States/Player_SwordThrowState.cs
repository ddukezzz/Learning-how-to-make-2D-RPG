using UnityEngine;

public class Player_SwordThrowState : PlayerState
{
    private Camera mainCamera;
    
    public Player_SwordThrowState(Player player, StateMachine stateMachine, string animBoolName) : base(player, stateMachine, animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        
        skillManager.swordThrow.EnableDots(true);

        if (mainCamera != Camera.main)
        {
            mainCamera = Camera.main;
        }
    }

    public override void Update()
    {
        base.Update();

        Vector2 directionToMouse = DirectionToMouse();
        
        player.SetVelocity(0, rb.linearVelocityY);
        player.HandleFlip(directionToMouse.x);
        skillManager.swordThrow.PredictTrajectory(directionToMouse);
        
        if (input.Player.Attack.WasPressedThisFrame())
        {
            anim.SetBool("swordThrowPerformed", true);
            
            skillManager.swordThrow.EnableDots(false);
            skillManager.swordThrow.ConfirmTrajectory(directionToMouse);
        }

        if (input.Player.RangeAttack.WasReleasedThisFrame() || triggerCalled)
        {
            stateMachine.ChangeState(player.idleState);
        }
    }

    public override void Exit()
    {
        base.Exit();
        
        anim.SetBool("swordThrowPerformed", false);
        skillManager.swordThrow.EnableDots(false);
    }

    private Vector2 DirectionToMouse()
    {
        Vector2 playerPosition = player.transform.position;
        Vector2 worldMousePosition = mainCamera.ScreenToWorldPoint(player.mousePosition);

        Vector2 direction = worldMousePosition - playerPosition;
        
        return direction.normalized;
    }
}
