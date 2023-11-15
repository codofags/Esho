using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TrackController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI TMP_trackName;

    [SerializeField]
    private TextMeshProUGUI TMP_trackNumber;

    private RectTransform rectTransform;

    private VerticalLayoutGroup verticalLayout;

    private bool isActive;

    public string trackName;
    private string duration;

    private int trackNumber;
    public int trackIndex;

    [SerializeField] private Image trackTypeImage;
    public TrackType trackType;

    public AudioClip audioClip;
    public VideoClip videoClip;

    public void Assign(int trackIndex, int trackNumber, ArScenario scenario)
    {
        Setup();

        this.trackType = scenario.track.trackType;

        audioClip = scenario.track.audioClip;
        videoClip = scenario.track.videoClip;

        switch (trackType)
        {
            case TrackType.Audio: trackTypeImage.sprite = AudioPlayerController.ins.AudioIco; break;
            case TrackType.Video: trackTypeImage.sprite = AudioPlayerController.ins.VideoIco; break;
        }        

        this.trackIndex = trackIndex;

        this.trackNumber = trackNumber;

        this.trackName = TranslateHelper.ins.GetText(scenario.translation);

        if (trackNumber < 10)
        {
            TMP_trackNumber.text = $"0{trackNumber}";
        }
        else
        {
            TMP_trackNumber.text = $"{trackNumber}";
        }

        this.duration = scenario.track.duration;

        TMP_trackName.text = $"{trackName}\n<size=40><color={AudioPlayerController.ins.passiveHex}>{duration}</color></size>";
    }

    public Content_Config content { get; private set; }

    public void Assign(int index, int number, Content_Config content)
    {
        Setup();

        this.content = content;

        //this.trackType = scenario.track.trackType;

        if (content is Content_Audio_Config)
        {
            var audioContent = (Content_Audio_Config)content;

            audioClip = audioContent.GetClip();
            this.duration = audioContent.Duration();
           
        }

        if (content is Content_Video_Config)
        {
            var videoContent = (Content_Video_Config)content;

            videoClip = videoContent.clip;
            this.duration = videoContent.Duration();
            trackTypeImage.sprite = AudioPlayerController.ins.VideoIco;
        }        

        this.trackIndex = index;
        this.trackNumber = number;

        this.trackName = content.GetCaption();//TranslateHelper.ins.GetText(scenario.translation);

        if (trackNumber < 10)
        {
            TMP_trackNumber.text = $"0{trackNumber}";
        }
        else
        {
            TMP_trackNumber.text = $"{trackNumber}";
        }

        TMP_trackName.text = $"{trackName}\n<size=40><color={AudioPlayerController.ins.passiveHex}>{duration}</color></size>";
    }

    private void Setup()
    {
        rectTransform = GetComponent<RectTransform>();

        verticalLayout = GetComponent<VerticalLayoutGroup>();
    }

    public void ButtonPlayPauseTap()
    {
        PlayTrack();
    }

    [SerializeField]
    public GameObject activeBackground;

    [SerializeField]
    private Image playPauseBackGround;

    [SerializeField]
    private Image playPauseIco;

    public void PlayTrack()
    {
        if (AudioPlayerController.ins.currentTrackControler == this)
        {
            AudioPlayerController.ins.PlayPauseButton();

            Debug.Log("set pause");
        }
        else
        {
            AudioPlayerController.ins.PlayTrack(this);

            Debug.Log("set play");
        }        
    }

    public void SetActive()
    {       
        //меняем размер
        rectTransform.sizeDelta = new Vector2(AudioPlayerController.ins.activeWidth, AudioPlayerController.ins.activeHeight);

        //отображаем положку
        activeBackground.SetActive(true);

        //меняем цвет кнопки
        playPauseBackGround.color = AudioPlayerController.ins.whiteColor;

        //меняем кнопку на паузу
        playPauseIco.sprite = AudioPlayerController.ins.pauseSprite;

        //меняем цвет паузы
        playPauseIco.color = AudioPlayerController.ins.blueColor;

        //меняем цвет текста
        TMP_trackName.text = $"{trackName}\n<size=40><color={AudioPlayerController.ins.activeHex}>{duration}</color></size>";

        AudioPlayerController.ins.UpdatePlaListLayoutGroup();
    }

    public void SetPassive()
    {
        isActive = false;

        //меняем размер
        rectTransform.sizeDelta = new Vector2(AudioPlayerController.ins.passiveWidth, AudioPlayerController.ins.passiveHeight);

        //отображаем положку
        activeBackground.SetActive(false);

        //меняем цвет кнопки
        playPauseBackGround.color = AudioPlayerController.ins.blueColor;

        //меняем кнопку на play
        playPauseIco.sprite = AudioPlayerController.ins.playSprite;

        //меняем цвет play
        playPauseIco.color = AudioPlayerController.ins.whiteColor;

        //меняем цвет текста
        TMP_trackName.text = $"{trackName}\n<size=40><color={AudioPlayerController.ins.passiveHex}>{duration}</color></size>";

        AudioPlayerController.ins.UpdatePlaListLayoutGroup();
    }

    public void OpenAudioPlayer()
    {
        //ButtonManager.ins.ClosePlayList();

        ButtonManager.ins.OpenAudioPlayer(trackIndex);

        AudioPlayerController.ins.PlayTrack(this);
        //AudioPlayerController.ins.SetPlayListPosition(-trackIndex);
    }

    internal void SetPause()
    {
        playPauseIco.sprite = AudioPlayerController.ins.pauseSprite; 
    }

    internal void SetPlay()
    {
        playPauseIco.sprite = AudioPlayerController.ins.playSprite; 
    }
}
