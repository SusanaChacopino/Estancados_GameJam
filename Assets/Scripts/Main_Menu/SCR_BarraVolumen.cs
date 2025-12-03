using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{
    public Slider soundSlider;
    public AudioMixer masterMixer;

    void Start()
    {
        float saved = PlayerPrefs.GetFloat("SavedMasterVolume", 100f);
        SetVolume(saved);
    }

    public void SetVolume(float value)
    {
        if (value < 1f)
            value = 0.001f;

        RefreshSlider(value);

        PlayerPrefs.SetFloat("SavedMasterVolume", value);

        float db = Mathf.Log10(value / 100f) * 20f;
        masterMixer.SetFloat("MasterVolume", db);
    }

    public void SetVolumeFromSlider()
    {
        SetVolume(soundSlider.value);
    }

    void RefreshSlider(float value)
    {
        soundSlider.value = value;
    }
}
