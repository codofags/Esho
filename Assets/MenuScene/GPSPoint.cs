using Google.Maps.Coord;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class GPSPoint
{
    public TextTranslation translation;

    public string PointDescription;
    public string pointDuration;
    public string pointObjectCount;

    public LatLng PointCoord;
    public Vector3 PointPosition;
    public float Radius=3f;   

    public  GPSPointController _GPSPointController;
    public GPSPath path;
    public ContentPack contentPack;
    public ScenarioList scenarioList;
    public List<Sprite> pointSprites = new List<Sprite>();

    public bool isAr;

    public GPSPoint()
    {

    }

    public GPSPoint(PointConfig point)
    {
        this.translation = point.translation;
        this.isAr = point.isAR;
    }

    //public void AssignPoint(Dictionary<byte, object> NewPoint)
    //{
    //    var Latitude = (double)NewPoint[(byte)Params.Latitude];
    //    var Longitude = (double)NewPoint[(byte)Params.Longitude];

    //    PointCoord = new LatLng(Latitude, Longitude);

    //    PointName = (string)NewPoint[(byte)Params.Name];
    //    PointDescription = (string)NewPoint[(byte)Params.Description];
    //    Radius = (float)NewPoint[(byte)Params.Radius];
    //    //AudioClipId =(int)NewPoint[(byte)Params.AudioClipId];
    //    //ModelId= (int)NewPoint[(byte)Params.ModelId];


    //    PointPosition =( Helper.Inst.GPSToMeters(Longitude, Latitude) - GameManager.Inst.MapCenter) * GameManager.Inst.MapScale;

    //    PointPosition.y = 0;

    //    //Debug.Log($"PointPosition {PointPosition}");
    //}

    //internal void SetPath(GPSPath path)
    //{
    //    this.path = path;
    //}

    //internal void AddContent(GPSPoint newGPSPoint, Content content)
    //{
    //    if (contentPack == null)
    //    {
    //        contentPack = new ContentPack();
    //    }

    //    contentPack.AddConent(content);
    //}

    internal void AddScenarioList(ScenarioList scenarioList)
    {
        this.scenarioList = scenarioList;
    }

    public void AddPointSprite(Sprite s)
    {
        pointSprites.Add(s);
    }

    private List<string> searchKeys = new List<string>();
    internal void AddSearhKeys(List<string> searchKeys)
    {
        this.searchKeys = searchKeys;
    }

    public List<string> GetSearchKeys()
    {
        return searchKeys;
    }
}
