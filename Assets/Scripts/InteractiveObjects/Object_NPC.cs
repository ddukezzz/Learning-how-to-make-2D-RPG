using System;
using UnityEngine;

public class Object_NPC : MonoBehaviour
{
    protected Transform player;
    protected UI ui;

    [SerializeField] private Transform npc;
    [SerializeField] private GameObject interactiveToolTip;
    private bool facingRight = true;
    
    [Header("Floaty Tooltip")] 
    [SerializeField] private float floatSpeed = 8f;
    [SerializeField] private float floatRange = 0.1f;
    private Vector3 startPosition;

    protected virtual void Awake()
    {
        ui = FindFirstObjectByType<UI>();
        startPosition = interactiveToolTip.transform.position;
        interactiveToolTip.SetActive(false);
    }

    protected virtual void Update()
    {
        HandleNpcFlip();
        HandleToolTipFloat();
    }

    private void HandleToolTipFloat()
    {
        if (interactiveToolTip.activeSelf)
        {
            float yOffset = Mathf.Sin(Time.time * floatSpeed) * floatRange;
            interactiveToolTip.transform.position = startPosition + new Vector3(0, yOffset);
        }
    }

    private void HandleNpcFlip()
    {
        if (player == null || npc == null) return;

        if (npc.position.x > player.position.x && facingRight)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            facingRight = false;
        }
        else if (npc.position.x < player.position.x && facingRight == false)
        {
            npc.transform.Rotate(0f, 180f, 0f);
            facingRight = true;
        }
    }

    protected virtual void OnTriggerEnter2D(Collider2D collision)
    {
        player =  collision.transform;
        interactiveToolTip.SetActive(true);
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        interactiveToolTip.SetActive(false);
    }
}
