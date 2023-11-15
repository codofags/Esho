using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Content_Menu : MonoBehaviour
{
    public ContentManager uiContent;

    public static Ui_Content_Menu ins;
    private void Start()
    {
        ins = this;
    }

    [SerializeField] private GameObject _menu;
    [SerializeField] private Transform _headerImagesContainer;
    [SerializeField] private ImagePagination _headerImagesPagination;
    [SerializeField] private TextMeshProUGUI _contentNameText;
    [SerializeField] private Transform _contentContainer;
    [SerializeField] private SpriteComponent BigSpritePrefab;
    [SerializeField] private VerticalLayoutGroup _contentGroup;
    public void AssignTextContent(Content_Text_Config content)
    {
        //настраиваем спрайты в шапке 
        uiContent.ClearContainer(_headerImagesContainer);

        foreach (var i in content.ico)
        {
            var newHeaderImage = Instantiate(BigSpritePrefab, _headerImagesContainer);
            newHeaderImage.transform.localScale = Vector3.one;
            newHeaderImage.Assign(i);
        }

        _headerImagesPagination.Setup();

        uiContent.ClearContainer(_contentContainer);

        //имя/заголовок
        _contentNameText.text = content.GetCaption();

        _contentGroup.enabled = false;

        //uiContent. AddDragPin(_contentContainer);   

        foreach (var c in content.subContent)
        {
            if(c is Content_Audio_Config)
            {
                uiContent.AddAudioButton((Content_Audio_Config)c, _contentContainer);
            }

            if(c is Content_Video_Config)
            {
                uiContent.AddVideoButton((Content_Video_Config)c, _contentContainer);
            }            
        }

        uiContent.AddText(content, _contentContainer);


        uiContent.AddEmptyObject(460, _contentContainer);

        _menu.SetActive(true);

        _contentGroup.enabled = true;

        Canvas.ForceUpdateCanvases();
    }
}
