using NotificationSamples;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Android;
using UnityEngine.UI;

public enum AppState
{

}

public enum Language
{
    Null,
    Rus,
    Eng,
}

public class ButtonManager : MonoBehaviour
{
    public static ButtonManager ins;

    [Header("global settings")]
    public float dragOffsetLimit = 150;
    public float tabDragSpeed = 10;

    [SerializeField]
    private Color activeColor;
    [SerializeField]
    private Color passiveColor;

    public Color activePaginationColor;
    public Color passivePaginationColor;

    [SerializeField] private SwipeHandler swipeHandler;

    //настройки языка
   public Language language { get; private set; }

    private void Awake()
    {
        ins = this;

        InitializeNotifications();

        SetupLanguage();

        swipeHandler.Setup();

        OpenHome();
    }

    private const string languageKey = "Language";
    private const string rusKey = "Rus";
    private const string engKey = "Eng";

    private void SetupLanguage()
    {
        //при первом старте устанавливаем русский
        if (!PlayerPrefs.HasKey(languageKey))
        { 
            PlayerPrefs.SetString(languageKey, rusKey);
            language = Language.Rus;
            languageButtonText.text = $"Eng";
            PlayerPrefs.Save();
        }
        else
        {
            var savedLanguage = PlayerPrefs.GetString(languageKey);
            SetLanguage(savedLanguage);
        }
    }

    [SerializeField] private TextMeshProUGUI languageButtonText;
    public event EventHandler OnSwitchLanguage;
    public void SwitchLanguage()
    {
        var key = "";

        switch (language)
        {
            default:
            case Language.Rus:
                {
                    key = engKey;

                    SetLanguage(engKey);                    
                }
                break;

            case Language.Eng:
                {
                    key = rusKey;

                    SetLanguage(rusKey);
                }
                break;        
        }

        PlayerPrefs.SetString(languageKey, key);
        PlayerPrefs.Save();

        OnSwitchLanguage?.Invoke(this, EventArgs.Empty);
    }  

    private void SetLanguage(string key)
    {
        switch (key)
        {
            case rusKey:
                {
                    language = Language.Rus;
                    languageButtonText.text = $"Eng";
                }
                break;

            case engKey:
                {
                    language = Language.Eng;                    
                    languageButtonText.text = $"Рус";
                }
                break;
        }
    }

    public Language GetLanguage()
    {
        return language;
    }

    #region main menu

    [SerializeField]
    private GameObject SearchWindow;

    public void OpenSearch()
    {
        navigationHistory.AddHistoryPoint(
          () =>
          {
              OpenSearch();
          });

        OpenPathContainer();

        SearchWindow.SetActive(true);
    }

    public void CloseSearch()
    {
        SearchWindow.SetActive(false);        
    }

  

    [SerializeField]
    private Image homeImage;
    [SerializeField]
    private Image pathImage;
    [SerializeField]
    private Image scanImage;

   

    [SerializeField]
    private Image menuImage;

    private void ResetWindows()
    {
        homeContainer.SetActive(false);
        pathsContainer.SetActive(false);
        scanContainer.SetActive(false);
        menuContainer.SetActive(false);

        //костыль
        mapMarkerContainer.SetActive(false);

        DisableMenuButton(homeImage);
        DisableMenuButton(pathImage);
        DisableMenuButton(scanImage);
        DisableMenuButton(menuImage);

        //главная камера
        mainCamera.SetActive(true);

        if (arCoreMain != null)
        {
            //arCoreMain.SetActive(false);
            var arManager = arCoreMain.GetComponent<ARManager>();

            arManager.Clear();

            Destroy(arCoreMain);            
        }

        if(ArVideoPlayer.ins)        ArVideoPlayer.ins.Stop();

        scanHelp.SetActive(false);

        //скрываем аудиоплеер
        AudioPlayer.SetActive(false);
        PlayList.SetActive(false);
        VideoPlayer.SetActive(false);

        RotateMenuButtons(0);
    }   

    private void DisableMenuButton(Image buttonImage)
    {
        buttonImage.color = passiveColor;
    }

    private void EnableMenuButton(Image buttonImage)
    {
        buttonImage.color = activeColor;
    }

    #region Map Mesh

    [SerializeField] private GameObject mapMeshContainer;

    private void ShowMapMesh()
    {
        mapMeshContainer.SetActive(true);
    }

    private void HideMapMesh()
    {
        mapMeshContainer.SetActive(false);
    }

    #endregion

    private void ResetSwipeSubscribes()
    {
        //homeSwipe.UnSubscribeToSwipe();
        //pathMainSwipe.UnSubscribeToSwipe();
    }

    #region Navigation History

    public NavigationHistory navigationHistory = new NavigationHistory(); 

    public void DoBackNavigation()
    {
        navigationHistory.DoBackNavigation();
    }

    #endregion

    #region Home

    [Header("Home")]

    [SerializeField] private GameObject mapMarkerContainer;

    [SerializeField] private GameObject homeContainer;

    //[SerializeField] private SwipeReader homeSwipe;

    public void OpenHome()
    {
        navigationHistory.AddHistoryPoint(
            () =>
            {
                OpenHome();
            });

        ResetWindows();

        ResetSwipeSubscribes();

        //homeSwipe.SubscribeToSwipe();

        EnableMenuButton(homeImage);

        homeContainer.SetActive(true);

        mapMarkerContainer.SetActive(true);

        //отображение меша карты
        ShowMapMesh();
    }

    #endregion

    #region Paths

    [Header("Path")]

    [SerializeField] private GameObject pathsContainer;
    //[SerializeField] private SwipeReader pathMainSwipe;

    public void OpenPathContainer()
    {
        ResetWindows();       

        EnableMenuButton(pathImage);

        HidePathSubWindows();

        pathsContainer.SetActive(true);

        Canvas.ForceUpdateCanvases();

        //сокрытие меша карты
        HideMapMesh();
    }

    public void OpenPaths()
    {
        navigationHistory.AddHistoryPoint(
           () =>
           {
               OpenPaths();
           });       

        OpenPathContainer();
    }

    [SerializeField] private List<GameObject> pathsWindows = new List<GameObject>();
    /// <summary>
    /// метод, скрывающий в разделе "маршруты" все подразделы
    /// </summary>
    public void HidePathSubWindows()
    {
        foreach (var w in pathsWindows)
        {
            w.SetActive(false);
        }
    }    


    [SerializeField] private GameObject scenarioDescription;
    public void OpenScenarioDescription()
    {
        OpenPaths();

        scenarioDescription.SetActive(true);
    }

    public void CloseScenarioDescription()
    {
        scenarioDescription.SetActive(false);
    }

    #endregion

    #region Scan

    [SerializeField] private GameObject arCoreMain;
    [SerializeField] private GameObject arCorePrefab;
    [SerializeField] private GameObject mainCamera;

    [SerializeField] private GameObject scanContainer;
    [SerializeField] private GameObject scanHelp;

    private bool helpReaded = false;

    public void OpenScan()
    {
        if (scanContainer.activeSelf) return;

        navigationHistory.AddHistoryPoint(
         () =>
         {
             OpenPaths();
         });

        ResetWindows();

        EnableMenuButton(scanImage);

        if (!helpReaded)
        {
            scanHelp.SetActive(true);
            helpReaded = true;
        }
        else
        {
            scanHelp.SetActive(false);
            
        }



        MarkerScaner.SetActive(true);

        scanContainer.SetActive(true);

        //отключаем камеру карты
        mainCamera.SetActive(false);

        //включаем модуль аркора
        //arCoreMain.SetActive(true);

        if (arCoreMain != null)
        {
            var arManager = arCoreMain.GetComponent<ARManager>();
            arManager.Clear();

            Destroy(arCoreMain);           
        }

        //arCoreMain = Instantiate(arCorePrefab, Vector3.zero, Quaternion.identity);

        arCoreMain = Instantiate(arCorePrefab, Vector3.zero, Quaternion.identity);

        //StartCoroutine(TestArStart());
    }

    IEnumerator TestArStart()
    {
        arCoreMain = Instantiate(arCorePrefab, Vector3.zero, Quaternion.identity);

        yield return new WaitForSeconds(0.3f);

        Destroy(arCoreMain);

        yield return new WaitForSeconds(0.3f);

        arCoreMain = Instantiate(arCorePrefab, Vector3.zero, Quaternion.identity);
    }

    [SerializeField] private GameObject MarkerScaner;

    public void OpenMarkerScaner()
    {
        OpenScan();

        poiMenu.ClosePoi();
        poiNoArMenu.ClosePoiNoAr();

        //MarkerScaner.SetActive(true);
    }


    public void CloseMarkerScaner()
    {
        //MarkerScaner.transform.position = new Vector3(-1200, 0, 0);
        MarkerScaner.SetActive(false);
    }

    public void OpenScanHelp()
    {
        scanHelp.SetActive(true);
    }

    public void CloseScanHelp()
    {
        scanHelp.SetActive(false);  
    }

    #endregion

    #region Menu

    [SerializeField] private GameObject menuContainer;

    public void OpenMenu()
    {
        ResetWindows();

        EnableMenuButton(menuImage);

        //костыль
        scanHelp.SetActive(false);
        
        menuContainer.SetActive(true);
    }

    [SerializeField]
    private GameObject AboutProject;
    public void OpenAboutProject()
    {
        AboutProject.SetActive(true);
    }

    [SerializeField]
    private GameObject QA;
    public void OpenQA()
    {
        QA.SetActive(true);
    }

    [SerializeField]
    private GameObject SendProblem;
    public void OpenSendProblem()
    {
        SendProblem.SetActive(true);
    }

    [SerializeField]
    private GameObject Contacts;
    public void OpenContacts()
    {
        //Contacts.transform.position = new Vector3(0, Screen.height, 0);
        Contacts.SetActive(true);
    }

    public void HideMenuWindows()
    {
        //AboutProject.transform.position = new Vector3(-1200, 0, 0);
        AboutProject.SetActive(false);

        //QA.transform.position = new Vector3(-1200, 0, 0);
        QA.SetActive(false);

        //SendProblem.transform.position = new Vector3(-1200, 0, 0);
        SendProblem.SetActive(false);

        //Contacts.transform.position = new Vector3(-1200, 0, 0);
        Contacts.SetActive(false);
    }

    #endregion

    //IEnumerator OpenWindow()
    //{
    //    yield return new WaitForEndOfFrame();

    //    //PointDescription.transform.position = new Vector3(0, Screen.height, 0);
    //    PointDescription.SetActive(true);
    //}

   

    

    [SerializeField]
    public GameObject PlayerController;

    [SerializeField]
    private GameObject PlayList;

    public void OpenPlayList()
    {
        ///PlayList.transform.position = new Vector3(0, Screen.height, 0);
        PlayList.SetActive(true);

        CheckPlayerController();

        //poi.SetActive(false);
        //poiNoAr.SetActive(false);
    }

    public void ClosePlayList()
    {
        //PlayList.transform.position = new Vector3(-1200, 0, 0);
        PlayList.SetActive(false);

        CheckPlayerController();
    }

    [SerializeField]
    private GameObject AudioPlayer;

    public void OpenAudioPlayer(int trackIndex)
    {
        //AudioPlayer.transform.position = new Vector3(0, Screen.height, 0);
        AudioPlayer.SetActive(true);

        AudioPlayerController.ins.audioHorizontalDrag.SelectTrack(trackIndex);

        Debug.Log($"select {trackIndex}");

        //PlayerController.SetActive(false);

        CheckPlayerController();
    }

    public void CloseAudioPlayer()
    {
        //AudioPlayer.transform.position = new Vector3(-1200, 0, 0);
        AudioPlayer.SetActive(false);

        CheckPlayerController();
    }

    private void CheckPlayerController()
    {
        if(AudioPlayer.activeSelf || PlayList.activeSelf || VideoPlayer.activeSelf)
        {
            PlayerController.SetActive(false);
        }
        else
        {
            PlayerController.SetActive(true);
        }
    }


    [SerializeField]
    private GameObject VideoPlayer;

    public void OpenVideoPlayer()
    {
        //VideoPlayer.transform.position = new Vector3(0, Screen.height, 0);
        VideoPlayer.SetActive(true);

        CheckPlayerController();

        RotateMenuButtons(-90);
    }

    public void CloseVideoPlayer()
    {
        //VideoPlayer.transform.position = new Vector3(-1200, 0, 0);
        VideoPlayer.SetActive(false);

        CheckPlayerController();

        RotateMenuButtons(0);
    }

    [SerializeField]
    private GameObject GO_ButtonHome;
    [SerializeField]
    private GameObject GO_ButtonPath;
    [SerializeField]
    private GameObject GO_ButtonScan;
    [SerializeField]
    private GameObject GO_ButtonMenu;

    private void RotateMenuButtons(float angle)
    {
        GO_ButtonHome.transform.localRotation = Quaternion.Euler(0, 0, angle);
        GO_ButtonPath.transform.localRotation = Quaternion.Euler(0, 0, angle);
        GO_ButtonScan.transform.localRotation = Quaternion.Euler(0, 0, angle);
        GO_ButtonMenu.transform.localRotation = Quaternion.Euler(0, 0, angle);
    }

    #endregion   

    //[SerializeField] private GameObject mapCameraContainer;
    //[SerializeField] private GameObject mapUserMarker;

    //[SerializeField]
    //private Vector3 mapCameraOffset=new Vector3(0,0,-70);

    //public void MooveMapCameraToUser()
    //{
    //    var tragetPosition = mapUserMarker.transform.position;

    //    mapCameraContainer.transform.position = tragetPosition + mapCameraOffset;
    //}

    //
    [SerializeField]
    private GameObject wi_ArModel;

    public void OpenArModel()
    {
        wi_ArModel.SetActive(true);
    }

    public void CloseArModel()
    {
        wi_ArModel.SetActive(false);
    }

    [SerializeField] private GameObject arPointDetailsButton;
    [SerializeField] private GameObject arButtonBack;
    private ScenarioController currentArModelPoint;

    public void ShowArPointDetailsButton(ScenarioController arModelPoint)
    {
        arPointDetailsButton.SetActive(true);

        arButtonBack.SetActive(true);

        currentArModelPoint = arModelPoint;
    }

    [SerializeField] private GameObject wi_ArPointDetails;

    public void OpenArPointDetails()
    {
        wi_ArPointDetails.SetActive(true);

        arPointDetailsButton.SetActive(false);
    }

    public void CloseArPointDetails()
    {
        wi_ArPointDetails.SetActive(false);
    }

    public void ArButtonBack()
    {
        arPointDetailsButton.SetActive(false);

        if (currentArModelPoint != null)
        {
            currentArModelPoint.ResetPoint();
        }

        wi_ArPointDetails.SetActive(false);

        arButtonBack.SetActive(false);
    }


    #region In Poi

    [SerializeField] private Ui_GPS_Poi_Menu _ui_GPS_Poi_Menu;
    public Ui_GPS_Poi_Menu poiMenu { get => _ui_GPS_Poi_Menu; }

    [SerializeField] private Ui_GPS_PoiNoAr_Menu _ui_GPS_PoiNoAr_Menu;
    public Ui_GPS_PoiNoAr_Menu poiNoArMenu { get => _ui_GPS_PoiNoAr_Menu; }

    public void ShowPoiInfo(GPS_Point_Config point)
    {
        if (point.isAr)
        {
            poiMenu. OpenPoi(point);
        }
        else
        {
            poiNoArMenu.OpenPoiNoAr(point);
        }
    }

    #endregion
    [SerializeField] private Sprite _modelSprite;
    [SerializeField] private Sprite _markerSprite;
    [SerializeField] private Image _arModeImage;
    [SerializeField] private TextMeshProUGUI _scanHelpText;
    [SerializeField] private GameObject _placeModelButton;
    public void SwitchArMode()
    {
        ARManager.Ins.SwitchArMode();

        CloseArModel();

        UpdateScanHint();

        MarkerScaner.SetActive(true);
    }

    public void UpdateScanHint()
    {
        switch (ARManager.Ins._ARState)
        {
            case ARState.Marker:
                {
                    _placeModelButton.SetActive(false);

                    _arModeImage.sprite = _modelSprite;

                    switch (language)
                    {
                        case Language.Rus: _scanHelpText.text = $"Наведите камеру на метку"; break;
                        case Language.Eng: _scanHelpText.text = $"Point the camera at the mark"; break;
                    }
                }
                break;

            case ARState.Plane:
                {
                    _placeModelButton.SetActive(true);

                    _arModeImage.sprite = _markerSprite ;

                    switch (language)
                    {
                        case Language.Rus: _scanHelpText.text = $"Наведите камеру на пол"; ; break;
                        case Language.Eng: _scanHelpText.text = $"Point the camera at the floor"; ; break;
                    }

                    ArVideoPlayer.ins.Stop();
                }
                break;
        }
    }


    public void PlaceMainModel()
    {
        ARManager.Ins.CreateMainModel();

        _placeModelButton.SetActive(false);
    }

    [SerializeField] private GameNotificationsManager _notificationManager;
    private void InitializeNotifications()
    {
        var chanel = new GameNotificationChannel("Nasledie", "gps warning", "enter gps zone");
        _notificationManager.Initialize(chanel);
    }
    public void CreateNotification(string title, string body, DateTime time)
    {
        var notification = _notificationManager.CreateNotification();

        if (notification != null)
        {
            notification.Title = title;
            notification.Body = body;
            notification.DeliveryTime = time;
            _notificationManager.ScheduleNotification(notification);
        }
    }
}
