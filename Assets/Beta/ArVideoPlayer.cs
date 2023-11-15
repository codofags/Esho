using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class ArVideoPlayer : MonoBehaviour
{

    public static ArVideoPlayer ins;
    void Start()
    {
        ins = this;

        ResizeVideoTexture();

        videoTimeLineSize = videoTimeLine.rect.size.y;
    }

    [SerializeField] private RectTransform videoTextureContainer;
    [SerializeField] private RawImage videoPlayerImage;
    //[SerializeField] private RawImage playerControllerVideoImage;
    public void ResizeVideoTexture()
    {
        var videoContainerSize = videoTextureContainer.rect.size;

        videoPlayerImage.GetComponent<RectTransform>().sizeDelta = new Vector2(videoContainerSize.y, videoContainerSize.x);
    }

    [SerializeField] private VideoPlayer _videoPlayer;
    [SerializeField] private GameObject _arVideoPlayer_GO;
    public void Play()
    {
        _arVideoPlayer_GO.SetActive(true);
        _videoPlayer.Play();        
        ChangePlayPauseImage();

        AudioPlayerController.ins.StopPlayer();
    }

    public void PlayPauseButton()
    {
        if(_videoPlayer.isPlaying)
        {
            _videoPlayer.Pause();
        }
        else
        {
            _videoPlayer.Play();  
        }

        ChangePlayPauseImage();
    }

    private void Update()
    {
        UpdateVideoPlayerTimeLine();
    }


    [SerializeField] private RectTransform videoTimeLine;
    private float videoTimeLineSize;
    [SerializeField] private RectTransform videoTimePin;
    [SerializeField] private TextMeshProUGUI videoTrackTime;
    [SerializeField] private TextMeshProUGUI videoTrackDuration;
    private void UpdateVideoPlayerTimeLine()
    {
        if (timeLinePinIsLocked) return;

            if (_videoPlayer.isPlaying)
        {
            var trackPosition = (float)(_videoPlayer.time / _videoPlayer.clip.length);

            videoTimePin.anchoredPosition = new Vector2(0, -trackPosition * videoTimeLineSize);

            videoTrackTime.text = GetTimeFromSeconds((float)_videoPlayer.time);
            videoTrackDuration.text = GetTimeFromSeconds((float)_videoPlayer.clip.length);
        }
    }

    internal void SetTimeLine(float progress)
    {
        if (progress < 0) progress = 0;
        if (progress >= 0.99f) progress = 0.99f;      

        if (_videoPlayer.isPlaying)
        {
            _videoPlayer.time = _videoPlayer.clip.length * progress;
        }
        if (!_videoPlayer.isPlaying && _videoPlayer.clip != null)
        {
            _videoPlayer.Play();
            _videoPlayer.time = _videoPlayer.clip.length * progress;
        }
    }

    private bool timeLinePinIsLocked;

    public void LockTimeLinePin()
    {
        timeLinePinIsLocked = true;
    }

    public void StartUnlockTimeLinePin()
    {
        StartCoroutine(UnlockTimeLinePin());
    }

    IEnumerator UnlockTimeLinePin()
    {
        yield return new WaitForSeconds(0.5f);

        timeLinePinIsLocked = false;
    }

    private string GetTimeFromSeconds(float seconds)
    {
        var result = "";

        int resultMinutes = (int)(seconds / 60);
        var resultSeconds = seconds - resultMinutes * 60;

        if (resultMinutes == 0)
        {
            result = "00:";
        }
        else if (resultMinutes > 0 && resultMinutes < 10)
        {
            result = $"0{resultMinutes}:";
        }

        if (resultSeconds < 10)
        {
            result += "0";
        }

        result += resultSeconds.ToString("f0");

        return result;
    }

    public void Stop()
    {
        _videoPlayer.Stop();
        _arVideoPlayer_GO.SetActive(false);
        ChangePlayPauseImage();
    }

    [SerializeField] private Image _playPauseImage;
    private void ChangePlayPauseImage()
    {
        if (_videoPlayer.isPlaying)
        {
            _playPauseImage.sprite = AudioPlayerController.ins.pauseSprite;
        }
        else
        {
            _playPauseImage.sprite = AudioPlayerController.ins.playSprite;
        }
    }
}
