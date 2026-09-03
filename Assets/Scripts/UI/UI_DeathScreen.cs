using UnityEngine;

public class UI_DeathScreen : MonoBehaviour
{
    public void GoToCampButton()
    {
        GameManager.instance.ChangeScene("Level_0", RespawnType.NonSpecific);
    }

    public void GoToCheckpointButton()
    {
        GameManager.instance.RestartScene();
    }

    public void GoToMainMenuButton()
    {
        GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);
    }
}
