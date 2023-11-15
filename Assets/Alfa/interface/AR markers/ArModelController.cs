using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArModelController : MonoBehaviour
{
    [SerializeField] private List<ScenarioController> arModelPoints = new List<ScenarioController>();

    public void ResetPoints()
    {
        foreach(var p in arModelPoints)
        {
            p.ResetPoint();
        }
    }

    public void SelectPoint(ScenarioController arModelPoint)
    {
        //делаем сброс всех сценариев в этой модели
        ResetPoints();

        //активируем выделенный сценарий
        arModelPoint.Activate();

        //воспроизводим аудио/видео трэк сценария
        //AudioPlayerController.ins.PlayTrack();

        //отображаем кнопку "подробности", передав в нее сценарий
        ShowButtonPointDetails(arModelPoint);
    }

    public void ShowButtonPointDetails(ScenarioController arModelPoint)
    {


        ButtonManager.ins.ShowArPointDetailsButton(arModelPoint);

    }

    
}
