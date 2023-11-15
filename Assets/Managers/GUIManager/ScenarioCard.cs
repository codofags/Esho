using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ScenarioCard : MonoBehaviour
{
    public Text CardName;
    public Text CardDescription;
    public RectTransform DescriptionContainer;

    public Animator ScenarioAnimator;

    [Header("Audio")]
    public GameObject AudoPlayer;
    public int CurrentAudioClipId;
    public AudioSource _AudioSource;
    bool AudioIsPlaying;
    public Button ButtonPlayAudio;
    public Image AudioProgressSlider;

    [Header("Video")]
    public VideoPlayer CardVideo;
    bool VideoIsPlaying;
    public Button ButtonPlayVideo;
    bool VideoPlayerIsOpen;

    [Header("Subtitles")]
    public List<string> Subtitles = new List<string>();

    //public void AssignScenario(Scenario _Scenario)
    //{
    //    CurrentAudioClipId = _Scenario.AudioClipId;

    //    PlayAudio();
    //}

    public void CloseCard()
    {
        gameObject.SetActive(false);
        AudioIsPlaying = false;
    }

    public void PlayAudio()
    {
        AudioIsPlaying = !AudioIsPlaying;

        if (AudioIsPlaying)
        {
            //StartCoroutine(ShowSubTitles());

            _AudioSource.clip = AudioManager.Inst.GetScenarioClip(CurrentAudioClipId);

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

    //public void PlayVideo()
    //{
    //    VideoIsPlaying = !VideoIsPlaying;
    //    if (VideoIsPlaying)
    //    {
    //        ButtonPlayVideo.image.sprite = GUIManager.Inst.PauseSprite;
    //        CardVideo.Play();
    //    }
    //    else
    //    {
    //        ButtonPlayVideo.image.sprite = GUIManager.Inst.PlaySprite;
    //        CardVideo.Pause();
    //    }
    //}

    private void Update()
    {
        if (_AudioSource.isPlaying)
        {
            AudioProgressSlider.fillAmount = _AudioSource.time / _AudioSource.clip.length;
        }
    }

    

    public void ShowVideo()
    {
        VideoPlayerIsOpen = !VideoPlayerIsOpen;

        if (VideoPlayerIsOpen)
        {
            ScenarioAnimator.SetTrigger("Open");
        }
        else
        {
            ScenarioAnimator.SetTrigger("Close");
        }        
    }

   
}
