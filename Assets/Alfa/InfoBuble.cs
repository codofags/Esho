using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfoBuble : MonoBehaviour
{
    public Text Content;

    public void SetContent(string NewContent)
    {
        Content.text = NewContent;
    }

    public void HideBuble()
    {
        gameObject.SetActive(false);
    }

}
