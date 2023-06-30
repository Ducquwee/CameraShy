using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Audio;
public class OptionsScreen : MonoBehaviour
{
    public Toggle fullscreenTog, vsyncTog;

    public List<ResItem> resolutions = new List<ResItem>();
    private int selectedResolution;

    public TMP_Text resolutionLabel;

    public AudioMixer theMixer;

    public Slider masterSlider, musicSlider, SFXSlider;
    void Start()
    {
        fullscreenTog.isOn = Screen.fullScreen;

        if (QualitySettings.vSyncCount == 0)
        {
            vsyncTog.isOn = false;
        }
        else
        {
            vsyncTog.isOn = true;
        }

        bool foundRes = false;
        for (int i = 0; i < resolutions.Count; i++)
        {
            if (Screen.width == resolutions[i].horizontal && Screen.height == resolutions[i].vertical)
            {
                foundRes = true;

                selectedResolution = i;

                UpdateResLabel();
            }
        }

        if (!foundRes)
        {
            ResItem newRes = new ResItem();
            newRes.horizontal = Screen.width;
            newRes.vertical = Screen.height;

            resolutions.Add(newRes);
            selectedResolution = resolutions.Count - 1;

            UpdateResLabel();
        }

        float vol = -5f;
        theMixer.GetFloat("MasterVol", out vol);
        masterSlider.value = vol;

        theMixer.GetFloat("MusicVol", out vol);
        masterSlider.value = vol;

        theMixer.GetFloat("SFXVol", out vol);
        masterSlider.value = vol;


    }

    void Update()
    {
        SetMasterVol();
        SetMusicVol();
        SetSFXVol();
    }

    public void UpdateResLabel()
    {
        resolutionLabel.text = resolutions[selectedResolution].horizontal.ToString() + " x " + resolutions[selectedResolution].vertical.ToString();
    }
    public void ResLeft()
    {
        selectedResolution--;
        if (selectedResolution < 0)
        {
            selectedResolution = resolutions.Count - 1;
        }
        UpdateResLabel();
    }

    public void ResRight()
    {
        selectedResolution++;
        if (selectedResolution > resolutions.Count - 1)
        {
            selectedResolution = 0;
        }
        UpdateResLabel();
    }


    public void ApplyGraphics()
    {
        //Screen.fullScreen = fullscreenTog.isOn;

        if (vsyncTog.isOn)
        {
            QualitySettings.vSyncCount = 1;
        }

        else
        {
            QualitySettings.vSyncCount = 0;
        }

        Screen.SetResolution(resolutions[selectedResolution].horizontal, resolutions[selectedResolution].vertical, fullscreenTog.isOn);
    }

    public void SetMasterVol()
    {
        if (masterSlider.value > -20)
        {
            theMixer.SetFloat("MasterVol", masterSlider.value);
        }

        else
        {
            theMixer.SetFloat("MasterVol", -80);
        }

        PlayerPrefs.SetFloat("MaterVol", masterSlider.value);

    }

    public void SetMusicVol()
    {
        if (musicSlider.value > -20)
        {
            theMixer.SetFloat("MusicVol", musicSlider.value);
        }

        else
        {
            theMixer.SetFloat("MusicVol", -80);
        }

        PlayerPrefs.SetFloat("MusicVol", musicSlider.value);

    }

    public void SetSFXVol()
    {
        if (SFXSlider.value > -20)
        {
            theMixer.SetFloat("SFXVol", SFXSlider.value);
        }

        else
        {
            theMixer.SetFloat("SFXVol", -80);
        }

        PlayerPrefs.SetFloat("SFXVol", SFXSlider.value);

    }

}




[System.Serializable]
public class ResItem
{
    public int horizontal, vertical;
}