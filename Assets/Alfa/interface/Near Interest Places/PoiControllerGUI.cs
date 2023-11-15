using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class PoiControllerGUI : MonoBehaviour
{
    [SerializeField]
    private RectTransform imageContainer;

    [SerializeField]
    private RectTransform textContainer;

    [SerializeField]
    private Image ico;

    [SerializeField]
    private TextMeshProUGUI textMesh;

    [SerializeField] private Button buttonPOI;

    public void AssignSize(float value)
    {
        imageContainer.sizeDelta = new Vector2(462.5f, 408);
        textContainer.sizeDelta = new Vector2(462.5f, 200);
    }

    internal void AssignText(string v)
    {
        var charCount = Random.Range(10, 100);


        for (int i = 0; i < charCount; i++)
        {
            var randomChar = Random.Range(0, 5);

            switch (randomChar)
            {
                case 0: v += "a"; break;
                case 1: v += "b"; break;
                case 2: v += "c"; break;
                case 3: v += "d"; break;
                case 4: v += " "; break;
            }
        }

        textMesh.text = v;

        Canvas.ForceUpdateCanvases();
    }

    public void Resize()
    {
        var ySize = imageContainer.sizeDelta.y;
        ySize += textContainer.sizeDelta.y;

        GetComponent<RectTransform>().sizeDelta = new Vector2(462.5f, ySize);
    }

    internal void AssignData(PoiConfig pc)
    {
        ico.sprite = pc.poiSprite;
        textMesh.text = pc.poiName;

        Canvas.ForceUpdateCanvases();

        //buttonPOI.onClick.AddListener(() => ButtonManager.ins.OpenPointDescription());
    }
}
