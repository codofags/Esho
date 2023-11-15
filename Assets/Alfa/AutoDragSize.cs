using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoDragSize : MonoBehaviour
{
    [SerializeField] private RectTransform MainContainer;
    [SerializeField] private RectTransform DragContainer;

    //[SerializeField] private float MainContainerOffset;

    [SerializeField] private bool usePlayerOffset = true;
    [SerializeField] private float PlayerControllerSizeY = 435f;

    
    [SerializeField] private float Footer = 50;
    //[SerializeField] private float MainMenuSizeY = 277f;

    private void OnEnable()
    {
        StartCoroutine(SetupSize());
    }

    IEnumerator SetupSize()
    {
        Canvas.ForceUpdateCanvases();

        //yield return new WaitForSeconds(0.2f);
        yield return new WaitForEndOfFrame();


        GetComponent<RectTransform>().sizeDelta = new Vector2(0, 0);

        var minY = 9999f;
        GameObject lowest = null;

        //chewck all recttransforms in childs
        {
            var childRectTransforms = transform.GetComponentsInChildren<RectTransform>();

            foreach (var rt in childRectTransforms)
            {
                var minimumY = rt.anchoredPosition.y + rt.rect.size.y;

                //Debug.Log($"rt.anchoredPosition.y => {rt.anchoredPosition.y}");
                //Debug.Log($"rt.rect.size.y => {rt.rect.size.y}");
                //Debug.Log($"minimumY => {minimumY}");

                //if (rt.anchoredPosition.y - rt.rect.size.y < minY)
                //{
                //    minY = rt.anchoredPosition.y - rt.rect.size.y;

                //    lowest = rt.gameObject;
                //}

                if (rt.localPosition.y - rt.rect.size.y < minY)
                {
                    minY = rt.localPosition.y - rt.rect.size.y;

                    lowest = rt.gameObject;
                }
            }
        }

        //childcount
        {
            //var childCount = transform.childCount;
            //lowest = transform.GetChild(childCount - 1).gameObject;

            //var lowestRectTransform = lowest.GetComponent<RectTransform>();

            //minY = lowestRectTransform.anchoredPosition.y - lowestRectTransform.rect.size.y;
        }

        //Debug.Log($"minY => {minY} ************** lowest => {lowest.name}");

        var mainContainerOffset = Mathf.Abs(transform.localPosition.y) + Footer;
        if (usePlayerOffset) { mainContainerOffset += PlayerControllerSizeY; }

        MainContainer.sizeDelta = new Vector2(0, Mathf.Abs(minY) + mainContainerOffset);

        var dragContainerOffset = PlayerControllerSizeY + Footer;
        DragContainer.sizeDelta = new Vector2(0, Mathf.Abs(minY) + dragContainerOffset);
    }
}
