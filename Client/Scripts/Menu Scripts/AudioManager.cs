using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public AudioMixer theMixer;

    void Start()
    {
        string[] volumeKeys = { "MasterVol", "MusicVol", "EffectsVol" };

        foreach (string key in volumeKeys)
        {
            if (PlayerPrefs.HasKey(key))
            {
                theMixer.SetFloat(key, PlayerPrefs.GetFloat(key));
            }
        }

    }
}
