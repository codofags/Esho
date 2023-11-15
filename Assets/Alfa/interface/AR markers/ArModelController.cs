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
        //������ ����� ���� ��������� � ���� ������
        ResetPoints();

        //���������� ���������� ��������
        arModelPoint.Activate();

        //������������� �����/����� ���� ��������
        //AudioPlayerController.ins.PlayTrack();

        //���������� ������ "�����������", ������� � ��� ��������
        ShowButtonPointDetails(arModelPoint);
    }

    public void ShowButtonPointDetails(ScenarioController arModelPoint)
    {


        ButtonManager.ins.ShowArPointDetailsButton(arModelPoint);

    }

    
}
