using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class DragPinController : MonoBehaviour, /*IPointerDownHandler,*/ IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private UiTabDrag _uiTabDrag;

    private float beforeDragPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        //включаем флаг драга плашки, чтобы на него не реагировал свайп контента
        SwipeHandler.ins.DisableSwipe();

        //запоминаем высоту плашки перед драгом
        beforeDragPosition = transform.position.y;

        _uiTabDrag.DisableAutoScroll();        
    }
    
    private bool isDragable = true;
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragable || _uiTabDrag.useAutoScroll) return;
        //смещаем плашку 
        _uiTabDrag.ChangeSize(eventData.delta.y);
        //_uiTabDrag._targetY += eventData.delta.y; 

        //определяем на сколько сместилась плашка
        var offset =  transform.position.y - beforeDragPosition;

        //приклеиваем к верху
        if (offset < 0 && Mathf.Abs(offset) > ButtonManager.ins.dragOffsetLimit) 
        {
            _uiTabDrag.OpenTab();
            isDragable = false;           
        }
        //приклеиваем к низу
        if (offset > 0 && Mathf.Abs(offset) > ButtonManager.ins.dragOffsetLimit) 
        {
            _uiTabDrag.CloseTab();
            isDragable = false;
        }

        //_uiTabDrag.DragTab();
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        SwipeHandler.ins.EnableSwipe();

        isDragable = true;

        var offset = transform.position.y - beforeDragPosition;

        if(Mathf.Abs(offset) < ButtonManager.ins.dragOffsetLimit)
        {
            _uiTabDrag.ResetPosition();
        }
    }
}
