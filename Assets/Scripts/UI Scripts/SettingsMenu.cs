using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsMenu : MonoBehaviour
{
    public AudioMixer audioMixer;


    public TMP_Dropdown resolutionDropdown;

    public Slider volumeSlider;
    public Slider bgmSlider;
    public Slider sfxSlider;

    private List<string> commonResolutions = new List<string>
    {
        "1920x1080",
        "1680x1050",
        "1600x900",
        "1440x900",
        "1366x768",
        "1280x1024",
        "1280x800",
        "1280x720"
    };
    private int currentResolutionIndex;

    public void SetFullScreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;

    }

    private void Start()
    {




        for (int i = 0; i < commonResolutions.Count; i++)
        {
            string resolutionString = commonResolutions[i];
            string[] parts = resolutionString.Split('x');
            int width = int.Parse(parts[0]);
            int height = int.Parse(parts[1]);
            if (width == Screen.currentResolution.width && height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
       
        ResolutionSetup();
        InitializeVolumeSliders();
        

    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void InitializeVolumeSliders()
    {
        float currentVolume;
        float currentBGM;
        float currentSFX;

        audioMixer.GetFloat("volume", out currentVolume);
        audioMixer.GetFloat("BGM", out currentBGM);
        audioMixer.GetFloat("SFX", out currentSFX);

        // Convert from decibels back to slider value (0-1 range)
        volumeSlider.value = (currentVolume + 80f) / 80f;
        bgmSlider.value = (currentBGM + 80f) / 80f;
        sfxSlider.value = (currentSFX + 80f) / 80f;
    }


    public void SetVolume(float volume)
    {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("volume", dB);
    }

    public void SetBGMVolume(float volume)
    {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("BGM", dB);
    }

    public void SetSFXVolume(float volume)
    {
        float dB = Mathf.Lerp(-80f, 0f, volume);
        audioMixer.SetFloat("SFX", dB);
    }


    public void SetResolution(int resolutionIndex)
    {
        string resolutionString = commonResolutions[resolutionIndex];
        string[] parts = resolutionString.Split('x');
        int width = int.Parse(parts[0]);
        int height = int.Parse(parts[1]);
        Screen.SetResolution(width, height, Screen.fullScreen);
    }

    private void ResolutionSetup() 
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(commonResolutions);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

}
