using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARManager : MonoBehaviour
{
    public static ARManager Ins;
    public ARRaycastManager _ARRaycastManager;
    public ARTrackedImageManager _ARTrackedImageManager;
    public Camera ARCamera;
    public GameObject Cross;
    public ARState _ARState { get; private set; }
    Pose ARPose;

  

    private void Start()
    {
        Setup();
    }

    private Vector3 _screenCenter;

    private Transform _modelContainer;
    private Transform _markerContainer;
    public void Setup()
    {
        Ins = this;
        _screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);

        _modelContainer = new GameObject().transform;
        _markerContainer = new GameObject().transform;

        _ARState = ARState.Plane;

        ButtonManager.ins.UpdateScanHint();
    }

    #region Markers
    public ARTrackedImageManager _ImageManager;

    public ArModel arModel;

    public Dictionary<string, GameObject> MarkerObjects = new Dictionary<string, GameObject>();

    private void OnEnable()
    {
        _ImageManager.trackedImagesChanged += ImagesChanged;
    }

    private void OnDisable()
    {
        _ImageManager.trackedImagesChanged -= ImagesChanged;
    }

    private bool firstLunch=true;

    public void ImagesChanged(ARTrackedImagesChangedEventArgs _Args)
    {
        if (firstLunch)
        {
            firstLunch = false;
            ButtonManager.ins.CloseMarkerScaner();
            ButtonManager.ins.OpenArModel();
        }       

        foreach (var A in _Args.added)
        {
            UpdateModel(A);
        }

        foreach (var A in _Args.updated)
        {
            if (A.trackingState == TrackingState.Tracking)
            {
                UpdateModel(A);
            }
            else
            {
                //HideModel(A);
            }
        }

        foreach (var A in _Args.removed)
        {
            //HideModel(A);
        }
    }

    private void ResetMarkerObjects()
    {
        foreach (var O in MarkerObjects.Values)
        {
            O.SetActive(false);
        }
    }

    private void HideModel(ARTrackedImage A)
    {
        MarkerObjects[A.referenceImage.name].SetActive(false);
    }

    private void Update()
    {
        if (_ARState == ARState.Plane)
        {
            DetectARPlane();
        }

#if UNITY_EDITOR

        EditorZoom();

        if (Input.GetKeyDown(KeyCode.Alpha1)) 
        {
            CreatePreviewModel();
        }

#else

            AndroidZoom();

#endif   
    }

    //private void PrintModel(string name)
    //{
    //    if (firstLunch)
    //    {
    //        firstLunch = false;
    //        ButtonManager.ins.CloseMarkerScaner();
    //        ButtonManager.ins.OpenArModel();
    //    }

    //    string imageName = name;

    //    if (!MarkerObjects.ContainsKey(imageName))
    //    {
    //        CreateModelAndScenarios(imageName);
    //    }

    //    MarkerObjects[imageName].SetActive(true);
    //}

    ///если прочитался маркер, воспроизводим видео
    private bool videoPlayed = false;    
    private void UpdateModel(ARTrackedImage A)
    {
        if (!videoPlayed)
        {
            ArVideoPlayer.ins.Play();
        }

        //var arMarker = ModelManager.ins.GetArMarker(A.referenceImage.name);

        //if (!MarkerObjects.ContainsKey(A.referenceImage.name))
        //{
        //    CreateModelAndScenarios(A.referenceImage.name);
        //}

        //MarkerObjects[A.referenceImage.name].transform.position = A.transform.position;//+ arMarker.position;
        //MarkerObjects[A.referenceImage.name].transform.rotation = A.transform.rotation;// Quaternion.Euler(A.transform.rotation.eulerAngles + arMarker.rotation.eulerAngles);
        //MarkerObjects[A.referenceImage.name].SetActive(true);      
    }

    private void CreateModelAndScenarios(string markerName)
    {
        Clear();

        //создаем и запиоминаем модель
        var arMarker = ModelManager.ins.GetArMarker(markerName);

        if (arMarker == null)
        {
            Debug.Log("cant create ar model and scenarios. marker not found");
            return;
        }

        //setup audio/video play list
        //AudioPlayerController.ins.SetupPlayList(arMarker.point);

        //
        var prefab = ModelManager.ins.GetModelPrefab(arMarker.modelIndex);

        var NewModel = Instantiate(prefab.gameObject);

        NewModel.name = markerName;

        var arModel = NewModel.GetComponent<ArModel>();

        arModel.SetPosition(arMarker.position);
        arModel.SetRotation(arMarker.rotation);

        MarkerObjects.Add(markerName, NewModel);

        NewModel.SetActive(false);

        //создаем сценарии и засовываем их в созданную модель
        //foreach(var s in arMarker.point.scenarios)
        //{
        //    //создаем новый контроллер сценария на сцене
        //    var newScenario = Instantiate(ModelManager.ins.ScenarioPrefab);

        //    //получаем компонент контроллера сценария
        //    var scenarioController = newScenario.GetComponent<ScenarioController>();

        //    scenarioController.SetArScenario(s);

        //    if (!scenarioController.GetArScenario().visible)
        //    {
        //        newScenario.SetActive(false);
        //    }

        //    //настраиваем контроллер сценария
        //    scenarioController.AssigScenario(s);

        //    //засовываем контроллер в модель
        //    //newScenario.transform.SetParent(NewModel.transform);
        //    arModel.AddPin(newScenario, s.scenarioPosition);
        //    //размещаем локально 
        //    //newScenario.transform.localPosition = s.scenarioPosition;

        //    //запоминаем сценарий в модели
        //    arModel.arModelPoints.Add(scenarioController);
        //    //запоминаем модель в сценарии
        //    scenarioController.arModelController = arModel;
        //}
    }

    #endregion

    //[SerializeField] private GameObject marker;
    [SerializeField] private GameObject _bk31Model;
    private GameObject _currentModel;
    private void DetectARPlane()
    {
        if (_currentModel != null) return;

        var NewRay = ARCamera.ScreenPointToRay(_screenCenter);

        //сюда будем писать результаты рейкастов АР
        var Hits = new List<ARRaycastHit>();

        //кидаем рейкаст
        _ARRaycastManager.Raycast(NewRay, Hits, TrackableType.Planes);

        //определяем попали ли в трекаемую плоскость
        var PlaneDetected = Hits.Count > 0;

        //если во что-то попали
        if (PlaneDetected)
        {
            //запоминаем точку, в которой нашли плоскость
            ARPose = Hits[0].pose;

            //если нет модели для просмотра, создаем
            if (_previewModel == null)
            {
                //создаем прозрачную модель
                CreatePreviewModel();

                MoveAndRotatePreviewModel();

                ButtonManager.ins.CloseMarkerScaner();
                ButtonManager.ins.OpenArModel();
            }
            //если модель есть, двигаем и поворачиваем её
            else
            {
                MoveAndRotatePreviewModel();
            }            
        }
        else
        {
           
        }
    }

    private void MoveAndRotatePreviewModel()
    {
        if (_previewModel == null) return;

        //положение
        _previewModel.transform.position = ARPose.position;

        //вращение
        var cameraForward = ARCamera.transform.forward;
        var cameraBearing = new Vector3(cameraForward.x, 0, cameraForward.z).normalized;
        _previewModel.transform.rotation = Quaternion.LookRotation(cameraBearing);
    }

    private GameObject _previewModel;
    [SerializeField] private Material transparentMaterial;
    private GameObject CreatePreviewModel()
    {
        //создаем модель
        _previewModel = Instantiate(_bk31Model);

        //меняем материалы
        var arModel = _previewModel.GetComponent<ArModel>();
        var renders = arModel.modelState[1].GetComponents<Renderer>();

        arModel.DisablePin();

        foreach (var render in renders)
        {
            render.material = transparentMaterial;
        }

        return _previewModel;
    }

    public void CreateMainModel()
    {
        if (_previewModel != null) Destroy(_previewModel);

        if (_currentModel != null) Destroy(_currentModel);

        //если нет модели
        if (_currentModel == null)
        {
            //создаем 3д модель и ставим её на плоскость
            _currentModel = Instantiate(_bk31Model, _modelContainer);
            _currentModel.transform.position = _previewModel.transform.position;
            _currentModel.transform.rotation = _previewModel.transform.rotation;

            var arModelComponent = _previewModel.GetComponent<ArModel>();

            _currentModel.transform.localScale = Vector3.one * arModelComponent.GetModelScale();
        }
    }

    public void PlaceMainModel()
    {

    }


    public bool isModelMode = false;
   
    public void SwitchArMode()
    {
        isModelMode = !isModelMode;

        if (isModelMode)
        {
            _modelContainer.gameObject.SetActive(true);
            _markerContainer.gameObject.SetActive(false);

            _ARState = ARState.Plane;

            //_switchButtonImage.sprite = _qrCodeSprite;
        }
        else
        {
            _modelContainer.gameObject.SetActive(false);
            _markerContainer.gameObject.SetActive(true);

            _ARState = ARState.Marker;

            ClearModel();

            //_switchButtonImage.sprite = _modelSprite;
        }       
    }

    public void Clear()
    {
        foreach(var mo in MarkerObjects)
        {
            Destroy(mo.Value);
        }

        MarkerObjects.Clear();

        ClearModel();

        //Destroy(model);
    }

    private void ClearModel()
    {
        if (_currentModel != null)
        {
            Destroy(_currentModel);
            _currentModel = null;
        }
      
        if(_previewModel != null)
        {
            Destroy(_previewModel);
            _previewModel = null;
        }
    }

    internal bool HaveCurrentModel()
    {
        return _currentModel != null;
    }

    private void EditorZoom()
    {
        if (_previewModel == null) return;

        var scrollDelta = Input.mouseScrollDelta;

        if (scrollDelta != Vector2.zero)
        {
            var arModel = _previewModel.GetComponent<ArModel>();

            arModel.ScaleModel(scrollDelta.y);
        }
    }

    private void AndroidZoom()
    {
        if (_previewModel == null) return;

        // If there are two touches on the device...
        if (Input.touchCount >= 2)
        {
            // Store both touches.
            var touchZero = Input.GetTouch(0);
            var touchOne = Input.GetTouch(1);

            // Find the position in the previous frame of each touch.
            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            // Find the magnitude of the vector (the distance) between the touches in each frame.
            float prevTouchDeltaMag = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float touchDeltaMag = (touchZero.position - touchOne.position).magnitude;

            // Find the difference in the distances between each frame.
            float deltaMagnitudeDiff = prevTouchDeltaMag - touchDeltaMag;

            if (deltaMagnitudeDiff != 0)
            {
                var arModel = _previewModel.GetComponent<ArModel>();

                arModel.ScaleModel(-deltaMagnitudeDiff);
            }           
        }
    }
}

