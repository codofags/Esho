using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GUIManager : MonoBehaviour
{
    public static GUIManager Inst;

    public GameObject Canvas;

    //[Header("GPS PATH")]
    //public GameObject GUIPathControllerPrefab;
    //public Transform GUIPathControllersContainer;

    [Header("GPS CARD")]
    public GameObject GPSCardGameObject;
    public GPSCard _GPSCard;

    [Header("SPRITES")]
    public Sprite PlaySprite;
    public Sprite PauseSprite;

    [Header("Error Message")]
    public GameObject Wi_Error;
    public Text ErrorCaption;
    public Text ErrorMessage;

    private void Awake()
    {
        Setup();
    }

    public void Setup()
    {
        Inst = this;

        //DontDestroyOnLoad(Canvas);
    }

    //public void SetupGUIPathControllers(Dictionary<byte, object> NewPaths)
    //{
    //    Helper.Inst.ClearContainer(GUIPathControllersContainer);

    //    foreach(var K in NewPaths.Keys)
    //    {
    //        var NewPath = (Dictionary<byte, object>)NewPaths[K];

    //        var NewGUIPathControllerGameObject = Instantiate(GUIPathControllerPrefab);
    //        NewGUIPathControllerGameObject.transform.SetParent(GUIPathControllersContainer);

    //        var NewGUIPathControllerComponent = NewGUIPathControllerGameObject.GetComponent<GPSPath>();

    //        var NewPoints = (Dictionary<byte, object>)NewPath[(byte)Params.Points];
    //        NewGUIPathControllerComponent.AssignPath(NewPath);            
    //    }
    //}

    public void ShowGPSPointCard(GPSPoint _GPSPoint)
    {
        _GPSCard.AssignGPSPoint(_GPSPoint);
        GPSCardGameObject.SetActive(true);
        _GPSCard.PlayAudio();
    }

    public void HideGPSPointCard()
    {
       
        GPSCardGameObject.SetActive(false);
       
    }

    public void ShowError(string Caption, string Message)
    {
        ErrorCaption.text = Caption;
        ErrorMessage.text = Message;

        Wi_Error.SetActive(true);
    }

    public void HideError()
    {
        Wi_Error.SetActive(false);
    }


    public Text PathNameText;
    public void ShowPathName(string pathName)
    {
        PathNameText.text = pathName;
    }
}
