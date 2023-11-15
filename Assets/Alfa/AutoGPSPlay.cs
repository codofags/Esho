using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoGPSPlay : MonoBehaviour
{
    public static AutoGPSPlay ins;

    public bool useAutoGid  { get; private set; }

    private void Awake()
    {
        ins = this;

        useAutoGid = false;
    }

    public void EnableAutoGid()
    {
        useAutoGid = true;
        HideAutoPlay();
    }

    public void DisableAutoGid()
    {
        useAutoGid = false;
    }

    [SerializeField] private GameObject wi_AutoPlay;
    public void HideAutoPlay()
    {
        wi_AutoPlay.SetActive(false);
    }
}
