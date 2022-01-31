using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public enum VolumeMixer { Master, Music, SFX }

public class VolumeControl : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioMixer mixer;
    float volumeMaster;
    float volumeMusic;
    float volumeSfx;

    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("Master")){
            PlayerPrefs.SetFloat(VolumeMixer.Master.ToString(), 1);
            PlayerPrefs.SetFloat(VolumeMixer.Music.ToString(), 1);
            PlayerPrefs.SetFloat(VolumeMixer.SFX.ToString(), 1);
        }

        volumeMaster = PlayerPrefs.GetFloat(VolumeMixer.Master.ToString());
        volumeMusic = PlayerPrefs.GetFloat(VolumeMixer.Music.ToString());
        volumeSfx = PlayerPrefs.GetFloat(VolumeMixer.SFX.ToString());

        masterSlider.value = volumeMaster;
        musicSlider.value = volumeMusic;
        sfxSlider.value = volumeSfx;

        masterSlider.onValueChanged.AddListener((float val) => SetVolume(VolumeMixer.Master, val));
        musicSlider.onValueChanged.AddListener((float val) => SetVolume(VolumeMixer.Music, val));
        sfxSlider.onValueChanged.AddListener((float val) => SetVolume(VolumeMixer.SFX, val));
    }

    // Update is called once per frame
    void Update()
    {

    }

    void SetVolume(VolumeMixer vMix, float val)
    {
        switch (vMix)
        {
            case VolumeMixer.Master:
                if (val < volumeMusic)
                {
                    volumeMusic = val;
                    musicSlider.value = volumeMusic;
                    mixer.SetFloat(VolumeMixer.Music.ToString(), ConvertToDecibel(val));
                    PlayerPrefs.SetFloat(VolumeMixer.Music.ToString(), val);
                }

                if (val < volumeSfx)
                {
                    volumeSfx = val;
                    sfxSlider.value = volumeSfx;
                    mixer.SetFloat(VolumeMixer.SFX.ToString(), ConvertToDecibel(val));
                    PlayerPrefs.SetFloat(VolumeMixer.SFX.ToString(), val);
                }

                volumeMaster = val;
                masterSlider.value = volumeMaster;

                break;
            case VolumeMixer.Music:
                if (val > volumeMaster)
                {
                    val = volumeMaster;
                }

                volumeMusic = val;
                musicSlider.value = volumeMusic;

                break;
            case VolumeMixer.SFX:
                if (val > volumeMaster)
                {
                    val = volumeMaster;
                }

                volumeSfx = val;
                sfxSlider.value = volumeSfx;

                break;
            default:
                break;
        }

        mixer.SetFloat(vMix.ToString(), ConvertToDecibel(val));
        PlayerPrefs.SetFloat(vMix.ToString(), val);
    }

    public float ConvertToDecibel(float val){
        return Mathf.Log10(Mathf.Max(val, 0.0001f))*20f;
     }
}
