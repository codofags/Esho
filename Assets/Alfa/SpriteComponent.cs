using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpriteComponent : MonoBehaviour
{
    public Image image;

    internal void Assign(Sprite s)
    {
        image.sprite = s;
    }
}
