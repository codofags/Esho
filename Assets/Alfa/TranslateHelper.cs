using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslateHelper : MonoBehaviour
{
    public static TranslateHelper ins;
    [SerializeField] private ButtonManager buttonManager;

    private void Awake()
    {
        ins = this;
    }

    public string GetText(TextTranslation translation)
    {
        Language language = buttonManager.GetLanguage();

        var translateConfig = translation.FindTranslateConfig(language);

        if (translateConfig == null)
        {
            return "cant translate";
        }

        return translateConfig.text;
    }

    public string GetTimeText(float time)
    {
        Language language = buttonManager.GetLanguage();

        switch (language)
        {
            default:
            case Language.Rus:
                {
                    return $"Время: {time}";
                }
            case Language.Eng:
                {
                    return $"Time: {time}";
                }            
        }
    }

    public string GetObjectsText(int objects)
    {
        Language language = buttonManager.GetLanguage();

        switch (language)
        {
            default:
            case Language.Rus:
                {
                    return $"Объекты: {objects}";
                }
            case Language.Eng:
                {
                    return $"Objects: {objects}";
                }
        }
    }
}
