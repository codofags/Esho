using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Audio Content", menuName = "BK/Content/Audio")]
public class Content_Audio_Config : Content_Config
{
    public Sprite ico;

    [SerializeField] private AudioClip rusClip;
    [SerializeField] private AudioClip engClip;

    public string Duration()
    {
        var duration = 0f;

        switch (ButtonManager.ins.language)
        {
            case Language.Rus: { duration = rusClip.length; } break;
            case Language.Eng: { duration = engClip.length; } break;
            default: { duration = rusClip.length; } break;
        }     

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

    public AudioClip GetClip()
    {
        switch (ButtonManager.ins.language)
        {
            case Language.Rus: { return rusClip; }
            case Language.Eng: { return engClip; }
            default: { return rusClip; }
        }
    }
}
