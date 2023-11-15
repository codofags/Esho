using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragItem : MonoBehaviour, IDragHandler
{
    [SerializeField] private Vector2 minimumPosition;
    [SerializeField] private Vector2 maximumPosition;

    public bool useLimits = true;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        //var lastElement = transform
    }

    public void OnDrag(PointerEventData eventData)
    {
        //eventData.

        var position = transform.position;
        position.y += eventData.delta.y;

        //Debug.Log(position.y);

        if (useLimits)
        {
            if (position.y < minimumPosition.y)
            {
                position.y = minimumPosition.y;
            }

            if (position.y > maximumPosition.y)
            {
                position.y = maximumPosition.y;
            }
        }     

        transform.position = position;
    }    
   
}
