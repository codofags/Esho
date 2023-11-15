using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class TextContent : Content
{
    public string text;

    public TextContent(string text)
    {
        this.text = text;
    }
}
