using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[Serializable]
public class TextTranslationUI : MonoBehaviour
{
    [SerializeField] private TextTranslation translation;

    [SerializeField] private TextMeshProUGUI textComponent;

    private void Start()
    {
        translation. SubscribeToSwitchLanguage();

        translation.SetTextComponent(textComponent);

        translation. Setup();
    }
}
