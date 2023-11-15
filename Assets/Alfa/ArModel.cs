using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArModel : MonoBehaviour
{
    public List<ScenarioController> arModelPoints = new List<ScenarioController>();  

    public List<GameObject> modelState;

    [SerializeField] private GameObject localPosition;
    [SerializeField] private GameObject localRotation;

    [SerializeField] private Transform pinsContainer;

    #region Model

    public void SetPosition(Vector3 position)
    {
        localPosition.transform.localPosition = new Vector3(position.x, -position.z, -position.y);
    }

    public void SetRotation(Vector3 rotation)
    {
        localRotation.transform.localRotation = Quaternion.Euler(rotation);
    }

    private void HideModel()
    {
        foreach(var s in modelState)
        {
            if(s != null)
            {
                s.SetActive(false);
            }
        }
    }

    public void SetModelState(int stateIndex)
    {
        HideModel();

        modelState[stateIndex].SetActive(true);
    }

    #endregion

    #region Points

    public void ResetPoints()
    {
        foreach (var p in arModelPoints)
        {
            p.ResetPoint();
        }
    }

    internal void SelectPoint(ScenarioController arModelPoint)
    {
        ResetPoints();

        arModelPoint.Activate();

        SetModelState(arModelPoint.GetArScenario().modelState);

        ShowButtonPointDetails(arModelPoint);

        PlayScenarioTrack(arModelPoint);
    }

    private void PlayScenarioTrack(ScenarioController arModelPoint)
    {
        var scenarioIndex = arModelPoints.IndexOf(arModelPoint);
        Debug.Log(scenarioIndex);

        var scenarioTrack = AudioPlayerController.ins.trackControllers[scenarioIndex];
        Debug.Log(scenarioTrack);

        AudioPlayerController.ins.PlayTrack(scenarioTrack);
    }

    public void ShowButtonPointDetails(ScenarioController arModelPoint)
    {
        ButtonManager.ins.ShowArPointDetailsButton(arModelPoint);
    }

    #endregion

    public void AddPin(GameObject pin, Vector3 position)
    {
        pin.transform.SetParent(pinsContainer);
        pin.transform.localPosition = position;        
    }

    private bool isOpen;
    public void SwitchView()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            modelState[0].SetActive(true);
            modelState[1].SetActive(false);
        }
        else
        {
            modelState[0].SetActive(false);
            modelState[1].SetActive(true);
        }
    }

    [SerializeField] private GameObject _pin;
    internal void DisablePin()
    {
        _pin.gameObject.SetActive(false);
    }

    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;
    [SerializeField] private Transform _zoomContainer;
    [SerializeField] private float _editorZoomSpeed = 1f;
    [SerializeField] private float _mobileZoomSpeed = 0.2f;
    internal void ScaleModel(float scaleValue)
    {
        var currentScale = _zoomContainer.localScale.x;

        float desireScale = 0f;

#if UNITY_EDITOR
        desireScale = currentScale + (scaleValue * _editorZoomSpeed * Time.deltaTime);
#else
        desireScale = currentScale + (scaleValue * _mobileZoomSpeed * Time.deltaTime);
#endif

        if (desireScale < _minZoom)
        {
            desireScale = _minZoom;
        }

        if (desireScale > _maxZoom)
        {
            desireScale = _maxZoom;
        }

        _zoomContainer.localScale = Vector3.one * desireScale;
    }

    public float GetModelScale()
    {
        return _zoomContainer.localScale.x;
    }
}
