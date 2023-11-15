using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Search : MonoBehaviour
{
    private int minimumCharLimit = 2;

    public Transform serchResultContainer;

    public void SearchByWord(string word)
    {
        if(word.Length < minimumCharLimit)
        {
            return;
        }

        List<PathInfoUI> resultPath = new List<PathInfoUI>();
        //ищем пути по имени и описанию

        List<PoiConfig> resultPoi = new List<PoiConfig>();
        //ищем пои по имени и описанию

        //выводим результат

        //выводим пути

        //выводим пои

    }
}
