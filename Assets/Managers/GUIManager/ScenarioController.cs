using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioController : MonoBehaviour
{
    private ArScenario arScenario;

    [SerializeField] private GameObject locked;
    [SerializeField] private GameObject unlocked;

    [SerializeField] private GameObject canvas;
    [SerializeField] private RectTransform image;

    private Vector2 defaultSize = new Vector2(218, 0);
    [SerializeField] private Vector2 maximumSize = new Vector2(600, 0);

    [SerializeField] private string targetCameraName = "AR Camera";
    private GameObject targetCamera;


    public ArModel arModelController;

    [SerializeField] private float ScaleFactor = 0.1f;

    [SerializeField] private float scaleMinimumTrashhold = 0.0001f;

    [SerializeField] private GameObject pointText;

    public int modelState;

    internal void AssigScenario(ArScenario arScenario)
    {
        this.arScenario = arScenario;
    }

    public void ResetPoint()
    {
        locked.SetActive(true);
        unlocked.SetActive(false);
    }

    public void Select()
    {
        arModelController.SelectPoint(this);
    }

    public void Activate()
    {
        locked.SetActive(false);
        unlocked.SetActive(true);
    }

    private void Update()
    {
        if (targetCamera == null)
        {
            targetCamera = GameObject.Find(targetCameraName);
        }
        else
        {
            var distance = Vector3.Distance(transform.position, targetCamera.transform.position);

            transform.rotation = targetCamera.transform.rotation;

            if (distance == 0)
            {
                distance = 0.1f;
            }

            var targetScale = Vector3.one * distance * ScaleFactor;

            transform.localScale = targetScale;
        }
    }

    [SerializeField] private ArModel _arModel;

    private void OnMouseDown()
    {
        //Select();
        _arModel.SwitchView();
    }

    public ArScenario GetArScenario()
    {
        return arScenario;
    }

    
    public void SetArScenario(ArScenario arScenario)
    {
        this.arScenario = arScenario;
    }
}
