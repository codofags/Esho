using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class GUIBotDrag : MonoBehaviour, IBeginDragHandler, IDragHandler//, IDropHandler
{
    Vector2 StartDragPoint;
    Vector2 DropDragPoint;

    public float OpenOffset = 100f;
    //public float OpenSpeed = 1f;

    bool PlayerIsOpen = false;

    public RectTransform PlayerTransform;
    Vector2 PlayerDeltaSize;

    //Vector2 ScreenSize;

    //private void OnMouseDown()
    //{
    //    GUIController.Inst.SetBotMenuState(MenuState.Idle);
    //}

    public void OnBeginDrag(PointerEventData eventData)
    {
        GUIController.Inst.SetBotMenuState(MenuState.Idle);

        StartDragPoint = eventData.position;

        PlayerDeltaSize = PlayerTransform.sizeDelta;

        //ScreenSize = new Vector2(Screen.width, Screen.height);

        //Debug.Log(ScreenSize);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (GUIController.Inst.BotMenuState != MenuState.Idle) return;

        Vector2 DeltaDrag = eventData.position;

        var DeltaY = (StartDragPoint.y - DeltaDrag.y);

        //Debug.Log(DeltaY);

        Vector2 DragDeltaSize = PlayerDeltaSize;

        DragDeltaSize.y += DeltaY;

        PlayerTransform.sizeDelta = DragDeltaSize;

        //transform.position = eventData.position;

        if (DeltaY > OpenOffset)
        {
            GUIController.Inst.SetBotMenuState(MenuState.Open);
        }
        else
        {
            //GUIController.Inst.SetTopMenuState(MenuState.Close);
        }
    }   

  
}
