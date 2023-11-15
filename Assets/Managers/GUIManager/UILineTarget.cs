using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UILineTarget : MonoBehaviour
{
    public Image _Marker;
    public Transform _Target;

    public Camera _UserCamera;

    public UILine _UILine;

    private void Update()
    {
        if (_Target != null)
        {
            _Marker.transform.position = _UserCamera.WorldToScreenPoint(_Target.position);
        }        
    }
}
