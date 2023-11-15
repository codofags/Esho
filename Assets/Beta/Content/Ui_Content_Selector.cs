using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Ui_Content_Selector : MonoBehaviour
{
    [SerializeField] private Image ico;
    [SerializeField] private TextMeshProUGUI caption;

    public Content_Text_Config config { get; private set; }
    public void AssignTextContent(Content_Text_Config config)
    {
        this.config = config;

        if (config.ico.Count > 0)
        {
            ico.sprite = config.ico[0];
        }

        caption.text = config.GetCaption();
    }

    public void OpenContent()
    {
        Ui_Content_Menu.ins.AssignTextContent(config);
    }
}
