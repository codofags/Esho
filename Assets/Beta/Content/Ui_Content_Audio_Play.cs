using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Ui_Content_Audio_Play : MonoBehaviour
{
    private Content_Audio_Config content;

    private void Start()
    {
        ButtonManager.ins.OnSwitchLanguage += OnSwitchLanguage;

        SwitchLanguage();
    }

    private void OnSwitchLanguage(object sender, EventArgs e)
    {
        SwitchLanguage();
    }

    [SerializeField] private TextMeshProUGUI _buttonText;
    [SerializeField] private string _rusText;
    [SerializeField] private string _engText;
    private void SwitchLanguage()
    {
        switch (ButtonManager.ins.language)
        {
            case Language.Rus: { _buttonText.text = _rusText; } break;
            case Language.Eng: { _buttonText.text = _engText; } break;
        }
    }

    internal void Assign(Content_Audio_Config content)
    {
        this.content = content;
    }

    public void PlayTrack()
    {
        AudioPlayerController.ins.PlayTrack(content);
    }
}
