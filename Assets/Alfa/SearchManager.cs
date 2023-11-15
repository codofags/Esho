using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SearchManager : MonoBehaviour
{
    public static SearchManager ins;

    [SerializeField] private int minimumSearchCharsLimit;
    [SerializeField] private TMP_InputField searchField;

    [SerializeField] private Transform searchPathContainer;
    [SerializeField] private Transform searchPointContainer;

    [SerializeField] private VerticalLayoutGroup searchGroup;

    [SerializeField] private Transform searchContentContainer;

    [SerializeField] private List<string> searchKeys = new List<string>();

    void Start()
    {
        ins = this;

        LoadSearchKeys();


        if (searchKeys.Count > 0)
        {
            ShowSearchHistory();
        }
        
    }

    [SerializeField] private GameObject seacrhKeyPrefab;
    [SerializeField] private Transform searchKeysContainer;
    private void LoadSearchKeys()
    {
        for (int i = 0; i < 12; i++)
        {
            var saveKey = $"search{i}";

            if (PlayerPrefs.HasKey(saveKey))
            {
                var searchKey = PlayerPrefs.GetString(saveKey);
                searchKeys.Add(searchKey);
            }
        }    

        foreach(var k in searchKeys)
        {
            AddSearchKey(k);

            //Debug.Log($"{k}");
        }


        //Debug.Log( $" searchKeys.Count {searchKeys.Count}");

        if (searchKeys.Count == 0)
        {
            var caption = TranslateHelper.ins.GetText(nullSearchTranslate);
            SetLogoCaption(caption);
        }
    }

    //private int keyCount = 0;
    private void SaveKeys()
    {     
        if(searchKeys.Count > 5)
        {
            var keyList = new List<string>();

            for (int i = 1; i < searchKeys.Count ; i++)
            {
                keyList.Add(searchKeys[i]);
            }

            foreach (var item in searchKeys)
            {
                Debug.Log(item);
            }

            foreach (var item in keyList)
            {
                Debug.Log(item);
            }

            searchKeys = keyList;
        }

        var childCount = searchKeysContainer.childCount;

        if (childCount > 5)
        {
            Destroy(searchKeysContainer.GetChild(5).gameObject);
        }

        PlayerPrefs.DeleteAll();

        for (int i = 0; i < searchKeys.Count; i++)
        {
            var saveKey = $"search{i}";
            PlayerPrefs.SetString(saveKey, searchKeys[i]);
        }

        PlayerPrefs.Save();       
    }

    private void AddSearchKey(string key)
    {      
        var newKey = Instantiate(seacrhKeyPrefab, searchKeysContainer);

        newKey.transform.SetAsFirstSibling();

        var textComponent = newKey.GetComponent<TextMeshProUGUI>();

        textComponent.text = key;
    }

    public void ClearKeys()
    {
        ContentManager.inst.ResetContainer(searchKeysContainer);
        PlayerPrefs.DeleteAll();
        PlayerPrefs.Save();

        searchKeys.Clear();

        ShowLogo();
        var caption = TranslateHelper.ins.GetText(nullSearchTranslate);
        SetLogoCaption(caption);
        SearchHistory.SetActive(false);
    }

    public void SelectKey(string key)
    {
        searchField.text = key;

        CheckSearchText();
    }

    bool haveResult = false;

    [SerializeField] private TextTranslation nothingFoundTranslate;
    private List<GPS_Path_Config> resultPaths = new List<GPS_Path_Config>();
    private List<GPS_Point_Config> resultPoints = new List<GPS_Point_Config>();
    [SerializeField] private GameObject _pathsCaption;
    [SerializeField] private GameObject _pointsCaption;
    public void CheckSearchText()
    {
        resultPaths.Clear();
        resultPoints.Clear();

        haveResult = false;

        ContentManager.inst.ResetContainer(searchPathContainer);
        ContentManager.inst.ResetContainer(searchPointContainer);

        searchGroup.enabled = false;

        if(searchField.text.Length == 0)
        {
            ShowSearchHistory();
        }

        if (searchField.text.Length > 0 && searchField.text.Length < minimumSearchCharsLimit)
        {
            HideSearchHistory();
            HideSearchResult();
            ShowLogo();
            var caption = TranslateHelper.ins.GetText(nothingFoundTranslate);
            SetLogoCaption(caption);
        }

        if (searchField.text.Length >= minimumSearchCharsLimit) 
        {
            var searchText = searchField.text;

            //перебираем все маршруты
            foreach(var path in MapManager.Inst.paths)
            {
                //поиск в контенте маршрута
                foreach(var c in path.content)
                {
                    if (resultPaths.Contains(path))
                    {
                        break;
                    }

                    if(c is Content_Text_Config)
                    {
                        bool captionContains = ((Content_Text_Config)c).GetCaption().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                        bool textContains = ((Content_Text_Config)c).GetText().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;

                        if (captionContains || textContains) 
                        {
                            ContentManager.inst.AddMidPath(path, searchPathContainer);
                            resultPaths.Add(path);
                            haveResult = true;


                        }
                    }
                }

                //поиске в контенте точек маршрута
                foreach (var point in path.points)
                {
                    foreach (var c in point.content)
                    {
                        if (resultPoints.Contains(point))
                        {
                            break;
                        }

                        if (c is Content_Text_Config)
                        {
                            bool captionContains = ((Content_Text_Config)c).GetCaption().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                            bool textContains = ((Content_Text_Config)c).GetText().IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;

                            if (captionContains || textContains)                               
                            {
                                ContentManager.inst.AddMiniPoi(point, searchPointContainer);
                                resultPoints.Add(point);
                                haveResult = true;
                            }
                        }
                    }
                }
            }

            //search keys
            {
                //foreach(var path in MapManager.Inst.GetAllPath())
                //{
                //    //перебираем ключи поиска маршрутов
                //    foreach (var k in path.searchKeys)
                //    {
                //        if (k.Contains(searchText))
                //        {
                //            //выводим карточку найденного маршрута 
                //            //Debug.Log($"{k} => {searchText}");

                //            ContentManager.inst.AddMidPath(path, searchPathContainer);

                //            ShowSearchResult();

                //            haveResult = true;
                //        }
                //    }

                //    //перебираем в маршруте все точки
                //    foreach (var point in path.points)
                //    {
                //        //перебираем ключи поиска точек
                //        foreach (var k in point.searchKeys)
                //        {
                //            if (k.Contains(searchText))
                //            {
                //                //выводим карточку найденного маршрута 
                //                //Debug.Log($"{k} => {searchText}");

                //                ContentManager.inst.AddMiniPoi(point, searchPointContainer);

                //                ShowSearchResult();

                //                haveResult = true;
                //            }
                //        }
                //    }
                //}
            }

            StartCoroutine(SetupGroups());

            //StartCoroutine(ResizeSearchConteiner());
            ResizeSearchConteiner();


            _pathsCaption.SetActive(resultPaths.Count > 0);
            _pointsCaption.SetActive(resultPoints.Count > 0);
            //if (resultPoints.Count > 0)
            //{
            //    Debug.Log($"found point => {resultPoints.Count}");
            //}
            //else
            //{
            //    Debug.Log($"point not found");
            //}

            if (haveResult)
            {
                ShowSearchResult();
            }
            else
            {          
                HideSearchHistory();
                ShowLogo();
                var caption = TranslateHelper.ins.GetText(nothingFoundTranslate);
                SetLogoCaption(caption);
                HideSearchResult();
            }
        }
    }
 

    IEnumerator SetupGroups()
    {
        yield return new WaitForEndOfFrame();
        //searchGroup.CalculateLayoutInputVertical();
        searchGroup.enabled = true;
    }

    [SerializeField] private GameObject SearchHistory;
    [SerializeField] private TextTranslation nullSearchTranslate;
    private void ShowSearchHistory()
    {
        if (searchKeys.Count == 0)
        {
            ShowLogo();

            var caption = TranslateHelper.ins.GetText(nullSearchTranslate);
            SetLogoCaption(caption);

            SearchHistory.SetActive(false);
        }
        else
        {
            //показываем историю поиска
            SearchHistory.SetActive(true);
            HideLogo();
        }

        //прячем результаты поиска
        HideSearchResult();
    }
    private void HideSearchHistory()
    {
        SearchHistory.SetActive(false);
    }

    [SerializeField] private GameObject Logo;
    private void ShowLogo()
    {
        Logo.SetActive(true);
    }
    private void HideLogo()
    {
        Logo.SetActive(false);
    }

    [SerializeField] private TextMeshProUGUI logoCaption;
    private void SetLogoCaption(string caption)
    {
        logoCaption.text = caption;
    }

    [SerializeField] private GameObject searchResult;
    private void ShowSearchResult()
    {
        //показываем результат поиска
        searchResult.SetActive(true);

        //скрываем историю поиска
        HideSearchHistory();

        //скрываем лого
        HideLogo();
    }
    private void HideSearchResult()
    {
        searchResult.SetActive(false);
    }
          
    private void ResizeSearchConteiner()
    {
        var resultHeight = 0f;

        //прибавляем плашку с пином (драг)
        resultHeight += 100;

        //прибавляем отступ
        //resultHeight += 50;

        //прибавляем заголовок 
        resultHeight += 90;

        //прибавляем отступ
        //resultHeight += 50;

        //прибавляем высоту контейнера с маршрутами   
        var pathCount = searchPathContainer.childCount;
        var pathHeight = (545 * pathCount) + ((pathCount - 1) * 50);

        resultHeight += pathHeight;

        //прибавляем отступ
        //resultHeight += 50;

        //прибавляем заголовок 
        resultHeight += 90;

        //прибавляем отступ
        //resultHeight += 50;

        //прибавляем высоту контейнера с точками   
        var pointCount = searchPointContainer.childCount;       
        var pointHeight= (490 * pointCount) + ((pointCount - 1) * 50);

        resultHeight += pointHeight;

        //прибавляем отступы
        var childCount = searchContentContainer.childCount;
        resultHeight += (childCount - 1) * 50;

        //прибавляем размер плашки меню
        resultHeight += 300;

        searchContentContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(0, resultHeight);

        //Debug.Log($" {pathHeight} {pointHeight} => {resultHeight}");

    }

    public void OnSearchSelect()
    {
        //Debug.Log($" Search Select");
    }

    
    public void OnSearchDeselect()
    {
        if (!haveResult) { return; }

        var key = searchField.text;

        if (searchKeys.Contains(key) || string.IsNullOrEmpty(key)) { return; }

        searchKeys.Add(key);

        AddSearchKey(key);

        SaveKeys();

        //Debug.Log($"save new key {key}");
    }

}

public class SearchKeys
{
    List<string> keys = new List<string>(); 
}
