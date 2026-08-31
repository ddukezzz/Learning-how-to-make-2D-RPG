using UnityEngine;

public class Player_Health : Entity_Health
{
    private void Update()
    {
        // Test function
        if (Input.GetKeyDown(KeyCode.N))
        {
            Die(); 
        }
    }
    
    protected override void Die()
    {
        base.Die();
        
        GameManager.instance.SetLastDeathPosition(transform.position);
        GameManager.instance.RestartScene();
    }
}
