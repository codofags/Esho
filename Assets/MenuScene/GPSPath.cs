using System;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class GPSPath 
{
    //иконка маршрута
    //public Sprite pathIco;

    public List<Sprite> pathSprites = new List<Sprite>();

    //название маршрута
    public TextTranslation translation;
    //public string pathName;

    public string pathDuration;
    //pub
    public string poiCount;

    public bool isAr;

    //точки интереса на маршруте
    public List<GPSPoint> GPSPoints = new List<GPSPoint>();

    public ContentPack contentPack;

    public GPSPath()
    {

    }

    //public GPSPath(PathConfig path)
    //{
    //    this.translation = path.translation;
    //    this.pathDuration = path.pathDuration;
    //    this.poiCount = path. poiCount;
    //    this.isAr = path. isAR;
    //}

    //public GPSPath(string pathName, string pathDuration, string poiCount, bool isAR)
    //{
    //    this.pathName = pathName;
    //    this.pathDuration = pathDuration;
    //    this.poiCount = poiCount;
    //    isAr = isAR;
    //}

    internal void AddPoint(GPSPoint newGPSPoint)
    {
        GPSPoints.Add(newGPSPoint);
    }

    //public void AssignPath(Dictionary<byte, object> NewPath)
    //{
    //    pathName = (string)NewPath[(byte)Params.Name];
    //    //PathDescription = (string)NewPath[(byte)Params.Description];

    //    var NewPoints = (Dictionary<byte, object>)NewPath[(byte)Params.Points];
    //    AddPoints(NewPoints);
    //}

    //public void AddPoints(Dictionary<byte, object> NewPoints)
    //{
    //    foreach (var K in NewPoints.Keys)
    //    {
    //        var NewPoint = (Dictionary<byte, object>)NewPoints[K];         

    //        var NewGPSPoint = new GPSPoint();

    //        NewGPSPoint.AssignPoint(NewPoint);           

    //        GPSPoints.Add(NewGPSPoint);
    //    }
    //}  

    internal void AddPathSprite(Sprite sprite)
    {
        pathSprites.Add(sprite);
    }

    internal void AddContent(Content content)
    {
        if (contentPack == null)
        {
            contentPack = new ContentPack();
        }

        contentPack.AddConent(content);
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
