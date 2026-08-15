using UnityEngine;
using System.Collections;

public class Player_VFX : Entity_VFX
{
    [Header("Image Echo VFX")] 
    [Range(0.01f, 0.1f)] 
    [SerializeField] private float imageEchoInterval = 0.05f;
    [SerializeField] private GameObject imageEchoPrefab;
    private Coroutine imageEchoCo;

    public void CreateEffectOf(GameObject effect, Transform target)
    {
        Instantiate(effect, target.position, Quaternion.identity);
    }
    
    public void DoImageEchoEffect(float duration)
    {
        if (imageEchoCo != null)
        {
            StopCoroutine(imageEchoCo);
        }

        imageEchoCo = StartCoroutine(ImageEchoEffectCo(duration));
    }
    
    private IEnumerator ImageEchoEffectCo(float duration)
    {
        float timeTracker = 0;

        while (timeTracker < duration)
        {
            CreateImageEchoCo();
            
            yield return new WaitForSeconds(imageEchoInterval);
            timeTracker += imageEchoInterval;
        }
    }

    private void CreateImageEchoCo()
    {
        GameObject imageEcho = Instantiate(imageEchoPrefab, transform.position, transform.rotation);
        imageEcho.GetComponentInChildren<SpriteRenderer>().sprite = sr.sprite;
    }
}
