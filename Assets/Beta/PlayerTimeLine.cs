using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerTimeLine : MonoBehaviour, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] private RectTransform _timeLine;
    [SerializeField] private RectTransform _pin;
    [SerializeField] private Canvas canvas;
    private float _anchoreYPosition;

    private float _timeLineWidth;
    private void Start()
    {
        _timeLineWidth = _timeLine.rect.width;

        

        _anchoreYPosition = _pin.anchoredPosition.y;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        AudioPlayerController.ins.timeLinePinIsLocked = true;
    }

    public void OnDrag(PointerEventData eventData)
    {        
        var pinAnchoredPosition = _pin.anchoredPosition + eventData.delta / canvas.scaleFactor;
        pinAnchoredPosition.y = _anchoreYPosition;

        if (pinAnchoredPosition.x < 0)
        {
            pinAnchoredPosition.x = 0;
        }
        if (pinAnchoredPosition.x > _timeLineWidth) 
        {
            pinAnchoredPosition.x = _timeLineWidth;
        }

        _pin.anchoredPosition = pinAnchoredPosition;      
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        var progress = _pin.anchoredPosition.x / _timeLineWidth;
        AudioPlayerController.ins.SetTimeLine(progress);

        AudioPlayerController.ins.StartUnlockTimeLinePin();
    }

   
}
