using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiTabDrag : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //определяем размер окна скрола
        CalculateTabSize();

        _targetY = _maxY;

        useAutoScroll = true;
    }

    [SerializeField] private float _minY;
    [SerializeField] private float _maxY;
    //[SerializeField] private float _animationSpeed;
    private void CalculateTabSize()
    {

    }

    private bool _tabIsOpen=true;
    public float _targetY;
    public void DragTab()
    {
        _tabIsOpen = !_tabIsOpen;
        if (_tabIsOpen)
        {
            _targetY = _maxY;
        }
        else
        {
            _targetY = _minY;
        }
    }

    public RectTransform _tabRect;

    public bool useAutoScroll { get; private set; }

    internal void ChangeSize(float y)
    {
        var currentY = _tabRect.offsetMax.y;

        currentY += y;

        if (currentY > _minY) currentY = _minY;
        if (currentY < _maxY) currentY = _maxY;

        _tabRect.offsetMax = new Vector2(0, currentY);
    }

    public void EnableAutoScroll()
    {
        useAutoScroll = true;
    }

    public void DisableAutoScroll()
    {
        useAutoScroll = false;
    }

    private void Update()
    {
        if (useAutoScroll )
        {
            var topY = _tabRect.offsetMax.y;

            var desireY = Mathf.Lerp(topY, _targetY, ButtonManager.ins.tabDragSpeed * Time.deltaTime);

            _tabRect.offsetMax = new Vector2(0, desireY);   
            
            if(Mathf.Abs( desireY- _targetY) < 1)
            {
                Debug.Log($"stop animation");
                useAutoScroll = false;
            }
        }
    }

    public void OpenTab()
    {
        _targetY = _maxY;
        _tabIsOpen = true;

        EnableAutoScroll();
    }

    public void CloseTab()
    {
        _targetY = _minY;
        _tabIsOpen = false;

        EnableAutoScroll();
    }

    public void ResetPosition()
    {
        if (_tabIsOpen)
        {
            OpenTab();
        }
        else
        {
            CloseTab();
        }
    }
}
