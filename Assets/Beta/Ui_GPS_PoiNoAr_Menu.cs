using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ui_GPS_PoiNoAr_Menu : MonoBehaviour
{
    public GameObject poiNoAr;
    [SerializeField] private Transform poiNoArPointTransform;
    [SerializeField] private Transform poiNoArAudioTransform;

    public void OpenPoiNoAr(GPS_Point_Config point)
    {
        ContentManager.inst.ResetContainer(poiNoArPointTransform);
        ContentManager.inst.ResetContainer(poiNoArAudioTransform);

        ContentManager.inst.AddMidPoi(point, poiNoArPointTransform);

        ContentManager.inst.AddPoiAudio(point, poiNoArAudioTransform);

        poiNoAr.SetActive(true);
    }

    public void ClosePoiNoAr()
    {
        poiNoAr.SetActive(false);
    }
}
