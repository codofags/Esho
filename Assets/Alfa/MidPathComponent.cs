using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MidPathComponent : MonoBehaviour
{
    [SerializeField] private Image pathIco;   

    //private PathConfig path;
    private GPS_Path_Config path;

    [SerializeField] private Button buttonSelect;

    internal void Assign(GPS_Path_Config path)
    {
        this.path = path;

        if (path.sprites.Count > 0)
        {
            pathIco.sprite = path.sprites[0];
        }

        UpdateInfoUI();

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
        SetName(path, language);

        //длительность пути
        SetDuration(path);

        //кол-во пои на пути
        SetPoiCount(path);

        //является ли путь ар экскурсией
        SetPathAR(path, language);
    }

    [SerializeField] private TextMeshProUGUI pathNameText;
    //private void SetName(PathConfig path, Language language)
    //{
    //    var translation = path.translation;

    //    var translateConfig = translation.FindTranslateConfig(language);

    //    pathNameText.text = translateConfig.text;
    //}
    private void SetName(GPS_Path_Config path, Language language)
    {
        var translation = path.pathName;// path.translation;

        var translateConfig = translation.FindTranslateConfig(language);

        if (translateConfig == null)
        {
            pathNameText.text = "no name";
        }
        else
        {
            pathNameText.text = translateConfig.text;
        }
    }

    [SerializeField] private TextMeshProUGUI pathDurationText;
    //private void SetDuration(PathConfig path)
    //{
    //    pathDurationText.text = TranslateHelper.ins.GetTimeText(path.pathTime);
    //}
    private void SetDuration(GPS_Path_Config path)
    {
        pathDurationText.text = TranslateHelper.ins.GetText(path.duration);
    }

    [SerializeField] private TextMeshProUGUI pathPoiCountText;
    //private void SetPoiCount(PathConfig path)
    //{
    //    pathPoiCountText.text = TranslateHelper.ins.GetTimeText(path.pathObjectsCount);
    //}
    private void SetPoiCount(GPS_Path_Config path)
    {
        pathPoiCountText.text = path.GetObjectCountText();
    }

    [SerializeField] private GameObject pathArObject;
    [SerializeField] private TextMeshProUGUI pathArText;
    //private void SetPathAR(PathConfig path, Language language)
    //{
    //    pathArObject.SetActive(path.isAR);

    //    switch (language)
    //    {
    //        case Language.Rus:
    //            {
    //                pathArText.text = $"AR экскурсия";
    //            }
    //            break;
    //        case Language.Eng:
    //            {
    //                pathArText.text = $"AR tour";
    //            }
    //            break;
    //    }
    //}

    private void SetPathAR(GPS_Path_Config path, Language language)
    {
        pathArObject.SetActive(path.isAR);

        switch (language)
        {
            case Language.Rus:
                {
                    pathArText.text = $"AR экскурсия";
                }
                break;
            case Language.Eng:
                {
                    pathArText.text = $"AR tour";
                }
                break;
        }
    }

    [SerializeField] private TextMeshProUGUI gpsDistance;
    private void OnUserGpsUpdate(object sender, EventArgs e)
    {
        var minDistance = float.MaxValue;

        //PointConfig point = null;
        GPS_Point_Config point = null;

        foreach (var p in path.points)
        {
            var distance = MapManager.Inst.GetDistance(p.coord) / 1000;

            if (distance < minDistance)
            {
                minDistance = distance;
                point = p;
            }
        }

        var language = ButtonManager.ins.GetLanguage();

        if (minDistance < point.radius)
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
        }
        else
        {
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

            gpsDistance.text = $"{ (minDistance ).ToString("f1")} {distanceText}";
        }

        //Debug.Log($"distance => {distance}");
    }

    public void OpenPath()
    {
        Ui_GPS_Path_Menu.ins.OpenPath(path);
    }
}
