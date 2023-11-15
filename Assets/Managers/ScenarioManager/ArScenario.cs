using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArScenario 
{
    public TextTranslation translation;

    [HideInInspector]
    public PointConfig point;

    public int modelState;

    public Vector3 scenarioPosition;

    public TrackConfig track;

    public bool visible;
}
