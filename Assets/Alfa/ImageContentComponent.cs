using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageContentComponent : MonoBehaviour
{
    public Image image;

    public void Assign(Sprite sprite)
    {
        image.sprite = sprite;
    }
}
