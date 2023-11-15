using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Helper : MonoBehaviour
{
    public static Helper Inst;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

    }

    public void ClearContainer(Transform Container)
    {
        var ChildCount = Container.childCount;

        for (int i = ChildCount - 1; i >= 0; i--) 
        {
            Destroy(Container.GetChild(i).gameObject);
        }
    }

    private static readonly double R_MAJOR = 6378137.0;
    private static readonly double R_MINOR = 6356752.3142;
    private static readonly double RATIO = R_MINOR / R_MAJOR;
    private static readonly double ECCENT = Math.Sqrt(1.0 - (RATIO * RATIO));
    private static readonly double COM = 0.5 * ECCENT;

    private static readonly double DEG2RAD = Math.PI / 180.0;
    private static readonly double RAD2Deg = 180.0 / Math.PI;
    private static readonly double PI_2 = Math.PI / 2.0;

    public Vector3 GPSToMeters(double Longtitude, double Latitude)
    {
        Vector3 Result = Vector3.zero;

        Result.x = (float)lonToX(Longtitude);
        Result.y = 0;
        Result.z = (float)latToY(Latitude);
        return Result;
    }

    public static double lonToX(double lon)
    {
        return R_MAJOR * DegToRad(lon);
    }

    public static double latToY(double lat)
    {
        lat = Math.Min(89.5, Math.Max(lat, -89.5));
        double phi = DegToRad(lat);
        double sinphi = Math.Sin(phi);
        double con = ECCENT * sinphi;
        con = Math.Pow(((1.0 - con) / (1.0 + con)), COM);
        double ts = Math.Tan(0.5 * ((Math.PI * 0.5) - phi)) / con;
        return 0 - R_MAJOR * Math.Log(ts);
    }

    public static double DegToRad(double deg)
    {
        return deg * DEG2RAD;
    }
}
