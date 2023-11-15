using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TimeLinePinY : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform _timeLine;
    [SerializeField] private RectTransform _pin;
    [SerializeField] private Canvas canvas;
    private float _anchoreXPosition;

    private float _timeLineHeight;
    private void Start()
    {
        _timeLineHeight = _timeLine.rect.height;

        _anchoreXPosition = _pin.anchoredPosition.x;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioPlayerController.ins.timeLinePinIsLocked = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        var pinAnchoredPosition = _pin.anchoredPosition + eventData.delta / canvas.scaleFactor;
        pinAnchoredPosition.x = _anchoreXPosition;

        if (pinAnchoredPosition.y > 0)
        {
            pinAnchoredPosition.y = 0;
        }
        if (pinAnchoredPosition.y < -_timeLineHeight)
        {
            pinAnchoredPosition.y = -_timeLineHeight;
        }

        _pin.anchoredPosition = pinAnchoredPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var progress = Mathf.Abs(_pin.anchoredPosition.y) / _timeLineHeight;
        AudioPlayerController.ins.SetTimeLine(progress);

        AudioPlayerController.ins.StartUnlockTimeLinePin();
    }
}
