using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioPin : MonoBehaviour
{
    public string PinInfo;

    public int AudioClipIndex;
    public Sprite DefaultSprite;
    public Sprite PinSprite;

    public GameObject BackGround;
    public Image PinImage;

    public PinState _PinState;

    public Sprite InfoSprite;
    public string InfoSpriteDescription;
    public string InfoCaption;
    public string InfoDescription;



    private void Start()
    {
        DefaultSprite = PinImage.sprite;

        _PinState = PinState.UnSelect;
    }

    public void SetPinState(PinState NewState)
    {
        _PinState = NewState;
    }

    public void TapPin()
    {
        switch (_PinState)
        {
            case PinState.UnSelect:
                {
                    SelectPin();
                }
                break;

            case PinState.Select:
                {
                    //OpenPin();
                    UnSelectPin();
                }
                break;

            case PinState.Open:
                {
                    OpenPin();

                }
                break;
        }


    }

    public void SelectPin()
    {
        //Debug.Log("select pin");
        ////отключаем другие пины
        //MapManager.Inst.ResetLocalPins();

        ////перенос пина в конец списка, чтобы отобразить поверх остальных
        //transform.SetAsLastSibling();

        ////показываем картинку пина
        //BackGround.SetActive(true);
        //PinImage.sprite = PinSprite;
        //PinImage.rectTransform.sizeDelta = new Vector2(PinSprite.texture.height, PinSprite.texture.width);

        ////меняем состояние пина
        //SetPinState(PinState.Select);

        ////воспроизводим аудио
        //AudioManager.Inst.PlayAudio(AudioClipIndex);

        ////показываем дескрипшен выделеного пина
        //GUIController.Inst.ShowInfoBubble(PinInfo, gameObject);
        ////GUIController.Inst.AssignPinInfo(this);
    }

    public void UnSelectPin()
    {
        //меняем состояние пина
        SetPinState(PinState.UnSelect);

        //скрываем картинку пина
        BackGround.SetActive(false);
        PinImage.sprite = DefaultSprite;
        PinImage.rectTransform.sizeDelta = new Vector2(DefaultSprite.texture.height, DefaultSprite.texture.width);

        //останавливаем аудио

        //скрываем инфо бабл
        GUIController.Inst.HideInfoBubble();
    }

    public void OpenPin()
    {
        Debug.Log("open local pin");

        //отображаем карту пина // маршруты
        //MapManager.Inst.SetMapState(MapState.Local,null,  this);

        //меняем состояние пина
        SetPinState(PinState.Open);
    }
}
