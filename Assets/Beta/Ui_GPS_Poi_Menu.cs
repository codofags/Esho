using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_GPS_Poi_Menu : MonoBehaviour
{
    public GameObject poi;
    [SerializeField] private Transform poiPointContainer;
    [SerializeField] private Transform poiAudioContainer;

    public void OpenPoi(GPS_Point_Config point)
    {
        ContentManager.inst.ResetContainer(poiPointContainer);
        ContentManager.inst.ResetContainer(poiAudioContainer);

        ContentManager.inst.AddMidPoi(point, poiPointContainer);

        ContentManager.inst.AddPoiAudio(point, poiAudioContainer);

        poi.SetActive(true);
    }

    public void ClosePoi()
    {
        poi.SetActive(false);
    }
}
