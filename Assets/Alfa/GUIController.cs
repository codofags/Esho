using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum MenuState : int
{
    Open,
    Close,
    Idle,
}

public class GUIController : MonoBehaviour
{
    public static GUIController Inst;

    [Header("Top menu")]
    public RectTransform TopMenuTransform;
    public GameObject TopSeparatorTop;
    public GameObject TopSeparatorBot;
    public GameObject TopScrollView;
    public GameObject TopDragPin;
    public GameObject TopHidePin;

    public MenuState TopMenuState = MenuState.Idle;

    [Header("Bot menu")]
    public RectTransform BotMenuTransform;

    //public GameObject BotSeparatorTop;
    //public GameObject BotSeparatorBot;
    public GameObject BotScrollView;
    //public GameObject BotDragPin;
    public GameObject BotHidePin;
    //public GameObject ButtonBack;

    public MenuState BotMenuState = MenuState.Idle;

    Vector2 ScreenSize;

    public float OpenSpeed = 0.14f;


    public GameObject MainMenu;
    public GameObject DynamicContent;

    public GameObject GO_InfoBubble;
    public InfoBuble _InfoBubble;
    public Vector3 InfoBubbleOffset = new Vector3(0, -200, 0);

    public GameObject GO_LocalPinInfo;
    public LocalPinInfo _LocalPinInfo;
    public Button ButtonPinInfo;

    public GameObject GO_Warning;
    public WarningController _WarningController;

    private void Start()
    {
        if (Inst == null)
        {
            Inst = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        ScreenSize = new Vector2(Screen.width, Screen.height);

        HideTopMenu();
        HideBotMenu();
        HideInfoBubble();
    }

   

    private void Update()
    {
        CheckTopMenu();
        CheckBotMenu();
    }

    public void ShowTopMenu()
    {
        SetTopMenuState(MenuState.Open);

        //закрываем нижнее меню, если открыто
        SetBotMenuState(MenuState.Close);
    }

    public void HideTopMenu()
    {
        SetTopMenuState(MenuState.Close);
    }

    public void SetTopMenuState(MenuState NewState)
    {
        TopMenuState = NewState;

        switch (TopMenuState)
        {
            case MenuState.Idle:
                { }
                break;
            case MenuState.Open:
                {
                    TopSeparatorTop.SetActive(true);
                    TopSeparatorBot.SetActive(true);
                    TopScrollView.SetActive(true);

                    TopDragPin.SetActive(false);
                    TopHidePin.SetActive(true);                    
                }
                break;
            case MenuState.Close:
                {
                    TopSeparatorTop.SetActive(false);
                    TopSeparatorBot.SetActive(false);
                    TopScrollView.SetActive(false);

                    TopDragPin.SetActive(true);
                    TopHidePin.SetActive(false);
                }
                break;
        }

       
    }

    public void ShowBotMenu()
    {
        SetBotMenuState(MenuState.Open);

        //закрываем верхнее меню, если открыто
        SetTopMenuState(MenuState.Close);
    }

    public void HideBotMenu()
    {
        SetBotMenuState(MenuState.Close);
    }

    public void SetBotMenuState(MenuState NewState)
    {
        BotMenuState = NewState;

        switch (BotMenuState)
        {
            case MenuState.Idle:
                { }
                break;
            case MenuState.Open:
                {
                    //BotSeparatorTop.SetActive(true);
                    //BotSeparatorBot.SetActive(true);
                    BotScrollView.SetActive(true);

                    //BotDragPin.SetActive(false);
                    BotHidePin.SetActive(true);
                }
                break;
            case MenuState.Close:
                {
                    //TopSeparatorTop.SetActive(false);
                    //TopSeparatorBot.SetActive(false);
                    BotScrollView.SetActive(false);

                    //TopDragPin.SetActive(true);
                    BotHidePin.SetActive(false);
                }
                break;
        }
    }

   

    private void CheckTopMenu()
    {
        switch (TopMenuState)
        {
            case MenuState.Idle:
                {

                }
                break;
            case MenuState.Open:
                {
                    OpenTopDragMenu();
                }
                break;
            case MenuState.Close:
                {
                    CloseTopDragMenu();
                }
                break;
        }       
    }

    private void CheckBotMenu()
    {
        switch (BotMenuState)
        {
            case MenuState.Idle:
                {

                }
                break;
            case MenuState.Open:
                {
                    OpenBotDragMenu();
                }
                break;
            case MenuState.Close:
                {
                    CloseBotDragMenu();
                }
                break;
        }
    }

    private void OpenTopDragMenu()
    {
        var TargetY = ScreenSize.y;

        var SmoothY = Mathf.Lerp(TopMenuTransform.sizeDelta.y, TargetY, OpenSpeed * Time.deltaTime);

        TopMenuTransform.sizeDelta = new Vector2(TopMenuTransform.sizeDelta.x, SmoothY);
    }

    private void CloseTopDragMenu()
    {
        var TargetY = 150;

        var SmoothY = Mathf.Lerp(TopMenuTransform.sizeDelta.y, TargetY, OpenSpeed * Time.deltaTime);

        TopMenuTransform.sizeDelta = new Vector2(TopMenuTransform.sizeDelta.x, SmoothY);
    }

    private void OpenBotDragMenu()
    {
        var TargetY = ScreenSize.y - 150;

        var SmoothY = Mathf.Lerp(BotMenuTransform.sizeDelta.y, TargetY, OpenSpeed * Time.deltaTime);

        BotMenuTransform.sizeDelta = new Vector2(BotMenuTransform.sizeDelta.x, SmoothY);
    }

    private void CloseBotDragMenu()
    {
        var TargetY = 0;

        var SmoothY = Mathf.Lerp(BotMenuTransform.sizeDelta.y, TargetY, OpenSpeed * Time.deltaTime);

        BotMenuTransform.sizeDelta = new Vector2(BotMenuTransform.sizeDelta.x, SmoothY);
    }

    public void ShowDynamicContent()
    {
        MainMenu.SetActive(false);
        DynamicContent.SetActive(true);

        SetTopMenuState(MenuState.Open);
    }

    public void ShowMainMenu()
    {
        DynamicContent.SetActive(false);
        MainMenu.SetActive(true);

        SetTopMenuState(MenuState.Open);
    }

    public void ShowInfoBubble(string NewContent, GameObject Pin)
    {
        var PinPosition = Pin.transform.position;

        GO_InfoBubble.transform.position = new Vector3(PinPosition.x, PinPosition.y - InfoBubbleOffset.y, 0);

        //Debug.Log(PinPosition);
        //Debug.Log(GO_InfoBubble.transform.position);

        _InfoBubble.SetContent(NewContent);
        GO_InfoBubble.SetActive(true);

        //Debug.Log("show pin description");
    }

    public void HideInfoBubble()
    {
        GO_InfoBubble.SetActive(false);
    }

    //назначаем действие на кнопку "подробнее" во всплывающей подсказке о пине
    //на локальной карте, передвая в нее содержимое конкретного пина
    public void AssignPinInfo(LocalPinController LPin)
    {
        ButtonPinInfo.onClick.RemoveAllListeners();

        ButtonPinInfo.onClick.AddListener(() => ShowLocalPinInfo(LPin));
    }

    //заполняем карточку с подробностями о пине
    //отображаем карточку
    public void ShowLocalPinInfo(LocalPinController LPin)
    {
        _LocalPinInfo.SetInfo(LPin);
        GO_LocalPinInfo.SetActive(true);    

        SetTopMenuState(MenuState.Open);
    }

    public void OpenAR()
    {
        //SetTopMenuState(MenuState.Close);

        //MapManager.Inst.SetMapState(MapState.None);

        //SceneManager.LoadScene("AR");
        ////SceneLoader.Inst.LoadScene("AR");
    }

    public void ShowWarning(string Caption, string Description)
    {
        _WarningController.AssignWarning(Caption, Description);
        GO_Warning.SetActive(true);
    }
}
