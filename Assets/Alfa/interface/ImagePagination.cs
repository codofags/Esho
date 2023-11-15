using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImagePagination : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private bool usePagination;

    [SerializeField] private bool useGridLayout;
    [SerializeField] private GridLayoutGroup gridLayout;

    [SerializeField] private Transform imageContainer;
    [SerializeField] private Transform paginationsContainer;
    [SerializeField] private GameObject paginationPrefab;

    private List<PaginatorController> paginations = new List<PaginatorController>();

    private int imageCount;
    [SerializeField] public float cellSizeX= 1080;

    //[SerializeField] private bool useTargetOffset;
    [SerializeField] private float targetOffsetX;
    private bool isDraged;

    [SerializeField] private float SmoothSpeed = 6;
    private float targetPosition;
    [SerializeField] private Vector3 targetPositionOffset ;

    [SerializeField] private bool useStartSetup=false;

    private void Start()
    {
        if (useStartSetup)
        {
            Setup();
        }
        //
    }

    public GameObject testmarker;
    public GameObject targetmarker;

    public void Setup()
    {
        //добавляем картинки

        //определяем кол-во картинок в контейнере
        imageCount = imageContainer.childCount;

        //создаем пагинаторы по кол-ву картинок
        if (usePagination)
        {
            CreatePaginations();
        }

        if (useGridLayout)
        {
            cellSizeX = gridLayout.cellSize.x;
        }

        imageContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(cellSizeX * (imageCount+1), 0);

        if (imageCount > 0)
        {
            Select(0);
        }

        currentTrack = 0;
        _startPosition = Vector3.zero;
        imageContainer.transform.localPosition = new Vector3(0, targetPositionOffset.y, 0);
    }

    private void ResetPaginators()
    {
        var childCount = paginationsContainer.childCount;

        for (int i = childCount - 1; i >= 0; i--)
        {
            var child = paginationsContainer.GetChild(i).gameObject;

            ContentManager.inst.MoveToTrash(child);            
        }
        paginations.Clear();
    }

    private void CreatePaginations()
    {
        ResetPaginators();

        for (int i = 0; i < imageCount; i++)
        {
            var newPagination_GO = Instantiate(paginationPrefab, paginationsContainer);
            var newPagination_SC = newPagination_GO.GetComponent<PaginatorController>();
            paginations.Add(newPagination_SC);

            newPagination_SC.Disable();
        }
    }

   

    private Vector3 _startPosition;
    public void OnBeginDrag(PointerEventData eventData)
    {
        _startPosition = imageContainer.transform.localPosition;
        canDrag = true;
    }

    private bool canDrag = true;
    [SerializeField] private float _dragLimitX = 100;
    private int currentTrack = 0;
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;

        //определяем текущую позицию контейнера
        var currentPosition = imageContainer.transform.localPosition;

        //смещаем контейнер за пальцем
        if(currentTrack == 0 && eventData.delta.x > 0 || currentTrack >= imageCount-1 && eventData.delta.x < 0)
        {

        }
        else
        {
            currentPosition.x += eventData.delta.x;
            imageContainer.transform.localPosition = currentPosition;
        }      

        //определяем смещение контейнера относительно начала драга
        var offset = currentPosition - _startPosition;
        //Debug.Log($"offset {offset.x} => {_startPosition.x}");

        //если сместились сильнее, чем нужно для перелистывания
        if (offset.x > _dragLimitX)
        {
            currentTrack = GetIndex();
            //Debug.Log($"=> {currentTrack }");
            //currentTrack -= 1;
            //Debug.Log($"=> {currentTrack }");
            //_startPosition.x = currentPosition.x + cellSizeX;
            _startPosition.x = -currentTrack * cellSizeX;

            Select(currentTrack);

            canDrag = false;
        }

        if (offset.x < -_dragLimitX)
        {
            currentTrack = GetIndex();
            //Debug.Log($"<= {currentTrack }");
            currentTrack += 1;
            //Debug.Log($"<= {currentTrack }");
            //_startPosition.x = currentPosition.x - cellSizeX;
            _startPosition.x = -currentTrack * cellSizeX;

            Select(currentTrack);

            canDrag = false;
        }

        if (!canDrag) return;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canDrag = false;
    }

    //private int GetImageIndex()
    //{
    //    var result = 0f;

    //    if (useGridLayout)
    //    {
    //        result = (imageContainer.transform.position.x - targetOffsetX) / cellSizeX;
    //    }
    //    else
    //    {
    //        result= (imageContainer.transform.localPosition.x - targetOffsetX) / cellSizeX;
    //    }

    //   // Debug.Log((int)result);

    //    return (int)result;
    //}

    private void DisablePaginations()
    {
        if (!usePagination) return;

        foreach (var p in paginations)
        {
            p.Disable();
        }
    }

    private void EnablePaginator(int index)
    {
        if (!usePagination) return;

        DisablePaginations();

        paginations[Mathf.Abs(index)].Enable();
    }

    //private void SelectImage(int imageIndex)
    //{
    //    if (imageIndex > 0) imageIndex = 0;
    //    if (imageIndex < -(imageCount - 1)) imageIndex = -(imageCount - 1);

    //    targetPosition = imageIndex * cellSizeX;

    //    //var desirePosition = new Vector3(targetPosition + targetPositionOffset.x, targetPositionOffset.y, 0);   

    //    EnablePaginator(imageIndex);
    //}

    private void Select(int index)
    {
        if (index < 0) { index = 0; }

        if (index > imageCount - 1) { index = imageCount - 1; }

        targetPosition = index * cellSizeX;
        Debug.Log($"{index} targetX {targetPosition}");
        EnablePaginator(index);

        canMoove = true;
    }

    [SerializeField] private float DragPeriod = 0.1f;

    IEnumerator ResetDrag()
    {
        yield return new WaitForSeconds(DragPeriod);

        canDrag = true;
    }

    private int GetIndex()
    {
        var result = Math.Round(imageContainer.transform.localPosition.x);

        result /= -cellSizeX;

        return (int)result;       
    }

    [SerializeField] float mooveTrashHold = 1;
    [SerializeField] bool canMoove = false;

    private void Update()
    {
        if (canDrag) return;
        //if (canMoove)
        {
            var currentPosition = imageContainer.localPosition;
            var desirePosition = new Vector3(-targetPosition + targetPositionOffset.x, targetPositionOffset.y, 0);
            imageContainer.localPosition = Vector3.Lerp(currentPosition, desirePosition, Time.deltaTime * SmoothSpeed);

            //if (Mathf.Abs(currentPosition.x - desirePosition.x) < mooveTrashHold) 
            //{
            //    canMoove = false;
            //}

            if (Mathf.Abs(imageContainer.localPosition.x - desirePosition.x) < 1)
            {
                canDrag = true;
            }
        }
    }

}
