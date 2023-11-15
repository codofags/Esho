using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//[Serializable]
public class MapMarkerGroup : MonoBehaviour
{

    [SerializeField]
    private Text Text_MarkerCounter;

    [SerializeField]
    private GameObject UI_GroupMarkersCounter;

    [SerializeField]
    private Image groupIco;

    private List<MapMarker> markers = new List<MapMarker>();

    /// <summary>
    /// добавить маркер в группу маркеров
    /// </summary>
    /// <param name="mapMarker"></param>
    internal void AddMarker(MapMarker mapMarker)
    {
        //добавляем маркер в список
        markers.Add(mapMarker);       

        //указываем маркеру в какой он теперь группе
        mapMarker.SetMapMarkerGroup(this);

        //скрываем маркер
        mapMarker.HideMarker();

        //обновляем и отображаем кол-во маркеров в группе
        RecalculateGroupCounter();
        //обновляем положение контроллера группы
        RecalculatePosition();
        //определяем иконку группы
        RecalculateIco();
    }

    /// <summary>
    /// добавить маркер в группу маркеров
    /// </summary>
    /// <param name="mapMarker"></param>
    internal void AddMarkers(List<MapMarker> customMapMarkers)
    {
        foreach (var m in customMapMarkers)
        {
            //добавляем маркер в список
            markers.Add(m);

            //указываем маркеру в какой он теперь группе
            m.SetMapMarkerGroup(this);
        }

        //обновляем и отображаем кол-во маркеров в группе
        RecalculateGroupCounter();
        //обновляем положение контроллера группы
        RecalculatePosition();
        //определяем иконку группы
        RecalculateIco();
    }

  

    /// <summary>
    /// удалить маркер из группы маркеров
    /// </summary>
    /// <param name="mapMarker"></param>
    internal void RemoveMarker(MapMarker mapMarker)
    {
        //забываем маркер
        markers.Remove(mapMarker);

        //освобождаем маркер от группы
            mapMarker.RemoveMapMarkerGroup();

        //показываем маркер
        mapMarker.ShowIco();

        //если маркеров в группе не осталось - удаляем группу
        if (markers.Count == 0)
        {
            MapManager.Inst.RemoveMarkerGroup(this);
            Destroy(gameObject);
            return;
        }

        //обновляем и отображаем кол-во маркеров в группе
        RecalculateGroupCounter();
        //обновляем положение контроллера группы
        RecalculatePosition();
        //определяем иконку группы
        RecalculateIco();
    }

    internal void RemoveMarkers(List<MapMarker> customMapMarkers)
    {
        foreach (var m in customMapMarkers)
        {
            //удаляем маркеры и списка
            markers.Remove(m);

            //очищаем группу маркеру
            m.RemoveMapMarkerGroup(); 
        }

        //если маркеров в группе не осталось - удаляем группу
        if (markers.Count == 0)
        {
            MapManager.Inst.RemoveMarkerGroup(this);
            Destroy(gameObject);
            return;
        }

        //обновляем и отображаем кол-во маркеров в группе
        RecalculateGroupCounter();
        //обновляем положение контроллера группы
        RecalculatePosition();
        //определяем иконку группы
        RecalculateIco();
    }

    public void RecalculatePosition()
    {
        if (markers.Count > 1)
        {
            var summaryPosition = Vector3.zero;

            foreach (var m in markers)
            {
                summaryPosition += m.transform.position;
            }

            var resultposition = summaryPosition / markers.Count;

            transform.position = resultposition;

            //Debug.Log($"summaryPosition {summaryPosition} => resultposition {resultposition}");
        }
    }

    public void RecalculateIco()
    {
        var primaryFactor = 0;

        Sprite primaryIco = null;

        foreach (var m in markers)
        {
            if (m.markerIcoPrimaryFactor > primaryFactor)
            {
                primaryIco = m.markerIcoSprite;
            }
        }

        groupIco.sprite = primaryIco;
    }

    public void RecalculateGroupCounter()
    {
        Text_MarkerCounter.text = $"{markers.Count}";
    }

    internal List<MapMarker> GetMapMarkers()
    {
        var result = new List<MapMarker>();

        foreach(var m in markers)
        {
            result.Add(m);
        }

        return result;
    }
}
