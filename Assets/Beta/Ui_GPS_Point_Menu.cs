using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_GPS_Point_Menu : MonoBehaviour
{
    [SerializeField] private ButtonManager uiManager;
    private void OnSwitchLanguage(object sender, EventArgs e)
    {
        SetupPointCaption(currentPoint);
    }

    [SerializeField] private GameObject PointDescription;

    [SerializeField] private ContentManager contentManager;  

    public static Ui_GPS_Point_Menu ins;

    private void Start()
    {
        ins = this;

        MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;
        uiManager.OnSwitchLanguage += OnSwitchLanguage;
    }

    private void OnUserGpsUpdate(object sender, EventArgs e)
    {
        UpdatePointDistance();
    }

    //private PointConfig currentPoint;
    //public void OpenPoint(PointConfig point)
    //{
    //    uiManager. navigationHistory.AddHistoryPoint(
    //      () =>
    //      {
    //          OpenPoint(point);
    //      });

    //    uiManager. OpenPathContainer();

    //    currentPoint = point;

    //    SetupPointCaption(point);

    //    contentManager.AssignPoint(point);

    //    PointDescription.SetActive(true);
    //}

    private GPS_Point_Config currentPoint;
    public void OpenPoint(GPS_Point_Config point)
    {
        uiManager.navigationHistory.AddHistoryPoint(
          () =>
          {
              OpenPoint(point);
          });

        uiManager.OpenPathContainer();

        currentPoint = point;

        SetupPointCaption(point);

        contentManager.AssignPoint(point);

        PointDescription.SetActive(true);
    }

    [SerializeField] private TextMeshProUGUI pointNameText;
    [SerializeField] private TextMeshProUGUI pointTimeText;
    [SerializeField] private TextMeshProUGUI pointObjectsText;

    private void SetupPointCaption(GPS_Point_Config point)
    {
        if (currentPoint == null) return;
        pointNameText.text = TranslateHelper.ins.GetText(point.pointName);

        //время
        {
            //var duration = point.duration;

            //var hours = (int)duration / 60;
            //var minutes = duration - hours * 60;

            //var resultTime = "";

            //if (hours > 0)
            //{
            //    switch (uiManager.language)
            //    {
            //        case Language.Rus:                    {                        resultTime += $"{hours} ч.";                    } break;
            //        case Language.Eng: resultTime += $"{hours} h."; break;
            //    }
            //}

            //if (minutes > 0)
            //{
            //    resultTime += " ";

            //    switch (uiManager.language)
            //    {
            //        case Language.Rus: resultTime += $"{minutes} м."; break;
            //        case Language.Eng: resultTime += $"{minutes} m."; break;
            //    }
            //}
        }

        pointTimeText.text = TranslateHelper.ins.GetText(point.duration); // resultTime;

        pointObjectsText.text = TranslateHelper.ins.GetObjectsText(point.objectsCount);

        StartCoroutine(UpdateCanvas());
    }

    [SerializeField] private HorizontalLayoutGroup detailsGroup;
    [SerializeField] private HorizontalLayoutGroup distanceGroup;
    private IEnumerator UpdateCanvas()
    {
        yield return new WaitForEndOfFrame();

        detailsGroup.childAlignment = TextAnchor.UpperCenter;
        distanceGroup.childAlignment = TextAnchor.MiddleCenter;

        yield return new WaitForEndOfFrame();

        detailsGroup.childAlignment = TextAnchor.UpperLeft;
        distanceGroup.childAlignment = TextAnchor.MiddleLeft;
    }

    private IEnumerator UpdateDistanceCanvas()
    {
        yield return new WaitForEndOfFrame();

        distanceGroup.childAlignment = TextAnchor.MiddleCenter;

        yield return new WaitForEndOfFrame();

        distanceGroup.childAlignment = TextAnchor.MiddleLeft;
    }

    public void ClosePointDescription()
    {
        PointDescription.SetActive(false);
    }

    [SerializeField] private TextMeshProUGUI pointGpsDistance;
    private void UpdatePointDistance()
    {
        if (currentPoint == null) return;

        var distance = MapManager.Inst.GetDistance(currentPoint.coord) / 1000;

        if (distance < currentPoint.radius)
        {
            switch (uiManager.language)
            {
                case Language.Rus:
                    {
                        pointGpsDistance.text = "На месте";
                    }
                    break;
                case Language.Eng:
                    {
                        pointGpsDistance.text = "On spot";
                    }
                    break;
            }
        }
        else
        {
            var distanceText = "";

            switch (uiManager.language)
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

            pointGpsDistance.text = $"{ (distance).ToString("f1")} {distanceText}";

            StartCoroutine(UpdateDistanceCanvas());
        }
    }
}
