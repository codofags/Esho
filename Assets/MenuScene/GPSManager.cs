using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Inst;

    public GameObject SelectedGPSPathGameObject;
    public GPSPath SelectedGPSPath;
    //public List<GPSPoint> GPSPointsList = new List<GPSPoint>();   

    public void Setup()
    {
        
            Inst = this;
       
    }      

  

    public void SelectPath(GPSPath GPSPath)
    {
        //if (SelectedGPSPathGameObject != null) Destroy(SelectedGPSPathGameObject);

        //SelectedGPSPathGameObject = GPSPath.gameObject;

        //SelectedGPSPathGameObject.transform.SetParent(transform);

        SelectedGPSPath = GPSPath;

      
    }
}
