using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MusicTrack
{
    public string trackName;
    public AudioClip audioClip;
}
public class MusicManager : MonoBehaviour
{
    public List<MusicTrack> musicTracks;
    private AudioSource audioSource;
    private int currentTrackIndex;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        currentTrackIndex = 0;
    }
    
    private void Start()
    {
        audioSource.clip = musicTracks[currentTrackIndex].audioClip;
        audioSource.Play();
    }

    public void PlayTrack(int trackIndex)
    {
        currentTrackIndex = trackIndex;
        audioSource.Stop();
        audioSource.clip = musicTracks[currentTrackIndex].audioClip;
        audioSource.Play();
    }

    public void PlayerDeathMusic()
    {
        PlayTrack(1); // Assuming "deathmusic" is at index 1
    }

    public void LevelCompleteMusic()
    {
        PlayTrack(2); // Assuming "Level complete music" is at index 2
    }

    public void StopMusic()
    {
        audioSource.volume = 0f;
    }

}
