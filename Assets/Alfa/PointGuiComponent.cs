using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PointGuiComponent : MonoBehaviour
{
    [SerializeField] private Image pointIco;
    [SerializeField] private TextMeshProUGUI pointName;
    [SerializeField] private TextMeshProUGUI pointDistance;
    [SerializeField] private GameObject arMarker;

    //private GPSPoint point;

    //internal void Assign(GPSPoint point)
    //{
    //    this.point = point;

    //    if (point.pointSprites.Count > 0)
    //    {
    //        pointIco.sprite = point.pointSprites[0];
    //    }

    //    //pointName.text = point.PointName;

    //    //подписываем карточку на жпс
    //    MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;

    //    if (point.isAr)
    //    {
    //        arMarker.SetActive(point.isAr);
    //    }

    //}

    //private PointConfig point;
    private GPS_Point_Config point;
    //internal void Assign(PointConfig point)
    //{
    //    this.point = point;

    //    if (point.sprites.Count > 0)
    //    {
    //        pointIco.sprite = point.sprites[0];
    //    }

    //    pointName.text = TranslateHelper.ins.GetText(point.translation);

    //    //подписываем карточку на жпс
    //    MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;

    //    arMarker.SetActive(point.isAR);
    //}
    internal void Assign(GPS_Point_Config point)
    {
        this.point = point;

        if (point.sprites.Count > 0)
        {
            pointIco.sprite = point.sprites[0];
        }

        pointName.text =  TranslateHelper.ins.GetText(point.pointName);

        //подписываем карточку на жпс
        MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;

        arMarker.SetActive(point.isAr);
    }

    private void OnUserGpsUpdate(object sender, EventArgs e)
    {
        var distance = MapManager.Inst.GetDistance(point.coord) / 1000;

        if (distance < point.radius)
        {
            pointDistance.text = "На месте";
        }
        else
        {
            pointDistance.text = $"{ (distance).ToString("f1")} км";
        }
    }

    public void OpenPoint()
    {
        var playAudio = AutoGPSPlay.ins.useAutoGid;
        AudioPlayerController.ins.SetupPlayList(point, playAudio);
        Ui_GPS_Point_Menu.ins.OpenPoint(point);
    }
}
