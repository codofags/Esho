using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Inst;

    public AudioClip Clip1;
    public AudioClip Clip2;
    public AudioClip Clip3;
    public AudioClip Clip4;

    public AudioClip ScenarioClip1;
    public AudioClip ScenarioClip2;
    public AudioClip ScenarioClip3;

    AudioSource AudioPlayer;

    public Image TimeBar;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        AudioPlayer = GetComponent<AudioSource>();
    }

    public AudioClip GetClip(int _Index)
    {
        switch (_Index)
        {
            case 1: return Clip1;
            case 2: return Clip2;
            case 3: return Clip3;
            case 4: return Clip4;
            default: return null;
        }
    }

    public AudioClip GetScenarioClip(int _Index)
    {
        switch (_Index)
        {
            case 1: return ScenarioClip1;
            case 2: return ScenarioClip2;
            case 3: return ScenarioClip3;
            default: return null;
        }
    }

    private void Update()
    {
        if (AudioPlayer.isPlaying)
        {
            TimeBar.fillAmount = AudioPlayer.time / AudioPlayer.clip.length;
        }
        
    }

    public void PlayerButton()
    {
        if (AudioPlayer.isPlaying) AudioPlayer.Pause();
        else if(!AudioPlayer.isPlaying && AudioPlayer.clip != null) AudioPlayer.UnPause();
    }

    public void PlayAudio(int ClipIndex)
    {
        StopAudio();

        var SelectedClip = GetClip(ClipIndex);
        if (SelectedClip == null)
        {
            Debug.Log("no clip");
            return;
        }
        AudioPlayer.clip = SelectedClip;
        AudioPlayer.Play();
    }

    public void StopAudio()
    {
        if (AudioPlayer.isPlaying) AudioPlayer.Stop();
    }

    public void PauseAudio()
    {
        if (AudioPlayer.isPlaying) AudioPlayer.Pause();
    }
}
