using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class UI_Options : MonoBehaviour
{
    private Player player;
    [SerializeField] private Toggle healthBarToggle;

    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private float mixerMultiplier = 25;

    [Header("BGM Volume Settings")] 
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private string bgmParameter;
    
    [Header("SFX Volume Settings")]
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private string sfxParameter;

    private void Start()
    {
        player = FindFirstObjectByType<Player>();
        
        healthBarToggle.onValueChanged.AddListener(OnHealthBarToggleChanged);
    }

    public void BGMSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(bgmParameter, newValue);
    }

    public void SFXSliderValue(float value)
    {
        float newValue = Mathf.Log10(value) * mixerMultiplier;
        audioMixer.SetFloat(sfxParameter, newValue);
    }
    
    private void OnHealthBarToggleChanged(bool isOn)
    {
        player.health.EnableHealthBar(isOn);
    }

    public void GoMainMenuButton() => GameManager.instance.ChangeScene("MainMenu", RespawnType.NonSpecific);

    private void OnEnable()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.6f);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.6f);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetFloat(bgmParameter, sfxSlider.value);
        PlayerPrefs.SetFloat(sfxParameter, sfxSlider.value);
    }

    public void LoadUpVolume()
    {
        bgmSlider.value = PlayerPrefs.GetFloat(bgmParameter, 0.6f);
        sfxSlider.value = PlayerPrefs.GetFloat(sfxParameter, 0.6f);
    }
}
