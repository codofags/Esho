using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundTimeLine : MonoBehaviour
{
    public AudioSource _AudioSource;

    public float[] Times = new float[5] { 2, 4, 6, 8, 10 };

    public List<GameObject> Objects = new List<GameObject>();

    GameObject CurrentObject;

    public Material Green;
    public Material Default;

    private void Update()
    {
        CheckTimeLine();
    }

    private void CheckTimeLine()
    {
        for (int i = 0; i < Times.Length; i++)
        {
            var NextTime = i + 1;

            if (NextTime > Times.Length-1 ) 
            {
                NextTime = Times.Length-1 ;
            }

            if (_AudioSource.time > Times[i] && _AudioSource.time < Times[NextTime])  
            {
                if (CurrentObject == null || CurrentObject != Objects[i]) 
                {
                    ActivateObject(Objects[i]);
                }
            }

            if(_AudioSource.time > Times[NextTime])
            {
                if (CurrentObject == null || CurrentObject != Objects[i])
                {
                    ActivateObject(Objects[i]);
                }
            }
        } 
    }

    private void ActivateObject(GameObject _Object)
    {
        //Debug.Log($"activate object {_Object.name}");

        if (CurrentObject != null)
        {
            CurrentObject.GetComponent<Renderer>().material = Default;
        }

        CurrentObject = _Object;

        CurrentObject.GetComponent<Renderer>().material = Green;
    }

}
