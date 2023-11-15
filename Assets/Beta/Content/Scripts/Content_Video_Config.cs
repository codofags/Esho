using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "Video Content", menuName = "BK/Content/Video")]
public class Content_Video_Config : Content_Config
{
    public Sprite ico;

    public VideoClip clip;

    public AudioClip _audio;

    public string Duration()
    {
        var duration = clip.length;

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
