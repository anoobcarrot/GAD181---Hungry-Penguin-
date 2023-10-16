using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    private static AudioManager instance;
    public static AudioManager Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<AudioManager>();
            }
            return instance;
        }
    }

    public AudioSource backgroundMusic;
    private float musicVolume = 1.0f;

    private void Start()
    {
        if (backgroundMusic != null)
        {
            // Load the saved music volume from PlayerPrefs.
            musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1.0f);
            backgroundMusic.volume = musicVolume;
        }
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        if (backgroundMusic != null)
        {
            backgroundMusic.volume = musicVolume;

            // Save the music volume to PlayerPrefs.
            PlayerPrefs.SetFloat("MusicVolume", musicVolume);
            PlayerPrefs.Save();
        }
    }

    public float GetMusicVolume()
    {
        return musicVolume;
    }
}