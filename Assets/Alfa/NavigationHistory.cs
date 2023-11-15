using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NavigationHistory
{
    private List<Action> navigations = new List<Action>();

    public void AddHistoryPoint(Action point)
    {
        navigations.Add(point);

        //Debug.Log(point.Method.Name);
    }

    public void DoBackNavigation()
    {
        if (navigations.Count == 0) return;

        var targetPoint = navigations[navigations.Count - 2];
        var lastPoint = navigations[navigations.Count - 1];

        targetPoint();

        navigations.Remove(targetPoint);
        navigations.Remove(lastPoint);
    }
}
