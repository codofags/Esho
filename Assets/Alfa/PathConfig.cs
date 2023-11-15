using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PathConfig
{
    public TextTranslation translation;
    public float pathTime;
    public int pathObjectsCount;
    public bool isAR;

    public List<string> searchKeys = new List<string>();

    public List<Sprite> sprites = new List<Sprite>();

    public List<PointConfig> points = new List<PointConfig>();

    //content
    public List<Content> content = new List<Content>();

    public Sprite mapSprite;
}
