using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WarningController : MonoBehaviour
{
    public Text Caption;
    public Text Description;

    public void AssignWarning(string NewCaption, string NewDescription)
    {
        Caption.text = NewCaption;
        Description.text = NewDescription;
    }

    public void HideWarning()
    {
        gameObject.SetActive(false);
    }
}
