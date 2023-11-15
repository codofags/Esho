using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static MapManager;

public class MiniPoiComponent : MonoBehaviour
{
    [SerializeField] private Image poiIco;

    //private GPSPoint poi;

    //internal void Assign(GPSPoint poi)
    //internal void Assign(GPSPoint pointConfig)
    //{
    //    this.pointConfig = pointConfig;

    //    if (pointConfig.sprites.Count > 0)
    //    {
    //        poiIco.sprite = pointConfig.sprites[0];
    //    }

    //    UpdateInfoUI();

    //    //подписываем карточку на жпс
    //    MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;

    //    //подписываемся на смену языка
    //    ButtonManager.ins.OnSwitchLanguage += OnSwitchLanguage;
    //}

    //private PointConfig pointConfig;
    //internal void Assign(PointConfig poi)
    //{
    //    this.pointConfig = poi;

    //    if (poi.sprites.Count > 0)
    //    {
    //        poiIco.sprite = poi.sprites[0];
    //    }

    //    UpdateInfoUI();

    //    //подписываем карточку на жпс
    //    MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;

    //    //подписываемся на смену языка
    //    ButtonManager.ins.OnSwitchLanguage += OnSwitchLanguage;
    //}

    private bool _checkGPSZone = false;

    private GPS_Point_Config config;
    internal void Assign(GPS_Point_Config point, bool checkGPSZone)
    {
        config = point;

        if (point.sprites.Count > 0)
        {
            poiIco.sprite = point.sprites[0];
        }

        UpdateInfoUI();

        _checkGPSZone = checkGPSZone;

        //подписываем карточку на жпс
        MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;        

        //подписываемся на смену языка
        ButtonManager.ins.OnSwitchLanguage += OnSwitchLanguage;
    }

    private void OnSwitchLanguage(object sender, EventArgs e)
    {
        UpdateInfoUI();
    }

    private void UpdateInfoUI()
    {
        var language = ButtonManager.ins.GetLanguage();

        //имя пути
        SetName(config, language);       
    }

    [SerializeField] private TextMeshProUGUI pathNameText;
    //private void SetName(PointConfig poi, Language language)
    //{
    //    var translation = poi.translation;

    //    //Debug.Log(translation);

    //    var translateConfig = translation.FindTranslateConfig(language);

    //    pathNameText.text = translateConfig.text;
    //}
    private void SetName(GPS_Point_Config point, Language language)
    {
        var translation = point.pointName;

        //Debug.Log(translation);

        var translate = translation.FindTranslateConfig(language);

        if (translate == null)
        {
            pathNameText.text = "no name";
        }
        else
        {
            pathNameText.text = translate.text;
        }        
    }

    [SerializeField] private TextMeshProUGUI gpsDistance;
    private void OnUserGpsUpdate(object sender, EventArgs e)
    {
        var distance = MapManager.Inst.GetDistance(config.coord) / 1000;

        var language = ButtonManager.ins.GetLanguage();

        if (distance < config.radius) 
        //if (distance > 998.72f && distance < 998.74f)
        {
            switch (language)
            {
                case Language.Rus:
                    {
                        gpsDistance.text = "На месте";
                    }
                    break;
                case Language.Eng:
                    {
                        gpsDistance.text = "On spot";
                    }
                    break;
            }

            if (!_checkGPSZone) return;

            //открываем вход в пои    
            if (Application.isFocused)
            {
                if (MapManager.Inst.currentGpsPoint != config)
                {
                    MapManager.Inst.currentGpsPoint = config;

                    ButtonManager.ins.ShowPoiInfo(config);
                }
            }
            else
            {
                //отправить нотифи на телефон

                if (MapManager.Inst.currentGpsPoint != config)
                {
                    MapManager.Inst.currentGpsPoint = config;

                    showPoi = () => ButtonManager.ins.ShowPoiInfo(config);

                    ButtonManager.ins.CreateNotification("Nasledie", "Enter GPS point", DateTime.Now.AddSeconds(0));
                }
            }

        }
        else
        {
            //if (MapManager.Inst.CurrentGpsPoint == pointConfig)
            //{
            //    MapManager.Inst.CurrentGpsPoint = null;
            //}

            var distanceText = "";

            switch (language)
            {
                case Language.Rus:
                    {
                        distanceText = "км";
                    }
                    break;
                case Language.Eng:
                    {
                        distanceText = "km";
                    }
                    break;
            }

            gpsDistance.text = $"{ (distance).ToString("f1")} {distanceText}";
        }
        //Debug.Log($"distance => {distance}");
    }

    private Action showPoi;
    private void OnApplicationPause(bool pause)
    {
        if (!pause)
        {
            if (showPoi != null)
            {
                showPoi();
                showPoi = null;
            }
        }
    }

    public void OpenPoi()
    {
        Debug.Log(config.name);

        var playAudio = AutoGPSPlay.ins.useAutoGid;

        AudioPlayerController.ins.SetupPlayList(config, playAudio);
        Ui_GPS_Point_Menu.ins.OpenPoint(config);
    }
}
