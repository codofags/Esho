using Google.Maps;
using Google.Maps.Coord;
using Google.Maps.Examples;
using Google.Maps.Examples.Shared;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class SetGps : MonoBehaviour
{
    [SerializeField]
    

    private GameObject TestMarker;

    private void Start()
    {
        
    }

    void OnFloatingOriginUpdated(Vector3 offset)
    {
        //MoveNames(offset);
    }

    private void MoveNames(Vector3 offset)
    {
        if (TestMarker != null) TestMarker.transform.position += offset;       
    }

   

    private bool IsMarkerCreated = false;

    public void FixedUpdate()
    {
     

       

        //if (mapsService.Projection.IsFloatingOriginSet && !IsMarkerCreated)
        //{
        //    IsMarkerCreated = true;

        //    var TestMarkerPosition = mapsService.Projection.FromLatLngToVector3(markerCoord);

        //    var TestMarker = GameObject.CreatePrimitive(PrimitiveType.Sphere);

        //    TestMarker.name = "my house";

        //    TestMarker.transform.position = TestMarkerPosition;
        //}
       
    }


    private void OnApplicationQuit()
    {
        Input.location.Stop();
    }
}
