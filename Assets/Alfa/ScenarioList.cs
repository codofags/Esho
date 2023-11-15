using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScenarioList
{
    private List<ArScenario> scenarios = new List<ArScenario>();

    public void AddScenario(ArScenario newScenario)
    {
        scenarios.Add(newScenario);
    }

    public List<ArScenario> GetArScenarios()
    {
        return scenarios;
    }
}
