using Google.Maps;
using Google.Maps.Examples;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwitchMapZoom : MonoBehaviour
{
    [SerializeField]
    private MapsService mapsService;

    [SerializeField]
    public BaseMapLoader BaseMapLoader;

    public int zoom = 16;

    public void SetZoom()
    {
        //mapsService.ZoomLevel = zoom;
        //mapsService.LoadMap();

        BaseMapLoader. MapsService.ZoomLevel = zoom;
        mapsService.MakeMapLoadRegion()
          .AddViewport(Camera.main, 1000)
          .Load(BaseMapLoader.RenderingStyles);

        BaseMapLoader.ClearMap();
        BaseMapLoader.LoadMap();

        Debug.Log($"map reloaded");
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetZoom();
        }
    }
}
