using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwipeReader : MonoBehaviour
{
    [SerializeField] private bool useLimit;
    [SerializeField] private int minY;
    [SerializeField] private int maxY;

    private void OnVerticalSwipe(object sender, SwipeHandler.OnVerticalSwipeArgs e)
    {
        if (!gameObject.activeInHierarchy)
        {
            //Debug.Log($"not act");
            return;
        }

        var rectTransform = GetComponent<RectTransform>();

        var desirePosition = rectTransform.anchoredPosition;
        desirePosition.y += e.delta;

        if (useLimit)
        {
            if (desirePosition.y > maxY) desirePosition.y = maxY;
            if (desirePosition.y < minY) desirePosition.y = minY;
        }

        rectTransform.anchoredPosition = desirePosition;
    }

    public void SubscribeToSwipe()
    {
        SwipeHandler.ins.onVerticalSwipe += OnVerticalSwipe;
    }

    public void UnSubscribeToSwipe()
    {
        SwipeHandler.ins.onVerticalSwipe -= OnVerticalSwipe;
    }
}
