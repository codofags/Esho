using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPSPointController : MonoBehaviour
{
    public GPSPoint _GPSPoint;

    public GameObject PointLock;

    private void OnMouseDown()
    {
        GUIManager.Inst.ShowGPSPointCard(_GPSPoint);
    }

    public void LockPoint()
    {
        PointLock.SetActive(true);
    }

    public void UnlockPoint()
    {
        PointLock.SetActive(false);
    }
}
