using Google.Maps.Coord;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class PointConfig 
{
    public TextTranslation translation;
    public float time;
    public int objectsCount;
    public bool isAR;

    [HideInInspector]
    public PathConfig path;

    public LatLng coord;

    public List<string> searchKeys = new List<string>();

    public List<Sprite> sprites = new List<Sprite>();

    public float radius = 1;

    public List<Content> content = new List<Content>();

    public List<ArScenario> scenarios = new List<ArScenario>();

    public List<ArMarker> markers = new List<ArMarker>();
}
