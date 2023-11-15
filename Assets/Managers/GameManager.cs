using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Cache;
using System.Net.Sockets;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Inst;

    //public Helper _Helper;

    //public GUIManager _GUIManager;

    public GPSManager _GPSManager;

    //public MapManager _MapManager;

    //public ARManager _ARManager;

    //public ScenarioManager _ScenarioManager;

    //public AudioManager _AudioManager;

    //public SceneLoader _SceneLoader;

    public GameState _GameState;

    public List<GameObject> MenuObjectsList = new List<GameObject>();
    public List<GameObject> MapObjectsList = new List<GameObject>();
    public List<GameObject> AR3DObjectsList = new List<GameObject>();

    public Vector3 MapCenter;
    public float MapScale = 0.001f;
    public float MapSpriteScale = 0.02f;
    public float PointRadius = 0.05f;

    public DateTime EndTime;

    public bool NeverSleep;

    public bool UseFrameRate;
    public int TargetFrameRate = 60;

    //private void Awake()
    //{

    //    //var Run = GetTime();

    //    //if (!Run)
    //    //{
    //        //Debug.Log($"can start = {Run}");
    //        //return;
    //    //}

           

    //    //Debug.Log($"GetNistTime {TestTime}");
    //}

    private void Start()
    {       
        Permission.RequestUserPermission(Permission.FineLocation);

        Setup();
       
        EnableGPS();

        FakeStart();
    }

    private void Setup()
    {
        if (Inst == null)
        {
            Inst = this;
            return;

            //DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        if (NeverSleep)
        {
            Screen.sleepTimeout = SleepTimeout.NeverSleep;
        }

        if (UseFrameRate)
        {
            Application.targetFrameRate = TargetFrameRate;
        }

       //_Helper.Setup();

        //_SceneLoader.Setup();

        //_GUIManager.Setup();

        _GPSManager.Setup();

        //_MapManager.Setup();

        //_ARManager.Setup();

        //_ScenarioManager.Setup();

        //_AudioManager.Setup();

        SetGameState(GameState.Menu);

        
    }

    private void EnableGPS()
    {
        Input.location.Start();
    }

    private void OnApplicationQuit()
    {
        Input.location.Stop();
    }

    private void FakeStart()
    {
        //получили с сервера данные по маршрутам
        var NewPaths = new Dictionary<byte, object>();

        double P1Lo = 48.71416102924657 ;
        double P1La = 44.53542134133471 ;

        double P2Lo = 48.713195667876214 ;
        double P2La = 44.53444750055736 ;

        double P3Lo = 48.71169531794457 ;
        double P3La = 44.533770325078095 ;

        double P4Lo = 48.71055402460171 ;
        double P4La = 44.53196669408133 ;

        var C1Lo = 48.70872033597564;
        var C1La = 44.54185414928878;

        var C2Lo = 48.7156791108271;
        var C2La = 44.52660815244289;        

        var Corner1 = Helper.Inst.GPSToMeters(C1La, C1Lo);
        var Corner2 = Helper.Inst.GPSToMeters(C2La, C2Lo);

        //Debug.Log($"Corner1 {Corner1} Corner2 {Corner2}");

        MapCenter = FindMapCenter(Corner1, Corner2);

        //Debug.Log($"Map center {MapCenter}");

        //добавляем маршрут 1
        {
            var NewPath = new Dictionary<byte, object>();
            NewPath.Add((byte)Params.Name, "Маршрут 1");
            NewPath.Add((byte)Params.Description, "Маршрут 1");

            {
                var NewPathPoints = new Dictionary<byte, object>();
                NewPathPoints.Add((byte)1, AddPoint("точка 1", "описание точки 1", P1La, P1Lo, PointRadius,1));
                NewPathPoints.Add((byte)2, AddPoint("точка 2", "описание точки 2", P2La, P2Lo, PointRadius,2));
                NewPathPoints.Add((byte)3, AddPoint("точка 3", "описание точки 3", P3La, P3Lo, PointRadius,3));

                NewPath.Add((byte)Params.Points, NewPathPoints);
            }

            NewPaths.Add((byte)1, NewPath);
        }

        //добавляем маршрут 2
        {
            var NewPath = new Dictionary<byte, object>();
            NewPath.Add((byte)Params.Name, "Маршрут 2");
            NewPath.Add((byte)Params.Description, "Маршрут 2");

            {
                var NewPathPoints = new Dictionary<byte, object>();
                NewPathPoints.Add((byte)1, AddPoint("точка 2", "описание точки 2", P2La, P2Lo, PointRadius,2));
                NewPathPoints.Add((byte)2, AddPoint("точка 3", "описание точки 3", P3La, P3Lo, PointRadius,3));
                NewPathPoints.Add((byte)3, AddPoint("точка 4", "описание точки 4", P4La, P4Lo, PointRadius, 4, 1));

                NewPath.Add((byte)Params.Points, NewPathPoints);
            }

            NewPaths.Add((byte)2, NewPath);
        }

        //добавляем маршрут 3
        {
            var NewPath = new Dictionary<byte, object>();
            NewPath.Add((byte)Params.Name, "Маршрут 3");
            NewPath.Add((byte)Params.Description, "Маршрут 3");

            {
                var NewPathPoints = new Dictionary<byte, object>();
                NewPathPoints.Add((byte)1, AddPoint("точка 1", "описание точки 1", P1La, P1Lo, PointRadius,1));
                NewPathPoints.Add((byte)2, AddPoint("точка 2", "описание точки 2", P2La, P2Lo, PointRadius,2));
                NewPathPoints.Add((byte)3, AddPoint("точка 3", "описание точки 3", P3La, P3Lo, PointRadius,3));
                NewPathPoints.Add((byte)4, AddPoint("точка 4", "описание точки 4", P4La, P4Lo, PointRadius, 4, 1));

                NewPath.Add((byte)Params.Points, NewPathPoints);
            }

            NewPaths.Add((byte)3, NewPath);
        }

        //отображаем маршруты в меню
        //MapManager.Inst.SetupGPSPaths(NewPaths);
    }

    private Vector3 FindMapCenter(Vector3 Corner1, Vector3 Corner2)
    {
        float Min_X = float.MaxValue;
        float Min_Z = float.MaxValue;
        float Max_X = float.MinValue;
        float Max_Z = float.MinValue;

        //minX
        if (Corner1.x < Min_X) Min_X = Corner1.x;
        if (Corner2.x < Min_X) Min_X = Corner2.x;

        //maxX
        if (Corner1.x > Max_X) Max_X = Corner1.x;
        if (Corner2.x > Max_X) Max_X = Corner2.x;

        //minZ
        if (Corner1.z < Min_Z) Min_Z = Corner1.z;
        if (Corner2.z < Min_Z) Min_Z = Corner2.z;

        //maxZ
        if (Corner1.z > Max_Z) Max_Z = Corner1.z;
        if (Corner2.z > Max_Z) Max_Z = Corner2.z;       

        float _X = (Max_X - Min_X) / 2f + Min_X;
        float _Y = 0;
        float _Z = (Max_Z - Min_Z) / 2f + Min_Z;

        

        var SizeX = Max_X - Min_X;
        var SizeZ = Max_Z - Min_Z;

        Debug.Log($"X {SizeX}  Z {SizeZ}");

        var NewMapSpriteScale = new Vector3(SizeX,  1f, SizeZ) * MapScale * MapSpriteScale;

        //NewMapSpriteScale.z = 1f;
        //NewMapSpriteScale.x *= 0.68f;

        //MapManager.Inst.Map.transform.localScale = NewMapSpriteScale; 

        return new Vector3(_X, _Y, _Z);
    }


    private Dictionary<byte, object> AddPoint(
        string Name, string Description,
        double Longitude, double Latitude,
        float Radius,
        int AudioClipId, int ModelId=0)
    {
        var Result = new Dictionary<byte, object>();
        Result.Add((byte)Params.Name, Name);
        Result.Add((byte)Params.Description, Description);
        Result.Add((byte)Params.Longitude, Longitude);
        Result.Add((byte)Params.Latitude, Latitude);
        Result.Add((byte)Params.Radius, Radius);
        Result.Add((byte)Params.AudioClipId, AudioClipId);
        Result.Add((byte)Params.ModelId, ModelId);

        return Result;
    }

    //public void SelectPath(GPSPath NewPath)
    //{
    //    SetGameState(GameState.Map);

    //    GPSManager.Inst.SelectPath(NewPath);

    //    //SceneManager.LoadScene("Map");

    //    MapManager.Inst.ShowPath();        
    //}

    public void ShowAR3D()
    {
        //_ARManager.SetARState(ARState.DetectPlane);
        //SetGameState(GameState.AR3D);
        //SceneLoader.Inst.Load(2/*, GameState.AR3D*/);
    }

    public void ShowMenu()
    {
        //if (ScenarioManager.Inst.ScenarioGameObject != null)
        //{
        //    Destroy(ScenarioManager.Inst.ScenarioGameObject);
        //}

        //if (ScenarioManager.Inst.GetCurrentScenario() != null)
        //{
        //    Destroy(ScenarioManager.Inst.GetCurrentScenario().gameObject);
        //}

        //SetGameState(GameState.Menu);

        //SceneLoader.Inst.Load(0/*, GameState.Menu*/);

    }

    public void SetGameState(GameState NewState)
    {
        _GameState = NewState;

        switch (_GameState)
        {
            case GameState.Menu:
                {
                    ShowObjects(MapObjectsList, false);
                    ShowObjects(AR3DObjectsList, false);

                    ShowObjects(MenuObjectsList, true);
                }
                break;

            case GameState.Map:
                {
                    ShowObjects(MenuObjectsList, false);
                    ShowObjects(AR3DObjectsList, false);

                    ShowObjects(MapObjectsList, true);
                }
                break;

            case GameState.AR3D:
                {
                    ShowObjects(MapObjectsList, false);
                    ShowObjects(MenuObjectsList, false);

                    ShowObjects(AR3DObjectsList, true);

                    //_GUIManager.GPSCardGameObject.SetActive(false);

                    //_ARManager.SetARState(ARState.DetectPlane);
                }
                break;
        }
    }  

    private void ShowObjects(List<GameObject> ObjectsList, bool Show)
    {
        foreach(var O in ObjectsList)
        {
            if (O != null)
            {
                O.SetActive(Show);
            }
        }
    }   

    private bool GetTime()
    {
        EndTime = new DateTime(2020, 12, 18, 17, 25, 00);

        var client = new TcpClient("time.nist.gov", 13);

        try
        {
            using (var streamReader = new StreamReader(client.GetStream()))
            {
                var response = streamReader.ReadToEnd();
                var utcDateTimeString = response.Substring(7, 17);
                var localDateTime = DateTime.ParseExact(utcDateTimeString, "yy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal);

                Debug.Log($"localDateTime {localDateTime}");

                if (localDateTime > EndTime)
                {
                    return false;
                    Debug.Log($"stop demo");
                }
                else
                {
                    return true;
                    Debug.Log($"run demo");
                }
            }
        }
        catch(Exception e)
        {
            Debug.Log($"connection error {e.Message}");
            return false;
        }
    }

}


