using Google.Maps.Coord;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GPS Point Config", menuName = "BK/GPS Point/Point")]
public class GPS_Point_Config : ScriptableObject
{
    public TextTranslation pointName;

    public TextTranslation duration;

    public int objectsCount;

    public LatLng coord;
    public Vector3 position;
    public float radius = 3f;

    public GPSPointController _GPSPointController;
    public GPS_Path_Config path;
    public List<string> searchKeys = new List<string>();
    public List<Content_Config> content = new List<Content_Config>();
    public List<Sprite> sprites = new List<Sprite>();

    public bool isAr;
}
