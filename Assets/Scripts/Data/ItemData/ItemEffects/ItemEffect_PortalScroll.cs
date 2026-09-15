using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu(menuName = "RPG Setup/Item Data/Item Effect/Portal Scroll", fileName = "Item Effect data - Portal Scroll")]
public class ItemEffect_PortalScroll : ItemEffect_DataSO
{
    public override void ExecuteEffect()
    {
        if (SceneManager.GetActiveScene().name == "Level_0")
        {
            Debug.Log("Cannot open Portal in Town");
            return;
        }

        Player player = Player.instance;
        Vector3 portalPosition = player.transform.position + new Vector3(player.facingDirection * 1.5f, 0);
        
        Object_Portal.instance.ActivePortal(portalPosition, player.facingDirection);
    }
}
