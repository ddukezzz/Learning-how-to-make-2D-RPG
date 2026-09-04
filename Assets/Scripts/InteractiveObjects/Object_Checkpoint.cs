using System;
using UnityEngine;

public class Object_Checkpoint : MonoBehaviour, ISaveable
{
    [SerializeField] private string checkpointID;
    [SerializeField] private Transform respawnPoint;
    
    public bool isActive { get; private set; }
    private Animator anim;
    private AudioSource fireAudioSource;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
        fireAudioSource = GetComponent<AudioSource>();
    }
    
    public string GetCheckpointID() => checkpointID;
    
    public Vector3 GetPosition() => respawnPoint == null ? transform.position : respawnPoint.position;

    public void ActiveCheckpoint(bool activate)
    {
        isActive = activate;
        anim.SetBool("isActive", activate);

        if (isActive && fireAudioSource.isPlaying == false)
            fireAudioSource.Play();
        if (isActive == false)
            fireAudioSource.Stop();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        ActiveCheckpoint(true);
    }

    public void LoadData(GameData data)
    {
       bool active = data.unlockedCheckpoints.TryGetValue(checkpointID, out active);
       ActiveCheckpoint(active);
    }

    public void SaveData(ref GameData data)
    {
        if (isActive == false)
            return;
        
        if (data.unlockedCheckpoints.ContainsKey(checkpointID) == false)
            data.unlockedCheckpoints.Add(checkpointID, true);
    }
    
    private void OnValidate()
    {
#if UNITY_EDITOR
        if (string.IsNullOrEmpty(checkpointID))
        {
            checkpointID = System.Guid.NewGuid().ToString();
        }
#endif
    }
}
