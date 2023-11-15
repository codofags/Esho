using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "Text Content", menuName = "BK/Content/Text")]
public class Content_Text_Config : Content_Config
{
    [SerializeField] private string rusText;
    [SerializeField] private string engText;
    public List<Sprite> ico;
    public List<Content_Config> subContent = new List<Content_Config>();

    public string GetText()
    {
        switch (ButtonManager.ins.language)
        {
            case Language.Rus: { return rusText; }
            case Language.Eng: { return engText; }
            default: { return rusText; }
        }
    }         
}
