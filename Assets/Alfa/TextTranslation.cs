using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class TextTranslation 
{
    [SerializeField] private List<TranslateConfig> translates = new List<TranslateConfig>();

    private TextMeshProUGUI textComponent;

    public void SetTextComponent(TextMeshProUGUI textComponent)
    {
        this.textComponent = textComponent;
    }

    public void SubscribeToSwitchLanguage()
    {
        ButtonManager.ins.OnSwitchLanguage += OnSwitchLanguage;
    }

    private void OnSwitchLanguage(object sender, System.EventArgs e)
    {
        Setup();
    }

    public void Setup()
    {
        var language = ButtonManager.ins.GetLanguage();

        var translateConfig = FindTranslateConfig(language);

        if (translateConfig == null)
        {
            Debug.Log($"cant translate text to {language}");
            return;
        }

        this.textComponent.text = translateConfig.text;
    }

    public TranslateConfig FindTranslateConfig(Language language)
    {
        var result = translates.Find(x => x.language == language);

        return result;
    }
}
