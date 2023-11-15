using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    public Image bar;

    [SerializeField] private Canvas mainCanvas;
    [SerializeField] private List<Canvas> _canvases = new List<Canvas>();

    void Start()
    {
        DeviceChecker.SetupCanvas(_canvases);

        LoadScene("Main");
    }

    [SerializeField] private float loadSpeed = 0.1f;
    [SerializeField] private float loadPeriod = 1;
    float nextLoadTime = 0;

    //private void Update()
    //{
    //    if (Time.time > nextLoadTime)
    //    {
    //        nextLoadTime = Time.time + loadPeriod;

    //        var progress = bar.fillAmount + loadSpeed;
    //        bar.fillAmount = progress;
    //    }
    //}

    public void LoadScene(string SceneName)
    {
        StartCoroutine(AsyncLoadScene(SceneName));
    }

    [SerializeField] private float _targetLoadingValue = 0.13f;

    IEnumerator AsyncLoadScene(string SceneName)
    {
        yield return new WaitForSeconds(0.1f);

        AsyncOperation loading = SceneManager.LoadSceneAsync(SceneName);
        while (!loading.isDone)
        {
            float progress = loading.progress / _targetLoadingValue;
            bar.fillAmount = progress;

            yield return null;
        }
    }

}
