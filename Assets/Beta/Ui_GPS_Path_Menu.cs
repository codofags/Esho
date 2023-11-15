using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_GPS_Path_Menu : MonoBehaviour
{
    public static Ui_GPS_Path_Menu ins;
    private void Start()
    {
        ins = this;

        MapManager.Inst.onUserGpsUpdate += OnUserGpsUpdate;
        uiManager.OnSwitchLanguage += OnSwitchLanguage;
    }

    private void OnUserGpsUpdate(object sender, EventArgs e)
    {
        UpdatePathDistance();
    }

    [SerializeField] private ButtonManager uiManager;
    private void OnSwitchLanguage(object sender, EventArgs e)
    {
        SetupPathCaption(currentPath);
    }

    [SerializeField] private GameObject Path;

    //private PathConfig currentPath;
    private GPS_Path_Config currentPath;

    //public void OpenPath(PathConfig path)
    //{
    //    uiManager. navigationHistory.AddHistoryPoint(
    //     () =>
    //     {
    //         OpenPath(path);
    //     });

    //    uiManager. OpenPathContainer();

    //    currentPath = path;

    //    //��������� ����� ��������
    //    SetupPathCaption(path);

    //    //ContentManager.inst.AssignPath(path);
    //    Path.SetActive(true);

    //    uiManager.poiMenu.poi.SetActive(false);
    //    uiManager.poiNoArMenu.poiNoAr.SetActive(false);
    //}

    public void OpenPath(GPS_Path_Config path)
    {
        uiManager.navigationHistory.AddHistoryPoint(
         () =>
         {
             OpenPath(path);
         });

        uiManager.OpenPathContainer();

        currentPath = path;

        //��������� ����� ��������
        SetupPathCaption(path);

        AssignPath(path);

        Path.SetActive(true);

        uiManager.poiMenu.poi.SetActive(false);
        uiManager.poiNoArMenu.poiNoAr.SetActive(false);
    }

    [SerializeField] private TextMeshProUGUI pathNameText;
    [SerializeField] private TextMeshProUGUI pathTimeText;
    [SerializeField] private TextMeshProUGUI pathObjectsText;
    private void SetupPathCaption(GPS_Path_Config path)
    {
        //detailsGroup.enabled = false;

        if (currentPath == null) return;
        pathNameText.text = TranslateHelper.ins.GetText(path.pathName);

        //�����
        {
            //var duration = path.duration;

            //var hours = (int) duration / 60;
            //var minutes = duration - hours * 60;

            //var resultTime = "";

            //if (hours > 0)
            //{
            //    switch (uiManager.language)
            //    {
            //        case Language.Rus: resultTime += $"{hours} �."; break;
            //        case Language.Eng: resultTime += $"{hours} h."; break;
            //    }
            //}

            //if (minutes > 0)
            //{
            //    resultTime += " ";

            //    switch (uiManager.language)
            //    {
            //        case Language.Rus: resultTime += $"{minutes} �."; break;
            //        case Language.Eng: resultTime += $"{minutes} m."; break;
            //    }
            //}     
        }

        pathTimeText.text =  TranslateHelper.ins.GetText(path.duration); //resultTime;
        pathObjectsText.text = path.GetObjectCountText();

        StartCoroutine(UpdateCanvas());
    }

    [SerializeField] private HorizontalLayoutGroup detailsGroup;    
    [SerializeField] private HorizontalLayoutGroup distanceGroup;
    private IEnumerator UpdateCanvas()
    {       
        yield return new WaitForEndOfFrame();       
        
        detailsGroup.childAlignment = TextAnchor.UpperCenter;
        distanceGroup.childAlignment = TextAnchor.MiddleCenter;

        yield return new WaitForEndOfFrame();

        detailsGroup.childAlignment = TextAnchor.UpperLeft;
        distanceGroup.childAlignment = TextAnchor.MiddleLeft;
    }

    private IEnumerator UpdateDistanceCanvas()
    {
        yield return new WaitForEndOfFrame();

        distanceGroup.childAlignment = TextAnchor.MiddleCenter;

        yield return new WaitForEndOfFrame();

        distanceGroup.childAlignment = TextAnchor.MiddleLeft;
    }

    [SerializeField] private TextMeshProUGUI pathGpsDistance;
    private void UpdatePathDistance()
    {
        if (currentPath == null) return;

        var minDistance = float.MaxValue;

        //PointConfig pointConfig = null;
        GPS_Point_Config pointConfig = null;

        foreach (var p in currentPath.points)
        {
            var distance = MapManager.Inst.GetDistance(p.coord) / 1000;

            if (distance < minDistance)
            {
                minDistance = distance;
                pointConfig = p;
            }
        }

        if (minDistance < pointConfig.radius)
        {
            switch (uiManager.language)
            {
                case Language.Rus:
                    {
                        pathGpsDistance.text = "�� �� ��������";
                    }
                    break;
                case Language.Eng:
                    {
                        pathGpsDistance.text = "You are on route";
                    }
                    break;
            }
        }
        else
        {
            var distanceText = "";

            switch (uiManager.language)
            {
                case Language.Rus:
                    {
                        distanceText = "��";
                    }
                    break;
                case Language.Eng:
                    {
                        distanceText = "km";
                    }
                    break;
            }

            pathGpsDistance.text = $"{ (minDistance).ToString("f1")} {distanceText}";
        }

        StartCoroutine(UpdateDistanceCanvas());
    }

    public void ClosePath()
    {
        Path.SetActive(false);
    }

    //[SerializeField] private GameObject PathDescription;

    //public void OpenPathDescription()
    //{
    //    OpenPaths();
    //    PathDescription.SetActive(true);
    //}

    //public void ClosePathDescription()
    //{
    //    PathDescription.SetActive(false);
    //}

    public List<Sprite> testPathSprites = new List<Sprite>();

    [SerializeField] private Transform pathSpriteContainer;
    [SerializeField] private GameObject BigSpritePrefab;
    [SerializeField] private ImagePagination pathSpriteDrag;

    [SerializeField] private Transform pathContentContainer;

    public List<Sprite> pathMapsSprites = new List<Sprite>();
    [SerializeField] private TextTranslation objectOnPathTranslate;
    [SerializeField] private TextTranslation canSeeTranslate;
    internal void AssignPath(GPS_Path_Config path)
    {
        //��������� ������� �������� � �����        
        ContentManager.inst. ResetContainer(pathSpriteContainer);

        foreach (var s in path.sprites)
        {
            var newPathSprite = Instantiate(BigSpritePrefab, pathSpriteContainer);
            newPathSprite.transform.localScale = Vector3.one;

            var spriteComponent = newPathSprite.GetComponent<SpriteComponent>();
            spriteComponent.Assign(s);
        }

        pathSpriteDrag.Setup();


        //������� �������� ��������
        ContentManager.inst.ResetContainer(pathContentContainer);

        //ContentManager.inst.AddDragPin(pathContentContainer);

        //������� ������ ���������
        ContentManager.inst.AddCaption(path.content[0], pathContentContainer);
        ContentManager.inst.AddShortText((Content_Text_Config)path.content[0], pathContentContainer);

        //������� ������ ����� ��� �����������
        //ContentManager.inst.AddContent(path.content[1], pathContentContainer);

        //������� ������ "������ ���������"
        //ContentManager.inst.AddButtonFullReadPath(path);

        //ContentManager.inst.AddCaption(TranslateHelper.ins.GetText(objectOnPathTranslate), pathContentContainer);

        //��������� ��������� ����� ������
        switch (ButtonManager.ins.language)
        {
            case Language.Rus: ContentManager.inst.AddCaption("������� �� ��������:", pathContentContainer); break;
            case Language.Eng: ContentManager.inst.AddCaption("Objects on the route:", pathContentContainer); break;
        }       

        //������� ����� ��������
        ContentManager.inst.AddMap(path.mapSprite, pathContentContainer);

        //��������� ��� ���
        //ContentManager.inst.AddCaption(TranslateHelper.ins.GetText(canSeeTranslate), pathContentContainer);

        //������� ������ ���� ���������
        ContentManager.inst.AddAllPoints(path);

        ContentManager.inst.AddEmptyObject(460, pathContentContainer);
    }

    //private void AddButtonFullReadPath(PathConfig path)
    //{
    //    var newButton = Instantiate(buttonFullReadPrefab);

    //    var buttonComponent = newButton.GetComponent<Button>();

    //    buttonComponent.onClick.AddListener(() => ShowFullDescription(path));

    //    newButton.transform.SetParent(pathContentContainer);

    //    newButton.transform.localScale = Vector3.one;
    //}
}
