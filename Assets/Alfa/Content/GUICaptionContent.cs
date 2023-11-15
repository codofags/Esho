using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GUICaptionContent : MonoBehaviour
{
    public TextMeshPro captionText;

    public void AssignText(string text)
    {
        captionText.text = text;
    }
}
