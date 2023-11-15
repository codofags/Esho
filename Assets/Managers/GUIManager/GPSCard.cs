using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GPSCard : MonoBehaviour
{
    public Text CardName;
    public Text CardDescription;
    public RectTransform DescriptionContainer;

    [Header("Audio")]
    public int CurrentAudioClipId;
    public AudioSource _AudioSource;
    bool AudioIsPlaying;
    public Button ButtonPlayAudio;
    public Image AudioProgressSlider;

    [Header("Video")]
    public VideoPlayer CardVideo;
    bool VideoIsPlaying;
    public Button ButtonPlayVideo;

    public List<string> Subtitles = new List<string>();

    [Header("Buttons")]
    public GameObject Button3D;

    public void AssignGPSPoint(GPSPoint _GPSPoint)
    {
        //CardName.text = _GPSPoint.PointName;
        //CardDescription.text = _GPSPoint.PointDescription;

        //CurrentAudioClipId = _GPSPoint.AudioClipId;

        //if (_GPSPoint.ModelId > 0)
        //{
        //    Button3D.SetActive(true);
        //}
        //else
        //{
        //    Button3D.SetActive(false);
        //}
    }

    public void CloseCard()
    {
        gameObject.SetActive(false);
        AudioIsPlaying = false;
    }

    public void ShowCardDescription()
    {
        CardDescription.enabled = !CardDescription.enabled;
    }

    public void PlayAudio()
    {
        AudioIsPlaying = !AudioIsPlaying;

        if (AudioIsPlaying)
        {
            StartCoroutine(ShowSubTitles());

            _AudioSource.clip = AudioManager.Inst.GetClip(CurrentAudioClipId);

            ButtonPlayAudio.image.sprite = GUIManager.Inst.PauseSprite;
            _AudioSource.Play();
            CardVideo.Play();
        }
        else
        {
            ButtonPlayAudio.image.sprite = GUIManager.Inst.PlaySprite;
            _AudioSource.Pause();
            CardVideo.Pause();
        }
        
    }

    IEnumerator ShowSubTitles()
    {
        //отображаем субтитры
        CardDescription.text = Subtitles[CurrentAudioClipId - 1];

        yield return new WaitForSeconds(0.1f);

        DescriptionContainer.sizeDelta = CardDescription.GetComponent<RectTransform>().sizeDelta;
    }

    public void PlayVideo()
    {
        VideoIsPlaying = !VideoIsPlaying;
        if (VideoIsPlaying)
        {
            ButtonPlayVideo.image.sprite = GUIManager.Inst.PauseSprite;
            CardVideo.Play();
        }
        else
        {
            ButtonPlayVideo.image.sprite = GUIManager.Inst.PlaySprite;
            CardVideo.Pause();
        }
    }

    private void Update()
    {
        if (_AudioSource.isPlaying)
        {
            AudioProgressSlider.fillAmount = _AudioSource.time / _AudioSource.clip.length;
        }
    }
}
