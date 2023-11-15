using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class MapImageContent : Content
{
    public Sprite sprite;

    public MapImageContent(Sprite sprite)
    {
        this.sprite = sprite;
    }
}