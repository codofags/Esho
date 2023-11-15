using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LocalPinInfo : MonoBehaviour
{
    public Image PinImage;

    public Text PictureDescription;

    public Text PinCaption;
    public Text PinDescription;

    public void SetInfo(LocalPinController LPin)
    {
        PinImage.sprite = LPin.InfoSprite;

        PictureDescription.text = LPin.InfoSpriteDescription;

        PinCaption.text = LPin.InfoCaption;
        PinDescription.text = LPin.InfoDescription;
    }
}
