using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ModelManager : MonoBehaviour
{
    public static ModelManager ins;

    [SerializeField]
    public List<ArModel> modelPrefabs = new List<ArModel>();

    public GameObject ScenarioPrefab;

    private Dictionary<string, ArMarker> markers = new Dictionary<string, ArMarker>();

    private void Awake()
    {
        ins = this;
    }

    /// <summary>
    /// создаем новый ар маркер
    /// </summary>
    /// <param name="key">имя маркера в библиотеке референсов </param>
    public ArMarker CreateArMarker(string key)
    {
        //создаем новый ар маркер
        var newArMarker = new ArMarker();     

        markers.Add(key, newArMarker);

        return newArMarker;
    }

    public ArMarker GetArMarker(string key)
    {
        if (markers.ContainsKey(key))
        {
            return markers[key];
        }

        return null;
    }

    internal ArModel GetModelPrefab(int modelIndex)
    {
        return modelPrefabs[modelIndex];
    }

    internal void AddMarker(ArMarker marker)
    {
        markers.Add(marker.name, marker);
    }

    //internal void AddModel(int key, int model)
    //{
    //    if (!models.ContainsKey(key))
    //    {
    //        models.Add(key, modelPrefabs[model]);
    //    }
    //    else
    //    {
    //        Debug.Log("cant add model. marker already in use");
    //    }
    //}
}
