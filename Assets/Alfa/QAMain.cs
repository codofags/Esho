using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QAMain : MonoBehaviour
{
    private QAController[] qaControllers;

    private void Start()
    {
        qaControllers = GetComponentsInChildren<QAController>();

        foreach (var q in qaControllers)
        {
            q.SetMain(this);
        }
    }

    public void ResetQAControllers()
    {
        foreach (var q in qaControllers)
        {
            q.CloseWindow();
        }
    }
}
