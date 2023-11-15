using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HorizontalDrag : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private bool usePagination;

    [SerializeField] private bool useGridLayout;
    [SerializeField] private GridLayoutGroup gridLayout;

    [SerializeField] private Transform imageContainer;
    [SerializeField] private Transform paginationsContainer;
    [SerializeField] private GameObject paginationPrefab;

    private List<PaginatorController> paginations = new List<PaginatorController>();

    private int trackCount;
    [SerializeField] private int cellSizeX = 700;

    //[SerializeField] private bool useTargetOffset;
    //[SerializeField] private float targetOffsetX;
    //private bool isDraged;

    [SerializeField] private float SmoothSpeed = 6;
    private float targetPosition;
    [SerializeField] private Vector3 targetPositionOffset;

    [SerializeField] private TextMeshProUGUI trackName;

    private void Start()
    {
        //Assign();
    }

    public GameObject testmarker;
    public GameObject targetmarker;

    public void Setup()
    {
        //добавляем картинки

        trackCount = 0;

        //определяем кол-во картинок в контейнере
        trackCount = imageContainer.childCount;

        Debug.Log($"trackCount {trackCount}");


        //создаем пагинаторы по кол-ву картинок
        if (usePagination)
        {
            CreatePaginations();
        }

        //if (useGridLayout)
        //{

        //}

        //if(useTargetOffset)
        //{
        //    targetOffsetX = Screen.width / 2;
        //}
        //else
        //{
        //    targetOffsetX = 0;
        //}

        var sizeX = cellSizeX * trackCount + 185 * 2 + (trackCount - 1) * 60;

        imageContainer.GetComponent<RectTransform>().sizeDelta = new Vector2(sizeX, 0);

        for (int i = 0; i < trackCount; i++)
        {
            //var mark = Instantiate(testmarker, transform);
            //mark.transform.position = new Vector3(i * cellSizeX, 0, 0);
        }

        SelectTrack(0);
    }

    private void CreatePaginations()
    {

        for (int i = 0; i < trackCount; i++)
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
    }

    //float dragDelta;

    bool canDrag=true;
    public int currentTrack = 0;
    [SerializeField] private float _dragLimitX = 100;
    public void OnDrag(PointerEventData eventData)
    {
        if (!canDrag) return;
        //Debug.Log($"delta.x {eventData.delta.x}");

        //определяем текущую позицию контейнера
        var currentPosition = imageContainer.transform.localPosition;
        //Debug.Log($"currentPosition.x {currentPosition.x}");

        //смещаем контейнер за пальцем
        currentPosition.x += eventData.delta.x;
        //Debug.Log($"currentPosition.x {currentPosition.x}");
        imageContainer.transform.localPosition = currentPosition;

        //определяем смещение контейнера относительно начала драга
        var offset = currentPosition - _startPosition;
        //var offsetX = Mathf.Abs(offsetPosition.x);
        //Debug.Log($"offset.x {offset.x} {_startPosition}");

        //если сместились сильнее, чем нужно для перелистывания
        if (offset.x > _dragLimitX)
        {
            _startPosition.x = currentPosition.x+ cellSizeX;

            currentTrack = GetTrackIndex();
            currentTrack -= 1;
            SelectTrack(currentTrack);

            canDrag = false;
        }

        if (offset.x < -_dragLimitX)
        {
            _startPosition.x = currentPosition.x - cellSizeX;

            currentTrack = GetTrackIndex();
            currentTrack += 1;
            SelectTrack(currentTrack);

            canDrag = false;
        }
    }

    //IEnumerator ResetDrag()
    //{
    //    yield return new WaitForSeconds(0.2f);

    //    canDrag = true;
    //}

    public void OnEndDrag(PointerEventData eventData)
    {
        //isDraged = false;

        //SelectTrack(GetTrackIndex());

        canDrag = false;
    }

    private int GetTrackIndex()
    {
        var result = Math.Round(imageContainer.transform.localPosition.x) - 185;

        result /= (cellSizeX+60) * -1;   

        return (int)result;
    }

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

    public void SelectTrack(int trackIndex)
    {
        if (trackIndex < 0) { trackIndex = 0; }

        if (trackIndex > trackCount - 1) { trackIndex = trackCount - 1; }

        currentTrack = trackIndex;

        targetPosition = trackIndex * cellSizeX;       

        var config = AudioPlayerController.ins.contentConfigs[trackIndex];
        trackName.text = config.GetCaption();

        canDrag = false;

        EnablePaginator(trackIndex);
    }

    [SerializeField]private bool UseOffset;
    [SerializeField] private float spasingX = 0;

    private void Update()
    {
        if (canDrag) return;

        var currentPosition = imageContainer.localPosition;

        var offset =0f;
        if (UseOffset)
        {
            offset = currentTrack * spasingX;
        }        

        var desirePosition = new Vector3(-targetPosition - offset, 0, 0);

        imageContainer.localPosition = Vector3.Lerp(currentPosition, desirePosition, Time.deltaTime * SmoothSpeed);

        if (Mathf.Abs(imageContainer.localPosition.x - desirePosition.x) < 1) 
        {
            canDrag = true;
        }
        //}
    }

}
