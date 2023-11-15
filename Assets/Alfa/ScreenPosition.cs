using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenPosition : MonoBehaviour
{    
    [SerializeField] private Vector3 globalPosition;

    [SerializeField] private bool UseAnchor = false;
    [SerializeField] private Vector3 anchoredPosition;

    [SerializeField] private bool useHalfScreen;

    void Start()
    {
        SetPosition();

        //Debug.Log($"{GetComponent<RectTransform>().anchoredPosition} => {anchoredPosition}");
       // Debug.Log($"{GetComponent<RectTransform>().position} ");
    }

    private void OnEnable()
    {
        //Debug.Log("enable");
        ResetPosition();
    }  

    private void SetPosition()
    {
        if (UseAnchor)
        {
            //Debug.Log($"{GetComponent<RectTransform>().anchoredPosition} => {anchoredPosition}");

            GetComponent<RectTransform>().anchoredPosition = anchoredPosition;

           // Debug.Log($"{GetComponent<RectTransform>().anchoredPosition} => {anchoredPosition}");
        }
        else
        {
            if (useHalfScreen)
            {
                globalPosition.x = Screen.width / 2;
            }

            transform.position = globalPosition;
        }
    }

    private void ResetPosition()
    {
        var parent = transform.parent.gameObject;
        parent.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}
