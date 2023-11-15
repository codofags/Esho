using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAController : MonoBehaviour
{
    [SerializeField] private int minY = 215;
    [SerializeField] private int maxY = 600;

    private QAMain main;

    public void SetMain(QAMain main)
    {
        this.main = main;
    }

   
    [SerializeField] private RectTransform buttonImage;
    private bool isOpen;

    //public void ButtonClick()
    //{
    //    main.ResetQAControllers();

    //    isOpen = !isOpen;

    //    if (isOpen)
    //    {
    //        OpenWindow();
    //    }
    //    else
    //    {
    //        CloseWindow();
    //    }
    //}

    public void OpenWindow()
    {
        main.ResetQAControllers();
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, maxY);
        buttonImage.localScale = new Vector3(1, -1, 1);
    }

    public void CloseWindow()
    {
        GetComponent<RectTransform>().sizeDelta = new Vector2(0, minY);
        buttonImage.localScale = new Vector3(1, 1, 1);
    }
}
