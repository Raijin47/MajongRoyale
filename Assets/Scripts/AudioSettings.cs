using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class AudioSettings : MonoBehaviour
{
    [SerializeField] private AudioMixerGroup _mixer;
    [SerializeField] private TextMeshProUGUI _sfxText, _musicText;
    [SerializeField] private Button _sfxButton, _musicButton;

    private const float On = 0;
    private const float Off = -80;

    private readonly string SFXSave = "SFXVolume";
    private readonly string SFX = "SFX";
    private readonly string MusicSave = "MusicVolume";
    private readonly string Music = "Music";

    private bool _sfxValue;
    private bool _musicValue;

    public bool SFXValue { get => _sfxValue; set { _sfxValue = value; ToggleSFX(value); } }
    public bool MusicValue { get => _musicValue; set { _musicValue = value; ToggleMusic(value); } }

    private void Start()
    {
        _sfxButton.onClick.AddListener(() => { SFXValue = !SFXValue; });
        _musicButton.onClick.AddListener(() => { MusicValue = !MusicValue; });

        SFXValue = PlayerPrefs.GetInt(SFXSave, 1) == 1;
        MusicValue = PlayerPrefs.GetInt(MusicSave, 1) == 1;
    }

    private void ToggleSFX(bool value)
    {
        _sfxText.text = $"SOUND: {TextUtility.GetColorText(value ? "ON" : "OFF", 2)}";
        _mixer.audioMixer.SetFloat(SFX, value ? On : Off);
        PlayerPrefs.SetInt(SFXSave, value ? 1 : 0);
    }

    private void ToggleMusic(bool value)
    {
        _musicText.text = $"MUSIC: {TextUtility.GetColorText(value ? "ON" : "OFF", 2)}";
        _mixer.audioMixer.SetFloat(Music, value ? On : Off);
        PlayerPrefs.SetInt(MusicSave, value ? 1 : 0);
    }
}