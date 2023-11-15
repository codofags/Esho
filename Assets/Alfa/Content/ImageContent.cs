using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ImageContent : Content
{
    public Sprite sprite;

    public ImageContent(Sprite sprite)
    {
        this.sprite = sprite;
    }
}
