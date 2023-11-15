using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ContentManager : MonoBehaviour
{
    public static ContentManager inst;

    //сценарии
    public GameObject ScenarioPrefab;
    public GameObject ScenarioContainer;

    public GameObject pathsFullContent;
    public Transform pathsFullContentContainer;

    //контейнеры для плашек маршрутов

    //главная
    //контейнер для "маршуты рядом с Вами"
    public Transform mainMaxPathsContainer;
    //контейнер для всех пои 
    public Transform mainMiniPoisContainer;


    //маршруты
    //контейнер для маршуты "3 км от вас"
    public GameObject pathDistancePathsCaption;
    public GameObject pathDistancePathsContainer;
    public VerticalLayoutGroup pathDistanceGroup;

    public Transform pathMaxPathsContainer;
    //контейнер для все маршруты
    public Transform pathMidPathsContainer;
    //контейнер для все пои
    public Transform pathMiniPoisContainer;


    public Transform Trash;

    private void Awake()
    {
        inst = this;
    }

    #region Help Staff

    public void MoveToTrash(GameObject trashObject)
    {
        trashObject.transform.SetParent(Trash);
        Destroy(trashObject);
    }

    public void ResetContainer(Transform container)
    {
        var childCount = container.childCount;

        for (int i = childCount -1; i >=0; i--)
        {
            var child = container.GetChild(i).gameObject;

            MoveToTrash(child);
        }
    }

    public void ClearContainer(Transform container)
    {
        var childCount = container.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var child = container.GetChild(i).gameObject;

            MoveToTrash(child);
        }
    }

    #endregion

    #region Add Point
    [SerializeField] private Transform pointSpriteContainer;
    [SerializeField] private ImagePagination pointSpriteDrag;

    [SerializeField] private Transform pointContentContainer;

    
    [SerializeField] private GameObject BigSpritePrefab;
    [SerializeField] private Ui_Content_Selector _uiContentSelectorPrefab;
    [SerializeField] private Transform _pointContentSelectorsContainer;
    [SerializeField] private GameObject _buttonStartScan;
    public void AssignPoint(GPS_Point_Config point)
    {
        //настраиваем спрайты в шапке поинта
        
            ResetContainer(pointSpriteContainer);

            if (point.sprites.Count > 0)
            {
                foreach (var s in point.sprites)
                {
                    var newPointSprite = Instantiate(BigSpritePrefab, pointSpriteContainer);
                    newPointSprite.transform.localScale = Vector3.one;

                    var spriteComponent = newPointSprite.GetComponent<SpriteComponent>();
                    spriteComponent.Assign(s);
                }
            }

            pointSpriteDrag.Setup();

        _buttonStartScan.SetActive(point.isAr);

        ResetContainer(pointContentContainer);

        //AddDragPin(pointContentContainer);

        if (point.content.Count > 0)
        {
            //выводим первый заголовок
            AddContent(point.content[0], pointContentContainer);

            //выводим первый текст или изображение
            //AddContent(point.content[1], pointContentContainer);

            //выводим кнопку "читать полностью"
            //AddButtonFullReadPoint(point);
        }

        foreach (var c in point.content)
        {
            var newUiContentSelector = Instantiate(_uiContentSelectorPrefab, pointContentContainer);
            newUiContentSelector.AssignTextContent((Content_Text_Config)c);
        }

        AddEmptyObject(460, pointContentContainer);
    }

   

    [SerializeField] private GameObject miniPoiPrefab;
    //internal void AddMiniPoi(PointConfig point, Transform poiContainer)
    //{
    //    var newPoi = Instantiate(miniPoiPrefab, poiContainer);
    //    newPoi.transform.localScale = Vector3.one;

    //    var poiComponent = newPoi.GetComponent<MiniPoiComponent>();
    //    poiComponent.Assign(point);
    //}

    internal void AddMiniPoi(GPS_Point_Config point, Transform poiContainer, bool subscribeGPS=false)
    {
        var newPoi = Instantiate(miniPoiPrefab, poiContainer);
        newPoi.transform.localScale = Vector3.one;

        var poiComponent = newPoi.GetComponent<MiniPoiComponent>();
        poiComponent.Assign(point, subscribeGPS);
    }

    [SerializeField] private GameObject midPoiPrefab;
    public void AddMidPoi(GPS_Point_Config point, Transform container)
    {
        var newPoi = Instantiate(midPoiPrefab, container);
        newPoi.transform.localScale = Vector3.one;
        newPoi.transform.localPosition = Vector3.zero;

        var poiComponent = newPoi.GetComponent<MidPoiComponent>();
        poiComponent.Assign(point);
    }
    #endregion

    #region Add Paths
    //public List<Sprite> testPathSprites = new List<Sprite>();

    //[SerializeField] private Transform pathSpriteContainer;
    //[SerializeField] private GameObject BigSpritePrefab;
    //[SerializeField] private ImagePagination pathSpriteDrag;

    //[SerializeField] private Transform pathContentContainer;

    //public List<Sprite> pathMapsSprites = new List<Sprite>();
    //[SerializeField] private TextTranslation objectOnPathTranslate;
    //[SerializeField] private TextTranslation canSeeTranslate;
    //internal void AssignPath(PathConfig path)
    //{
    //    //загружаем спрайты маршрута в шапку        
    //    ResetContainer(pathSpriteContainer);

    //    foreach (var s in path.sprites)
    //    {
    //        var newPathSprite = Instantiate(BigSpritePrefab, pathSpriteContainer);
    //        newPathSprite.transform.localScale = Vector3.one;

    //        var spriteComponent = newPathSprite.GetComponent<SpriteComponent>();
    //        spriteComponent.Assign(s);
    //    }

    //    pathSpriteDrag.Setup();


    //    //верстка контента маршрута
    //    ResetContainer(pathContentContainer);

    //    AddDragPin(pathContentContainer);

    //    //выводим первый заголовок
    //    AddContent(path.content[0], pathContentContainer);

    //    //выводим первый текст или изображение
    //    AddContent(path.content[1], pathContentContainer);

    //    //выводим кнопку "читать полностью"
    //    AddButtonFullReadPath(path);

    //    AddCaption(TranslateHelper.ins.GetText(objectOnPathTranslate), pathContentContainer);

    //    //выводим карту маршрута
    //    AddMap(path.mapSprite, pathContentContainer);

    //    //заголовок для пои
    //    AddCaption(TranslateHelper.ins.GetText(canSeeTranslate), pathContentContainer);

    //    //выводим список всех сценариев
    //    AddAllPoints(path);
    //}
    
   

    [SerializeField] private ImagePagination MainMaxPathsDrag;
    internal void SetupMainMaxPathsContainer()
    {
        MainMaxPathsDrag.Setup();
    }

    [SerializeField] private ImagePagination PathMaxPathsDrag;
    internal void SetupPathMaxPathsContainer()
    {
        PathMaxPathsDrag.Setup();
    }

    [SerializeField] private GameObject maxPathPrefab;
    //internal void AddMaxPath(PathConfig path, Transform container, bool hideByDistance = false)
    //{
    //    var newPath = Instantiate(maxPathPrefab, container);
    //    newPath.transform.localScale = Vector3.one;

    //    var pathComponent = newPath.GetComponent<MaxPathComponent>();
    //    pathComponent.Assign(path, hideByDistance);

    //    pathComponent.OnHideByDistance += CheckPathsByDistance;
    //}

    internal void AddMaxPath(GPS_Path_Config path, Transform container, bool hideByDistance = false)
    {
        var newPath = Instantiate(maxPathPrefab, container);
        newPath.transform.localScale = Vector3.one;

        var pathComponent = newPath.GetComponent<MaxPathComponent>();
        pathComponent.Assign(path, hideByDistance);

        pathComponent.OnHideByDistance += CheckPathsByDistance;
    }

    private void CheckPathsByDistance(object sender, EventArgs e)
    {
        var havePath = false;
        //если нет маршрутов в пределах 3км, 
        var childCount = pathMaxPathsContainer.childCount;

        for (int i = 0; i < childCount; i++)
        {
            var child = pathMaxPathsContainer.GetChild(i).gameObject;

            if (child.activeSelf)
            {
                havePath = true;
            }
        }
        //скрываем заголовок
        pathDistancePathsCaption.SetActive(havePath);
        //скрываем контейнер с маршрутами
        pathDistancePathsContainer.gameObject.SetActive(havePath);

        pathDistanceGroup.enabled = false;
        pathDistanceGroup.enabled = true;
    }

    [SerializeField] private GameObject midPathPrefab;
    //public void AddMidPath(PathConfig path, Transform container)
    //{
    //    var newPath = Instantiate(midPathPrefab, container);
    //    newPath.transform.localScale = Vector3.one;

    //    var pathComponent = newPath.GetComponent<MidPathComponent>();
    //    pathComponent.Assign(path);
    //}

    public void AddMidPath(GPS_Path_Config path, Transform container)
    {
        var newPath = Instantiate(midPathPrefab, container);
        newPath.transform.localScale = Vector3.one;

        var pathComponent = newPath.GetComponent<MidPathComponent>();
        pathComponent.Assign(path);
    }

    [SerializeField] private TextMeshProUGUI pathCountText;
    [SerializeField] private TextTranslation pathCountTranslate;
    internal void SetupPathCount(int count)
    {
        pathCountText.text = $"{TranslateHelper.ins.GetText(pathCountTranslate)}: {count}";
    }
    #endregion

    #region Add Content
    //private void AddContent(Content content, Transform container)
    //{
    //    switch (content.contnentType)
    //    {
    //        case ContnentType.Caption:
    //            {
    //                AddCaption(content, container);
    //            }
    //            break;

    //        case ContnentType.Text:
    //            {
    //                AddShortText(content, container);
    //            }
    //            break;

    //        case ContnentType.Image:
    //            {
    //                AddImage(content, container);
    //            }
    //            break;
    //    }
    //}

    public void AddContent(Content_Config content, Transform container)
    {
        if(content is Content_Text_Config)
        {
            AddShortText((Content_Text_Config)content, container);
        }

        //switch (content.contnentType)
        //{
        //    case ContnentType.Caption:
        //        {
        //            AddCaption(content, container);
        //        }
        //        break;

        //    case ContnentType.Text:
        //        {
        //            AddShortText(content, container);
        //        }
        //        break;

        //    case ContnentType.Image:
        //        {
        //            AddImage(content, container);
        //        }
        //        break;
        //}
    }

    public void AddMap(Sprite sprite, Transform container)
    {
        var newImage = Instantiate(imagePrefab);

        var imageComponent = newImage.GetComponent<ImageContentComponent>();

        imageComponent.Assign(sprite);

        newImage.transform.SetParent(container);

        newImage.transform.localScale = Vector3.one;
        Debug.Log("add image");
    }

    //public void AddAllContent(PointConfig point, Transform container)
    //{
    //    ResetContainer(container);

    //    AddDragPin(container);

    //    foreach (var c in point.content)
    //    {
    //        switch (c.contnentType)
    //        {
    //            case ContnentType.Caption:
    //                {
    //                    AddCaption(c, container);
    //                }
    //                break;

    //            case ContnentType.Text:
    //                {
    //                    AddShortText(c, container);
    //                }
    //                break;

    //            case ContnentType.Image:
    //                {
    //                    AddImage(c, container);
    //                }
    //                break;
    //        }
    //    }

    //    Debug.Log("full descr");
    //}   

    //public void AddAllContent(PathConfig path, Transform container)
    //{
    //    ResetContainer(container);

    //    AddDragPin(container);

    //    foreach (var c in path.content)
    //    {
    //        //Debug.Log(c);

    //        if (c is CaptionContent)
    //        {
    //            AddCaption((CaptionContent)c, container);
    //        }

    //        if (c is TextContent)
    //        {
    //            AddShortText((TextContent)c, container);
    //        }

    //        if (c is ImageContent)
    //        {

    //        }
    //    }

    //    Debug.Log("full descr");
    //}

    //public void AddAllContent(ContentPack contentPack, Transform container)
    //{
    //    ResetContainer(container);

    //    AddDragPin(container);

    //    foreach (var c in contentPack.content)
    //    {
    //        //Debug.Log(c);

    //        if (c is CaptionContent)
    //        {
    //            AddCaption((CaptionContent)c, container);
    //        }

    //        if (c is TextContent)
    //        {
    //            AddShortText((TextContent)c, container);
    //        }

    //        if (c is ImageContent)
    //        {
    //            AddImage((ImageContent)c, container);
    //        }
    //    }

    //    Debug.Log("full descr");
    //}
    #endregion

    #region Add Content Elements    

    public GameObject dragPinPrefab;
    public void AddDragPin(Transform container)
    {
        var newDragPin = Instantiate(dragPinPrefab);

        newDragPin.transform.SetParent(container);

        newDragPin.transform.localScale = Vector3.one;
    }

    public GameObject captionPrefab;
    //public void AddCaption(Content c, Transform container)
    //{
    //    var newCaption = Instantiate(captionPrefab);

    //    var captionComponent = newCaption.GetComponent<TextMeshProUGUI>();

    //    captionComponent.text = TranslateHelper.ins.GetText(c.translation);

    //    newCaption.transform.SetParent(container);

    //    newCaption.transform.localScale = Vector3.one;
    //}

    public void AddCaption(Content_Config content, Transform container)
    {
        var newCaption = Instantiate(captionPrefab);

        var captionComponent = newCaption.GetComponent<TextMeshProUGUI>();

        captionComponent.text = content.GetCaption(); // TranslateHelper.ins.GetText(c.translation);

        newCaption.transform.SetParent(container);

        newCaption.transform.localScale = Vector3.one;
    }

   public void AddCaption(string text, Transform container)
    {
        var newCaption = Instantiate(captionPrefab);

        var captionComponent = newCaption.GetComponent<TextMeshProUGUI>();

        captionComponent.text = text;

        newCaption.transform.SetParent(container);

        newCaption.transform.localScale = Vector3.one;
    }

    [SerializeField] private TextMeshProUGUI _textPrefab;
    public void AddText(Content_Text_Config c, Transform container)
    {
        var newText = Instantiate(_textPrefab, container);
        newText.transform.localScale = Vector3.one;
        newText.text = c.GetText();       
    }

    [SerializeField] private Ui_Content_Audio_Play _ui_AudioButton;
    public void AddAudioButton(Content_Audio_Config c, Transform container)
    {
        var newUiAudioButton = Instantiate(_ui_AudioButton, container);
        newUiAudioButton.transform.localScale = Vector3.one;

        newUiAudioButton.Assign(c);
    }
    [SerializeField] private Ui_Content_Video_Play _ui_VideoButton;
    public void AddVideoButton(Content_Video_Config c, Transform container)
    {
        var newUiVideoButton = Instantiate(_ui_VideoButton, container);
        newUiVideoButton.transform.localScale = Vector3.one;

        newUiVideoButton.Assign(c);
    }

    public GameObject shortTextPrefab;
    //private void AddShortText(Content c, Transform container)
    //{
    //    var newText = Instantiate(shortTextPrefab);

    //    var textComponent = newText.GetComponent<TextMeshProUGUI>();

    //    textComponent.text = TranslateHelper.ins.GetText(c.translation);

    //    newText.transform.SetParent(container);

    //    newText.transform.localScale = Vector3.one;
    //}

    public void AddShortText(Content_Text_Config content, Transform container)
    {
        var newText = Instantiate(shortTextPrefab);

        var textComponent = newText.GetComponent<TextMeshProUGUI>();

        textComponent.text = content.GetText();// TranslateHelper.ins.GetText(c.text);

        newText.transform.SetParent(container);

        newText.transform.localScale = Vector3.one;
    }

    public GameObject imagePrefab;
    private void AddImage(Content c, Transform container)
    {
        var newImage = Instantiate(imagePrefab);

        var imageComponent = newImage.GetComponent<ImageContentComponent>();

        imageComponent.Assign(c.sprite);

        newImage.transform.SetParent(container);

        newImage.transform.localScale = Vector3.one;
        Debug.Log("add image");

    }

    public GameObject mapImagePrefab;
    private void AddMapImage(MapImageContent c, Transform container)
    {
        var newImage = Instantiate(mapImagePrefab);

        var imageComponent = newImage.GetComponent<ImageContentComponent>();

        imageComponent.Assign(c.sprite);

        newImage.transform.SetParent(container);

        newImage.transform.localScale = Vector3.one;
        Debug.Log("add map image");

    }    

    public GameObject buttonFullReadPrefab;

    //private void AddButtonFullReadPoint(PointConfig point)
    //{
    //    var newButton = Instantiate(buttonFullReadPrefab);

    //    var buttonComponent = newButton.GetComponent<Button>();

    //    buttonComponent.onClick.AddListener(() => ShowFullDescription(point));

    //    newButton.transform.SetParent(pointContentContainer);

    //    newButton.transform.localScale = Vector3.one;
    //}

  


    #endregion

    public Transform FullDescriptionContainer;

    //public void ShowFullDescription(PointConfig point)
    //{
    //    AddAllContent(point, FullDescriptionContainer);

    //    //показать окно описания
    //    ButtonManager.ins.OpenScenarioDescription();
    //}

    //private void ShowFullDescription(PathConfig path)
    //{
    //    AddAllContent(path, FullDescriptionContainer);

    //    //показать окно описания
    //    ButtonManager.ins.OpenScenarioDescription();
    //}

    private void AddAllScenarios(PointConfig point)
    {
        var newScenarioContainer = Instantiate(ScenarioContainer);

        newScenarioContainer.transform.SetParent(pointContentContainer);

        newScenarioContainer.transform.localScale = Vector3.one;

        int scenarioNumber = 1;

        foreach (var s in point.scenarios)
        {
            var newScenario = Instantiate(ScenarioPrefab);

            newScenario.transform.SetParent(newScenarioContainer.transform);

            newScenario.transform.localScale = Vector3.one;

            var scenarioComponent = newScenario.GetComponent<ScenarioGuiController>();

            scenarioComponent.AssignScenario(scenarioNumber++, s);
        }
    }

    [SerializeField] private GameObject PointsContainer;
    [SerializeField] private GameObject pointPrefab;
    [SerializeField] private Transform pathContentContainer;
    //public void AddAllPoints(PathConfig path)
    //{
    //    var newPointsContainer = Instantiate(PointsContainer);

    //    newPointsContainer.transform.SetParent(pathContentContainer);

    //    newPointsContainer.transform.localScale = Vector3.one;

    //    foreach (var p in path.points)
    //    {
    //        var newPoint = Instantiate(pointPrefab);

    //        newPoint.transform.SetParent(newPointsContainer.transform);

    //        newPoint.transform.localScale = Vector3.one;

    //        var pointComponent = newPoint.GetComponent<PointGuiComponent>();

    //        pointComponent.Assign(p);
    //    }

    //    Debug.Log(newPointsContainer.name);
    //}
    public void AddAllPoints(GPS_Path_Config path)
    {
       

        var newPointsContainer = Instantiate(PointsContainer);

        newPointsContainer.transform.SetParent(pathContentContainer);

        newPointsContainer.transform.localScale = Vector3.one;

        foreach (var p in path.points)
        {
            var newPoint = Instantiate(pointPrefab);

            newPoint.transform.SetParent(newPointsContainer.transform);

            newPoint.transform.localScale = Vector3.one;

            var pointComponent = newPoint.GetComponent<PointGuiComponent>();

            pointComponent.Assign(p);
        }

        Debug.Log(newPointsContainer.name);
    }

    [SerializeField] private RectTransform EmptyObject;
    public void AddEmptyObject(float sizeY, Transform container)
    {
        var newEmptyObject = Instantiate(EmptyObject, container);
        newEmptyObject.sizeDelta = new Vector2(0, sizeY);
    }

    #region Poi Audio

    [SerializeField] private GameObject poiAudioPrefab;
    public void AddPoiAudio(GPS_Point_Config point, Transform container)
    {
        var newAudio = Instantiate(poiAudioPrefab, container);
        newAudio.transform.localScale = Vector3.one;
        newAudio.transform.localPosition = Vector3.zero;

        var audioComponent = newAudio.GetComponent<PoiAudioComponent>();
        audioComponent.Assign(point);
    }

    #endregion
}
