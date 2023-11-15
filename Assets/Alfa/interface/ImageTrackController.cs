using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ImageTrackController : MonoBehaviour
{
    [SerializeField] private Image ico;
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI number;

    [SerializeField] private GameObject locker;

    public RawImage videoImage;

    private TrackType trackType;

    public TrackController trackControler { get; private set; }

    //public void Assign(Sprite trackIco, int number, TrackController trackControler, TrackConfig trackConfig)
    //{
    //    ico.sprite = trackIco;

    //    this.trackType = trackConfig.trackType;

    //    this.number.text = number.ToString();

    //    button.onClick.AddListener(() => SelectTrack());

    //    videoImage.texture = AudioPlayerController.ins.videoTexture;

    //    videoImage.gameObject.SetActive(false);

    //    if(trackType == TrackType.Video)
    //    {
    //        ico.sprite = AudioPlayerController.ins.defaultVideoImage;
    //    }

    //    this.trackControler = trackControler;
    //}

    public Content_Config content { get; private set; }

    [SerializeField] private Image _trackTypeImage;
    [SerializeField] private GameObject _fullScreenMode;
    public void Assign(Sprite trackIco, int number, TrackController trackControler, Content_Config content)
    {
        this.content = content;

        _fullScreenMode.SetActive(false);

        if (content is Content_Audio_Config)
        {
            this.trackType = TrackType.Audio;
            ico.sprite = ((Content_Audio_Config)content).ico;

            _trackTypeImage.sprite = AudioPlayerController.ins.AudioIco;

           
        }
        else
        {
            this.trackType = TrackType.Video;
            ico.sprite = ((Content_Video_Config)content).ico;

            _trackTypeImage.sprite = AudioPlayerController.ins.VideoIco;

            
        }

        this.number.text = number.ToString();

        button.onClick.AddListener(() => SelectTrack());

        videoImage.texture = AudioPlayerController.ins.videoTexture;

        videoImage.gameObject.SetActive(false);

        this.trackControler = trackControler;
    }

    public void SelectTrack()
    {
        if (trackType == TrackType.Audio)
        {
            AudioPlayerController.ins.PlayTrack(trackControler);
        }

        if (trackType == TrackType.Video)
        {
            videoImage.gameObject.SetActive(true);

            if (AudioPlayerController.ins.currentTrackControler != trackControler)
            {
                AudioPlayerController.ins.PlayTrack(trackControler);

                _fullScreenMode.SetActive(true);
            }
            else
            {
                if (!AudioPlayerController.ins.videoPlayer.isPlaying)
                {
                    AudioPlayerController.ins.PlayTrack(trackControler);
                }
                else
                {
                    ButtonManager.ins.OpenVideoPlayer();
                }
            }           
        }

        Unlock();
    }

    public void Lock()
    {
        if (trackType == TrackType.Video)
        {
            videoImage.gameObject.SetActive(false);
        }

        _fullScreenMode.SetActive(false);

        locker.SetActive(true);
    }

    public void Unlock()
    {
        if(trackType == TrackType.Video)
        {
            videoImage.gameObject.SetActive(true);
            _fullScreenMode.SetActive(true);
        }

        locker.SetActive(false);
    }
}
