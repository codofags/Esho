using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SearchKeyUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI keyText;

    public void Select()
    {
        SearchManager.ins.SelectKey(keyText.text);
    }
}
