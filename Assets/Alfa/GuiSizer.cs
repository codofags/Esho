using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GuiSizer : MonoBehaviour
{
    [SerializeField]
    private GameObject ObjectPrefab;

    [SerializeField]
    private GameObject RowPrefab;  

    [SerializeField]
    private float sideOffset = 51.66f;
    [SerializeField]
    private float midOffset = 51.66f;
    [SerializeField]
    private float lineOffset = 51.66f;

    [SerializeField]
    private float imageTextSubLineSize = 28.5f; //расстояние между картинкой и текстом в мини карточке(интересные места рядом)

    [SerializeField]
    private List<PoiConfig> poiConfigs = new List<PoiConfig>();

    void Start()
    {
        var columnSize = (1080 - sideOffset*2 - midOffset) / 2;

        var poiContainerYSize = 0f;
        var poiYposition = 0f;

        List<PoiControllerGUI> guiPois = new List<PoiControllerGUI>();

        var poiCounter = 1;

        foreach (var pc in poiConfigs)
        {
            //создаем объект
            var newPoi = Instantiate(ObjectPrefab, transform);

            var guiPoiController = newPoi.GetComponent<PoiControllerGUI>();
            guiPois.Add(guiPoiController);

            guiPoiController.AssignSize(462.5f);

            guiPoiController.AssignData(pc);

            guiPoiController.Resize();

            if (poiCounter % 2 == 1)
            {
                newPoi.transform.localPosition = new Vector3(sideOffset, poiYposition, 0);
            }

            if (poiCounter % 2 == 0)
            {
                newPoi.transform.localPosition = new Vector3(1080 / 2 + midOffset / 2, poiYposition, 0);

                var prevPoi = guiPois[poiCounter -2];

                var prevYsize = prevPoi.GetComponent<RectTransform>().sizeDelta.y;
                var curYsize = newPoi.GetComponent<RectTransform>().sizeDelta.y;

                if (prevYsize > curYsize) 
                {
                    poiYposition -= prevYsize + lineOffset + imageTextSubLineSize;

                    poiContainerYSize += prevYsize + lineOffset + imageTextSubLineSize;
                }
                else
                {
                    poiYposition -= curYsize + lineOffset + imageTextSubLineSize;

                    poiContainerYSize += curYsize + lineOffset + imageTextSubLineSize;
                }
            }
            poiCounter++;
        }

        var rectTransform = GetComponent<RectTransform>();
        rectTransform.sizeDelta = new Vector2(1080, poiContainerYSize);
    }
}
