using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SwipeHandler : MonoBehaviour
{
    public static SwipeHandler ins;

    public event EventHandler<OnVerticalSwipeArgs> onVerticalSwipe;
    public class OnVerticalSwipeArgs : EventArgs
    {
        public float delta;
    }

    [SerializeField] private List<SwipeReader> subscribers = new List<SwipeReader>();

    public void Setup()
    {
        ins = this;

        foreach(var s in subscribers)
        {
            s.SubscribeToSwipe();
        }

        swipeIsEnabled = true;
    }

    private void Update()
    {
        CheckSwipe();
    }

    public bool swipeIsEnabled { get; private set; }

    public void EnableSwipe()
    {
        swipeIsEnabled = true;
    }

    public void DisableSwipe()
    {
        swipeIsEnabled = false;
    }

    private void CheckSwipe()
    {
        if (!isGui()) return;

        if (!swipeIsEnabled) return;

#if !UNITY_EDITOR
        if (Input.touchCount == 1)
        {
            var touch = Input.touches[0];

            var touchDeltaY = touch.deltaPosition.y;

            onVerticalSwipe?.Invoke(this, new OnVerticalSwipeArgs { delta = touchDeltaY });
        }
#else
        if (Input.GetMouseButton(0))
        {
            var cursorDeltaY = Mouse.current.delta.y;

            //Debug.Log($"cursorDeltaY {cursorDeltaY.ReadValue()}");

            onVerticalSwipe?.Invoke(this, new OnVerticalSwipeArgs { delta = cursorDeltaY.ReadValue() });
        }
#endif
    }

    private bool isGui()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
}
