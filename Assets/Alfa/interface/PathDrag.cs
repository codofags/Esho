using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PathDrag : MonoBehaviour, /*IPointerDownHandler,*/ IDropHandler, IBeginDragHandler
{
    public void OnBeginDrag(PointerEventData eventData)
    {
        PathController.ins.canMove = false;
        //Debug.Log("start drag");
    }

    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("drop");
        PathController.ins.OnImageContainerDrop();
    }

    //public void OnPointerDown(PointerEventData eventData)
    //{
    //    PathController.ins.canMove = false;
    //    Debug.Log("pointer down");
    //}
}
