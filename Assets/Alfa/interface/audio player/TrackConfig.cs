using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[Serializable]
public class TrackConfig 
{
    public int trackId;

    public TrackType trackType;

    public AudioClip audioClip;
    public VideoClip videoClip;

    public string url;

    //public TextTranslation nameTranslation;

    public string duration;
    public Sprite trackSprite;

    public void Setup()
    {
        if (audioClip != null) trackType = TrackType.Audio; else trackType = TrackType.Video;
        duration = GetDuration();
    }

    //public void Assign()
    //{
    //    if (audioClip != null) trackType = TrackType.Audio; else trackType = TrackType.Video;
    //}

    private string GetDuration()
    {
        var duration = 0f;

        if (audioClip != null)
            duration = audioClip.length;
        else if (videoClip != null)
            duration = (float)videoClip.length;
        else
            return "no clip";

        var min = (int)duration / 60;
        var sec = duration - min * 60;

        var result = "";

        if (min > 0)
        {
            switch (ButtonManager.ins.GetLanguage())
            {
                case Language.Rus:
                    {
                        result += $"{min} мин.";
                    }
                    break;
                case Language.Eng:
                    {
                        result += $"{min} min.";
                    }
                    break;
            }
           
        }

        if (sec > 0)
        {
            switch (ButtonManager.ins.GetLanguage())
            {
                case Language.Rus:
                    {
                        result += $"{sec.ToString("f0")} сек.";
                    }
                    break;
                case Language.Eng:
                    {
                        result += $"{sec.ToString("f0")} sec.";
                    }
                    break;
            }           
        }

        return result;
    }

   
}
