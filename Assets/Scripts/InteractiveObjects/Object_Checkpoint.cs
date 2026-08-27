using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    private Object_Checkpoint[] allCheckpoints;
    private Animator anim;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        allCheckpoints = FindObjectsByType<Object_Checkpoint>(FindObjectsSortMode.None);
    }

    public void ActiveCheckpoint(bool activate)
    {
        anim.SetBool("isActive", activate);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach (var point in allCheckpoints)
            point.ActiveCheckpoint(false);

        SaveManager.instance.GetGameData().savedCheckpoint = transform.position;
        ActiveCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
        bool active  = data.savedCheckpoint == transform.position;
        ActiveCheckpoint(active);
        
        if (active)
            Player.instance.TeleportPlayer(transform.position);
    }

    public void SaveData(ref GameData data)
    {
        
    }
}
