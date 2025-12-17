using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class VolumeController : MonoBehaviour
{

    public Slider soundSlider;      // Tu Slider de la UI
    public AudioMixer masterMixer;  // Tu asset de AudioMixer

    // Este nombre debe ser IDÉNTICO al nombre del parámetro en el Mixer
    private const string MIXER_PARAM = "MasterVolume";

    void Start()
    {

        float savedVol = PlayerPrefs.GetFloat("SavedMasterVolume", 1f); //el 1f es = al 100%

        soundSlider.value = savedVol;

        SetVolume(savedVol);


        soundSlider.onValueChanged.AddListener(SetVolume);
    }


    public void SetVolume(float sliderValue)
    {

        PlayerPrefs.SetFloat("SavedMasterVolume", sliderValue);


        float db;

        if (sliderValue <= 0.1f)
        {
            db = -800f; // Silencio total (evitamos el error matemático de log(0))
        }
        else
        {

            db = Mathf.Log10(sliderValue) * 20;
        }

        // Enviamos el valor calculado al Mixer
        masterMixer.SetFloat(MIXER_PARAM, db);
    }
}