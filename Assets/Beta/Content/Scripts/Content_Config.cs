using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//[CreateAssetMenu(fileName = "Content Config", menuName = "BK/Content/Base")]
public class Content_Config : ScriptableObject
{
    [SerializeField] private protected string rusCaption;
    [SerializeField] private protected string engCaption;

    public string GetCaption()
    {
        switch (ButtonManager.ins.language)
        {
            case Language.Rus: { return rusCaption; }
            case Language.Eng: { return engCaption; }
            default: { return rusCaption; }
        }
    }

    public bool showRus = true;
    public bool showEng = true;
}