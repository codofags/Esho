using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScenarioGuiController : MonoBehaviour
{
    public TextMeshProUGUI numberText;
    public Image image;
    public TextMeshProUGUI nameText;

    public Button buttonSelect;

    internal void AssignScenario(int number, ArScenario s)
    {
        var numberResult = number.ToString();

        if (number < 10)
        {
            numberResult = $"0 {numberResult}.";
        }

        this.numberText.text = numberResult;

        nameText.text = TranslateHelper.ins.GetText(s.translation);

        buttonSelect.onClick.AddListener(() => ShowContent(s));
    }

    public void ShowContent(ArScenario s)
    {
        var container = ContentManager.inst.pathsFullContentContainer;

        //ContentManager.inst.AddAllContent(s.point, container);

        ContentManager.inst.pathsFullContent.SetActive(true);
    }

    //public Text ScenarioNumber;
    //public Text ScenarioName;

    //public Image ScenarioBackGround;

    //public GameObject ScenarioLock;

    //public void AssignScenario(int Index, Scenario _Scenario)
    //{
    //    ScenarioBackGround.sprite = _Scenario.ScenarioGUIBackGround;
    //    ScenarioNumber.text = Index.ToString();
    //    ScenarioName.text = _Scenario.ScenarioName;
    //}

    //public void SelectScenario()
    //{
    //    //сбрасываем остальные сценарии
    //    ScenarioManager.Inst.ResetGUIScenarios();

    //    //блокируем этот сценарий
    //    ScenarioLock.SetActive(true);
    //}

    //public void UnSelectScenario()
    //{
    //    ScenarioLock.SetActive(false);
    //}
}
