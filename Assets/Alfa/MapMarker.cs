using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapMarker : MonoBehaviour
{
    //public GPSPoint gPSPoint;
    //public PointConfig point;
    public GPS_Point_Config point;

    public Image pointImage;

    [SerializeField]
    private MapMarkerGroup selfGroup;  

    [SerializeField]
    private GameObject UI_MarkerIco;

    [SerializeField]
    private GameObject UI_MarkerBackGround;

    public Sprite markerIcoSprite;

    public List<MapMarker> markersInTrigger = new List<MapMarker>();

    public int markerIcoPrimaryFactor = 1;

    private void OnTriggerEnter(Collider other)
    {   
        //получаем компонент маркера у объекта с которым столкнулись
        var otherMapMarker = other.gameObject.GetComponent<MapMarker>();

        //если такого компонента нет, то ничего не делаем
        if (otherMapMarker == null) return;  

        //запоминаем игровой объект чужого маркера
        markersInTrigger.Add(otherMapMarker);

        //ищем чужую группу 
        var otherGroup = otherMapMarker.GetMapMarkerGroup();

        //если группы совпали, то ничего не делаем
        if (selfGroup != null && selfGroup == otherGroup) return;

        //если оба маркера без группы, создаем новую группу и добавляем себя в нее
        if (selfGroup == null && otherGroup == null)
        {
            //создаем новую группу
            selfGroup = CreateMapMarkerGroup();

            //добавляем в новую группу свой маркер          
            selfGroup.AddMarker(this);
        }

        //если оба маркера в разных группах // слияние групп
        else if (selfGroup != null && otherGroup != null)
        {
            //переносим все маркеры из чужой группы в собственную
            var otherMapMarkers = otherGroup.GetMapMarkers();

            //сначала удаляем все маркеры и второй группы
            otherGroup.RemoveMarkers(otherMapMarkers);

            //потом добавляем все маркеры из второй группы в свою
            selfGroup.AddMarkers(otherMapMarkers);
        }

        //если нет собственной группы и есть чужая
        else if (selfGroup == null && otherGroup != null)
        {
            otherGroup.AddMarker(this);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var otherMapMarker = other.gameObject.GetComponent<MapMarker>();

        if (otherMapMarker == null) return;

        //Debug.Log($"marker exit => ");

        //если из нашего маркера вышел другой маркер / забываем его игровой объект
        markersInTrigger.Remove(otherMapMarker);
        
        //проверяем сколько игровых объектов контактирует с этим игровым объектом
        var selfNeighboursCount = markersInTrigger.Count;   

        //если у этого маркера не осталось соседей
        if (selfNeighboursCount == 0)
        {
            //удаляем себя из группы
            selfGroup.RemoveMarker(this); 
        }

        //нужжно понять на сколько подгрупп развалилась группа

        //если соседей больше 0, то это значит, что мы все еще можем контактировать с кем-то из старой группы
        if(selfNeighboursCount > 0)
        {
            //получаем всю цепочку маркеров из соседей
            List<MapMarker> childMarkers = new List<MapMarker>();
            childMarkers.Add(this);
            GetChilds(childMarkers, this);

            //если в эту цепочку входит маркер, который от нас отключился, то мы все еще в старой группе
            if (childMarkers.Contains(otherMapMarker))
            {
                //Debug.Log($"all markers in chain");
                return;
            }

            //удаляем все маркеры и старой группы
            selfGroup.RemoveMarkers(childMarkers);

            //создаем новую группу для этого маркера
            selfGroup = CreateMapMarkerGroup();

            //переносим все маркеры из нашей цепочки в новую группу
            selfGroup.AddMarkers(childMarkers);

            //foreach (var cm in childMarkers)
            //{
            //    selfGroup.AddMarker(cm);                
            //}

            //var summaryPosition = Vector3.zero;

            //foreach (var m in selfGroup.GetMapMarkers())
            //{
            //    //m.HideSelfMarker();


            //}

            //selfGroup.GetMapMarkers()[0].ShowSelfMarker();           
        }
      
    }

    private List<MapMarker> GetChilds(List<MapMarker> mapMarkers, MapMarker mapMarker)
    {
        var markerNeighbours = mapMarker.markersInTrigger;

        foreach(var n in markerNeighbours)
        {
            if (!mapMarkers.Contains(n))
            {
                mapMarkers.Add(n);

                GetChilds(mapMarkers, n);
            }
        }

        return mapMarkers;
    }

    private MapMarkerGroup CreateMapMarkerGroup()
    {
        var newMarkerGroup = Instantiate(MapManager.Inst.markerGroupControllerPrefab, MapManager.Inst.markersContainer);

        newMarkerGroup.transform.localRotation = Quaternion.identity;

        var result = newMarkerGroup.GetComponent<MapMarkerGroup>();

        MapManager.Inst.AddMarkerGroup(result);

        return result;
    }

    public void SetMapMarkerGroup(MapMarkerGroup mapMarkerGroup)
    {
        selfGroup = mapMarkerGroup;
    }

    public MapMarkerGroup GetMapMarkerGroup()
    {
        return selfGroup;
    }

    public void RemoveMapMarkerGroup()
    {
        selfGroup = null;
    }    

    /// <summary>
    /// /
    /// </summary>
    public void HideMarker()
    {
        HideIco();
        //HideMakerCounter();
    }

    public void HideIco()
    {        
        UI_MarkerIco.SetActive(false);
        UI_MarkerBackGround.SetActive(false);
    }   

    /// <summary>
    /// /
    /// </summary>
    private void ShowMarker()
    {
        ShowIco();
        //ShowMakerCounter();
    }

    public void ShowIco()
    {
        UI_MarkerIco.SetActive(true);
        UI_MarkerBackGround.SetActive(true);
    }

    //public void SelectMarker()
    //{
    //    Debug.Log("marker click");

    //    if (isGui())
    //    {
    //        return;
    //    }

    //    //ButtonManager.ins.OpenPointDescription();
    //}

    private void OnMouseDown()
    {
        Debug.Log("marker click");

        if (isGui())
        {
            return;
        }

        if (Input.touchCount>=2)
        {
            return;
        }

        var playAudio = AutoGPSPlay.ins.useAutoGid;

        AudioPlayerController.ins.SetupPlayList(point, playAudio);

        Ui_GPS_Point_Menu.ins.OpenPoint(point);
    }

    private bool isGui()
    {
        
            PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
            eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
            return results.Count > 0;
        

        //return EventSystem.current.IsPointerOverGameObject();
    }
}
