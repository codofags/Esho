using Google.Maps;
using Google.Maps.Coord;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public enum MapState
{
    Global,
    Local,
    None,
}

public class MapManager : MonoBehaviour
{
    public static MapManager Inst;

    public GameObject markerGroupControllerPrefab;
    public Transform markersContainer;

    [SerializeField]
    private Transform mainCamera;

    [SerializeField]
    private MapsService mapsService;

    public List<GPS_Path_Config> paths = new List<GPS_Path_Config>();

    [SerializeField] private Canvas mainCanvas;

    public void Setup()
    {
        Inst = this;       

        foreach (var p in paths)
        {
            SetupPath(p);
        }

        //настраиваем контейнер в max path на главной
        ContentManager.inst.SetupMainMaxPathsContainer();

        ContentManager.inst.SetupPathMaxPathsContainer();

        //ContentManager.inst.SetupPathCount(allPaths.Count);
        ContentManager.inst.SetupPathCount(paths.Count);

        ScaleMarkers();

        StartCoroutine(RequestFineLocation());        
    }

    //Task testGPS;

    //private void Start()
    //{
    //    testGPS = new Task(() => TestBackAction());
    //    testGPS.Start();
    //}

    private void SetupPath(GPS_Path_Config path)
    {
        foreach (var p in path.points)
        {
            //добавляем в точку маршурут
            p.path = path;

            //добавляем пин на карту
            AddMapPin(p);

            //добавляем гуи отображение точки в главном разделе
            var poiContainer = ContentManager.inst.mainMiniPoisContainer;
            ContentManager.inst.AddMiniPoi(p, poiContainer, true);

            //добавляем гуи отображение точки в разделе маршрутов
            poiContainer = ContentManager.inst.pathMiniPoisContainer;
            ContentManager.inst.AddMiniPoi(p, poiContainer);
        }

        //заполняем раздел с маршрутами на главной
        var container = ContentManager.inst.mainMaxPathsContainer;
        ContentManager.inst.AddMaxPath(path, container);

        //заполняем раздел с маршрутами в разделе маршруты
        //большие карточки в пределах 3км
        container = ContentManager.inst.pathMaxPathsContainer;
        ContentManager.inst.AddMaxPath(path, container, true);

        //средние карточки всех маршрутов
        container = ContentManager.inst.pathMidPathsContainer;
        ContentManager.inst.AddMidPath(path, container);       
    }

    [SerializeField] private LatLng coord = new LatLng(55.9276201748919, 37.8500781541225);
    
    IEnumerator RequestFineLocation()
    {
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
        }

        yield return new WaitForSeconds(0);

        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            StartCoroutine(RequestFineLocation());
        }
        else
        {
            Input.location.Start();
        }
    }
    
    [SerializeField] private float GPSUpdatePeriod = 1f;
    private float NextUpdateTime = 0;

    [SerializeField] private GameObject userMarker;

    public event EventHandler/*<UserGpsArgs>*/ onUserGpsUpdate;
    //public class UserGpsArgs : EventArgs { public LatLng coord; }

    private void FixedUpdate()
    {
        UpdateUserGps();
    }

    private void UpdateUserGps()
    {


        if (Time.time < NextUpdateTime) { return; }

        NextUpdateTime = Time.time + GPSUpdatePeriod;

#if UNITY_EDITOR

#else
        coord = new LatLng(Input.location.lastData.latitude, Input.location.lastData.longitude);
#endif
        //размещаем метку юзера в соотв место на карте по жпс с девайса
        userMarker.transform.position = mapsService.Projection.FromLatLngToVector3(coord);

        onUserGpsUpdate?.Invoke(this, EventArgs.Empty);
    }

    public float GetDistance(LatLng targetCoord)
    {
        var targetPosition = mapsService.Projection.FromLatLngToVector3(targetCoord);
        var sourcePosition = mapsService.Projection.FromLatLngToVector3(coord);

        return Vector3.Distance(targetPosition, sourcePosition);
    }   

    [SerializeField]
    private List<GameObject> markers = new List<GameObject>();
    [SerializeField]
    private float markerScaleFactor = 0.1f;

    public void ScaleMarkers( )
    {
        var y = mainCamera.position.y;

        foreach (var m in markers)
        {
            m.transform.localScale = Vector3.one * y * markerScaleFactor;
        }

        foreach (var g in mapMarkerGroups)
        {
            g.transform.localScale = Vector3.one * y * markerScaleFactor;
        }
    }

    private List<MapMarkerGroup> mapMarkerGroups = new List<MapMarkerGroup>();

    public void AddMarkerGroup(MapMarkerGroup mapMarkerGroup)
    {
        mapMarkerGroups.Add(mapMarkerGroup);

        ScaleMarkers();
    }

    public void RemoveMarkerGroup(MapMarkerGroup mapMarkerGroup)
    {
        mapMarkerGroups.Remove(mapMarkerGroup);
    }

    [SerializeField]
    private GameObject markerPrefab;

    [SerializeField]
    private Transform markerContainer;  

    public void AddMapPin(GPS_Point_Config point)
    {
        var mapPosition = mapsService.Projection.FromLatLngToVector3(point.coord);

        var newMapMarker = Instantiate(markerPrefab, markerContainer);

        var mapMarkerComponent = newMapMarker.GetComponent<MapMarker>();

        mapMarkerComponent.point = point;

        if (point.sprites.Count > 0)
        {
            mapMarkerComponent.pointImage.sprite = point.sprites[0];
        }

        markers.Add(newMapMarker);

        newMapMarker.transform.position = mapPosition;
    }

    public GPS_Point_Config currentGpsPoint;

    public List<GPS_Path_Config> GetAllPath()
    {
        return paths;
    }
}