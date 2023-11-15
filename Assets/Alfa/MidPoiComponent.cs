using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MidPoiComponent : MonoBehaviour
{
    [SerializeField] private Image pointIco;

    [SerializeField] private GameObject arPoint;

    [SerializeField] private TextMeshProUGUI pointName;
    [SerializeField] private TextMeshProUGUI pointDuration;
    [SerializeField] private TextMeshProUGUI pointObjectCount;

    private GPS_Point_Config point;

    [SerializeField] private Button buttonSelect;

    internal void Assign(GPS_Point_Config point)
    {
        this.point = point;

        if (point.sprites.Count > 0)
        {
            pointIco.sprite = point.sprites[0];
        }

        //имя пути
        pointName.text = TranslateHelper.ins.GetText(point.pointName);
        //длительность пути
        pointDuration.text = TranslateHelper.ins.GetText(point.duration);
        //кол-во пои на пути
        pointObjectCount.text = TranslateHelper.ins.GetObjectsText(point.objectsCount);
        //является ли путь ар экскурсией
        arPoint.SetActive(point.isAr);       
    }   

    public void OpenPoint()
    {
        AudioPlayerController.ins.SetupPlayList(point);
        Ui_GPS_Point_Menu.ins.OpenPoint(point);

        ButtonManager.ins.poiMenu.ClosePoi();
        ButtonManager.ins.poiNoArMenu.ClosePoiNoAr();
    }
}
