using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PoiAudioComponent : MonoBehaviour
{
    [SerializeField] private Button PlayPauseButton;
    [SerializeField] private Button OpenPlayListButton;

    [SerializeField] private TextMeshProUGUI trackName;

    private GPSPoint point;

    [SerializeField] private Image PlayPauseImage;

    private bool isPlaing=true;

    public void Assign(GPS_Point_Config point)
    {
        if (point.content != null && point.content.Count > 0)
        {
            var playFirstTrack = AutoGPSPlay.ins.useAutoGid;

            AudioPlayerController.ins.SetupPlayList(point, playFirstTrack);

            trackName.text = point.content[0].GetCaption();
        }
    }

    public void OpenPlayList()
    {
        ButtonManager.ins.OpenPlayList();
    }

    public void PlayTrack()
    {
        isPlaing = !isPlaing;

        if (isPlaing)
        {
            PlayPauseImage.sprite = AudioPlayerController.ins.pauseSprite;
        }
        else
        {
            PlayPauseImage.sprite = AudioPlayerController.ins.playSprite;
        }

        var track = AudioPlayerController.ins.trackControllers[0];

        if (AudioPlayerController.ins.currentTrackControler == track)
        {
            AudioPlayerController.ins.PlayPauseButton();
        }         
        else
        {
            AudioPlayerController.ins.PlayTrack(track);
        }
        
    }
}
