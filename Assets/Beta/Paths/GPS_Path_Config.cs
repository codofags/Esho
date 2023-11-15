using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GPS Path Config", menuName = "BK/GPS Path/Path")]
public class GPS_Path_Config : ScriptableObject
{
    public TextTranslation pathName;

    public TextTranslation duration;

    public string _rusObjectCount;
    public string _engObjectCount;
    public string GetObjectCountText()
    {
        switch (ButtonManager.ins.language)
        {
            case Language.Rus: { return _rusObjectCount; }
            case Language.Eng: { return _engObjectCount; }
            default: { return _rusObjectCount; }
        }
    }

    public bool isAR;

    public List<string> searchKeys = new List<string>();

    public List<Sprite> sprites = new List<Sprite>();

    public List<GPS_Point_Config> points = new List<GPS_Point_Config>();

    public List<Content_Config> content = new List<Content_Config>();

    public Sprite mapSprite;


}
