using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class ArMarker
{
    public string name;
    public int modelIndex;

    [HideInInspector]
    public PointConfig point;

    public Vector3 position;
    public Vector3 rotation;

    //internal void AssignModel(int modelIndex, Vector3 position, Quaternion rotation)
    //{
    //    this.modelIndex = modelIndex;

    //    this.position = position;
    //    this.rotation = rotation;
    //}

    //public int GetModelIndex()
    //{
    //    return modelIndex;
    //}
}
