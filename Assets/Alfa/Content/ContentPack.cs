using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ContentPack 
{
    public List<Content> content = new List<Content>();

    internal void AddConent(Content content)
    {
        this.content.Add(content);
    }
}
