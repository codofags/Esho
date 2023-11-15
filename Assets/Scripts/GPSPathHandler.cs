using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPS_PathHandler 
{
    public List<GPS_PointHandler> GPS_PointHandlers;
}

[Serializable]
public class GPS_PointHandler
{
    public List<Content> contents;
}

[Serializable]
public class AR_MarkerHandler
{

}

[Serializable]
public class AR_ModelHandler
{
    
}