using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public enum MapCameraState
{
    Follow,
    Static,
}

//namespace Google.Maps.Examples.Shared
//{
/// <summary>
/// A simple camera controller, to allow for user-controlled movement in example scenes.
/// </summary>
/// <remarks>
/// Intended to be attached to the <see cref="Camera"/> <see cref="GameObject"/> being controlled.
/// <para>
/// Movement is performed via WASD keys (transverse), QE (up/down) and arrow keys (rotation).
/// </para></remarks>
[RequireComponent(typeof(Camera))]
public class CameraController : MonoBehaviour
{
    /// <summary>
    /// Event triggered when the <see cref="Camera"/> moves, passing back the amount moved.
    /// </summary>
    [Serializable]
    public class MoveEvent : UnityEvent<Vector3> { }

    [Tooltip("Movement speed when pressing movement keys (WASD for panning, QE for up/down).")]
    public float MovementSpeed = 200f;

    [Tooltip("Rotation speed when pressing arrow keys.")]
    public float RotationSpeed = 100f;

    [Tooltip("Minimum height off the ground.")]
    public float MinHeight = 2f;

    [Tooltip("Maximum height off the ground.")]
    public float MaxHeight = 600f;

    [Tooltip("Minimum angle above ground.")]
    public float MinXRotation = 0;

    [Tooltip("Maximum angle above ground.")]
    public float MaxXRotation = 90;

    /// <summary>
    /// Optional <see cref="Action"/> called whenever the <see cref="Camera"/> is moved in any way.
    /// </summary>
    /// <remarks>
    /// Passes in the amount moved so the type/direction of movement can be queried.
    /// </remarks>
    public MoveEvent OnMove = new MoveEvent();

    /// <summary>
    /// Optional <see cref="Action"/> called whenever the <see cref="Camera"/> is rotated.
    /// </summary>
    public UnityEvent OnRotate = new UnityEvent();

    /// <summary>
    /// Optional <see cref="Action"/> called whenever the <see cref="Camera"/> is moved or rotated
    /// in any way.
    /// </summary>
    public UnityEvent OnTransform = new UnityEvent();

    /// <summary>
    /// The current desired rotation of the Camera around the Y-Axis. Applied in world space after
    /// Inclination is applied.
    /// </summary>
    private float Azimuth;

    /// <summary>
    /// The current desired rotation of the Camera around the X-Axis. Applied in world space before
    /// Azimuth is applied.
    /// </summary>
    private float Inclination;

    public void InitializeAzimuthAndInclination()
    {
        // Initialize Azimuth and Inclination from the current rotation Euler angles. Reading Euler
        // angles is generally not a good idea but should be safe to do once at initialization if the
        // Camera starts in a non-extreme orientation.
        Azimuth = transform.eulerAngles.y;
        Inclination = transform.eulerAngles.x;
    }

    private MapCameraState cameraState = MapCameraState.Static;
    private void Awake()
    {
        InitializeAzimuthAndInclination();
    }

    private void Update()
    {
        //if (!isGui())
        //{
           
        //}

        MoveCamera();

        CameraSmoothMove();
    }

    [SerializeField] private GameObject cameraContainer;

    [SerializeField] private float Last_Click_Time;
    [SerializeField] private float Editor_Moove_Factor = 1;
    [SerializeField] private float mobileMooveFactor = 14;

    private Vector3 _desireCameraPosition;

    private void MoveCamera()
    {
        if (Input.touchCount == 1)
        {
            var delta = Input.touches[0].deltaPosition;
            if (delta.x > 20 || delta.y > 20)
            {
                cameraState = MapCameraState.Static;
            }
        }

        switch (cameraState)
        {
            case MapCameraState.Follow:
                {
                    GetUserPosition();
                }
                break;
            case MapCameraState.Static:
                {
#if UNITY_EDITOR
                    Editor_Moove_Map();
#else
                    Android_Moove_Map();
#endif
                }
                break;
        }

        
    }

    private void Editor_Moove_Map()
    {
        if (isGui()) return;

        if (Input.GetMouseButtonDown(0))
        {
            Last_Click_Time = Time.time;
        }

        if (Input.GetMouseButton(0) && SwipeHandler.ins.swipeIsEnabled)
        {
            //cameraState = MapCameraState.Static;

            Vector3 Camera_Offset = Vector3.zero;

            var X = Input.GetAxis("Mouse X");
            var Z = Input.GetAxis("Mouse Y");

            var cameraY = transform.position.y;

            Vector3 Desire_Position_X = -cameraContainer.transform.right * X * Editor_Moove_Factor * cameraY;
            Vector3 Desire_Position_Z = -cameraContainer.transform.forward * Z * Editor_Moove_Factor * cameraY;

            Camera_Offset = Desire_Position_X + Desire_Position_Z;

            _desireCameraPosition = cameraContainer.transform.position + Camera_Offset;
            //Vector3 New_Camera_Position = cameraContainer.transform.position + Camera_Offset;
            //cameraContainer.transform.position = New_Camera_Position;
        }
    }

    private void Android_Moove_Map()
    {
        if (isGui()) return;

        if (Input.touchCount == 1 && SwipeHandler.ins.swipeIsEnabled)
        {
            //cameraState = MapCameraState.Static;

            Vector3 Camera_Offset = Vector3.zero;

            Vector2 Delta = Input.touches[0].deltaPosition;

            var cameraY = transform.position.y;

            Vector3 Desire_Position_X = -cameraContainer.transform.right * Delta.x * mobileMooveFactor * cameraY;
            Vector3 Desire_Position_Z = -cameraContainer.transform.forward * Delta.y * mobileMooveFactor * cameraY; 

            Camera_Offset = Desire_Position_X + Desire_Position_Z;

            _desireCameraPosition = cameraContainer.transform.position + Camera_Offset;
            //Vector3 New_Camera_Position = cameraContainer.transform.position + Camera_Offset;
            //cameraContainer.transform.position = New_Camera_Position;
        }
    }

    private bool isGui()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }

    [SerializeField] private GameObject mapUserMarker;

    [SerializeField]
    private Vector3 _mapCameraOffset = new Vector3(0, 0, -70);

    public void EnableFollowUser()
    {
        cameraState = MapCameraState.Follow;
    }

    private void GetUserPosition()
    {      
        _desireCameraPosition = mapUserMarker.transform.position + _mapCameraOffset;
    }

    [SerializeField] private float _smoothSpeed=1;
    private void CameraSmoothMove()
    {
        //var currentPosition = cameraContainer.transform.position;
        //cameraContainer.transform.position = Vector3.Lerp(currentPosition, _desireCameraPosition, Time.deltaTime * _smoothSpeed);
        cameraContainer.transform.position = _desireCameraPosition;
    }
}
//}
