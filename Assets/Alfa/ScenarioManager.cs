using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioManager : MonoBehaviour
{
    public static ScenarioManager Inst;

    public List<ScenarioGuiController> ScenarioGUIs = new List<ScenarioGuiController>();

    public GameObject ScenarioObject;
    //public List<Scenario> AllScenarious = new List<Scenario>();

    public GameObject ScenarioGUIPrefab;
    public Transform ScenarioGUIsContainer;

    //public GameObject ScenarioGameObject;
    //public List<Scenario> ScenariosList = new List<Scenario>();

    //public GameObject ScenarioControllerPrefab;
    //public GameObject ScenarioSeparatorPrefab;
    //public Transform ScenarioControllersContainer;

    //public Transform ScenarioContainer;
    //public Scenario CurrentScenario;

    //public GameObject ButtonPlayScenario;
    //public Text ScenarioNameText;

    //public float UserScenarioRotateFactor = 1f;

    //public float SmoothActionMovement = 1f;
    //public Vector3 ScenarioPosition;
    //public Vector3 ScenarioRotation;

    //public LayerMask ScenarioLayer;
    //public LayerMask DefaultLayer;

    //public UILine _UILine;
    //public UILineTarget _UILineTarget;

    //public ScenarioCard _ScenarioCard;

    private void Start()
    {
        Setup();
    }

    public void Setup()
    {
        Inst = this;
        SetupScenarios();
    }

    public void SetupScenarios()
    {
        //var Scenarious = ScenarioObject.GetComponentsInChildren<Scenario>();

        //for (int i = 0; i < Scenarious.Length; i++)
        //{
        //    Scenarious[i].Setup();
        //    AllScenarious.Add(Scenarious[i]);

        //    AddScenarioGUI(i + 1, Scenarious[i]);
        //}       
    }

    public void ResetGUIScenarios()
    {
        foreach(var S in ScenarioGUIs)
        {
            //S.UnSelectScenario();
        }
    }

    //private void AddScenarioGUI(int Index, Scenario _Scenario)
    //{
    //    var NewScenarioGUI = Instantiate(ScenarioGUIPrefab);

    //    NewScenarioGUI.transform.SetParent(ScenarioGUIsContainer);

    //    var _ScenarioGUIController = NewScenarioGUI.GetComponent<ScenarioGuiController>();

    //    ScenarioGUIs.Add(_ScenarioGUIController);

    //    _ScenarioGUIController.AssignScenario(Index, _Scenario);
    //}

    //public void AssignScenarioGameObject(GameObject _GameObject)
    //{
    //    ScenarioGameObject = _GameObject;

    //    ClearContainer(ScenarioControllersContainer);

    //    ScenariosList.Clear();

    //    var Scenarios = ScenarioGameObject.GetComponentsInChildren<Scenario>();

    //    for (int i = 0; i < Scenarios.Length; i++)
    //    {
    //        //создаем в гуи кнопку сценария
    //        var NewScenarioController = Instantiate(ScenarioControllerPrefab);
    //        NewScenarioController.transform.SetParent(ScenarioControllersContainer);

    //        var NewScenarioControllerComponent = NewScenarioController.GetComponent<ScenarioController>();
    //        NewScenarioControllerComponent.AssignScenario(Scenarios[i]);

    //        Scenarios[i].Setup();

    //        ScenariosList.Add(Scenarios[i]);

    //        //добавить разделитель
    //        if(i< Scenarios.Length - 1)
    //        {
    //            var NewScenarioSeparator = Instantiate(ScenarioSeparatorPrefab);
    //            NewScenarioSeparator.transform.SetParent(ScenarioControllersContainer);
    //        }
    //    }

    //    if (ScenariosList.Count > 0) SetScenario(ScenariosList[0]);
    //}

    //public void ClearContainer(Transform Container)
    //{
    //    var ChildCount = Container.childCount;

    //    for (int i = ChildCount - 1; i >= 0; i--)
    //    {
    //        Destroy(Container.GetChild(i).gameObject);
    //    }
    //}

    //public void PlayScenario()
    //{
    //    if (CurrentScenario != null)
    //    {
    //        if(CurrentScenario._ScenarioState == ScenarioState.PlayScenario ||
    //            CurrentScenario._ScenarioState == ScenarioState.UserScenario)
    //        {
    //            CurrentScenario.StopScenario();                
    //        }
    //        else
    //        {
    //            CurrentScenario.PlayScenario();
    //        }
    //    }
    //}

    //public void SetScenario(Scenario NewScenario)
    //{     
    //    if (CurrentScenario != null)
    //    {
    //        if (CurrentScenario != NewScenario)
    //        {
    //            CurrentScenario.Locker.SetActive(false);
    //            CurrentScenario.StopScenario();
    //            //CurrentScenario.StopAudio();
    //        }
    //    }

    //    CurrentScenario = NewScenario;

    //    _UILine.gameObject.SetActive(true);
    //    _UILineTarget._Target = CurrentScenario.transform;

    //    ScenarioNameText.text = CurrentScenario.ScenarioName;
    //    //CurrentScenario.PlayAudio();

    //    UpdateScenarioCounter(CurrentScenario);

    //    _ScenarioCard.AssignScenario(CurrentScenario);
    //    _ScenarioCard.PlayAudio();

    //    CurrentScenario.Locker.SetActive(true);
    //    //_ScenarioCard.ShowVideo();
    //}

    //public Scenario GetCurrentScenario()
    //{
    //    return CurrentScenario;
    //}

    //int ScenarioCounter = 0;

    //public void PreviousScenario()
    //{
    //    ScenarioCounter -= 1;

    //    if (ScenarioCounter < 0)
    //    {
    //        ScenarioCounter = ScenariosList.Count - 1;
    //    }

    //    SetScenario(ScenariosList[ScenarioCounter]);
    //}

    //public void NextScenario()
    //{
    //    ScenarioCounter += 1;

    //    if (ScenarioCounter > ScenariosList.Count - 1)
    //    {
    //        ScenarioCounter = 0;
    //    }

    //    SetScenario(ScenariosList[ScenarioCounter]);
    //}

    //private void UpdateScenarioCounter(Scenario _Scenario)
    //{
    //    ScenarioCounter = ScenariosList.IndexOf(_Scenario);
    //}
}
