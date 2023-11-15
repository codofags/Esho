using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextAutoSize : MonoBehaviour
{
    [SerializeField] private Transform ParentContainer;

    private void OnEnable()
    {
        //Canvas.ForceUpdateCanvases();

        var selfRect = GetComponent<RectTransform>().rect;

        var parentRect = ParentContainer.GetComponent<RectTransform>().rect;
        
        parentRect.size = selfRect.size;

        Debug.Log($"{selfRect.size} => {parentRect.size}");


    }
}
