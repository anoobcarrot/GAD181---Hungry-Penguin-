using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeManager : MonoBehaviour
{
    public Slider musicVolumeSlider;
    public GameObject optionsMenu;

    private void Start()
    {
        optionsMenu.SetActive(false);
        musicVolumeSlider.value = AudioManager.Instance.GetMusicVolume();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            ToggleOptionsMenu();
        }
    }

    public void ToggleOptionsMenu()
    {
        optionsMenu.SetActive(!optionsMenu.activeSelf);
    }

    public void OnMusicVolumeChanged(float volume)
    {
        Debug.Log("Slider Value: " + volume);
        AudioManager.Instance.SetMusicVolume(volume);
    }
}
