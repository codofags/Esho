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
        //�������� ���� ����� ������, ����� �� ���� �� ���������� ����� ��������
        SwipeHandler.ins.DisableSwipe();

        //���������� ������ ������ ����� ������
        beforeDragPosition = transform.position.y;

        _uiTabDrag.DisableAutoScroll();        
    }
    
    private bool isDragable = true;
    public void OnDrag(PointerEventData eventData)
    {
        if (!isDragable || _uiTabDrag.useAutoScroll) return;
        //������� ������ 
        _uiTabDrag.ChangeSize(eventData.delta.y);
        //_uiTabDrag._targetY += eventData.delta.y; 

        //���������� �� ������� ���������� ������
        var offset =  transform.position.y - beforeDragPosition;

        //����������� � �����
        if (offset < 0 && Mathf.Abs(offset) > ButtonManager.ins.dragOffsetLimit) 
        {
            _uiTabDrag.OpenTab();
            isDragable = false;           
        }
        //����������� � ����
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
