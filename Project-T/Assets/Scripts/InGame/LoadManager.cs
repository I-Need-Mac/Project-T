using BFM;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class LoadManager : SingletonBehaviour<LoadManager>
{
    public string sceneName = "Playing";

    public VisualElement _progressBar;

    private AsyncOperation operation;

    private void Awake()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        _progressBar = root.Q<VisualElement>("progressBar");

    }

    void Start()
    {
        StartCoroutine(LoadCoroutine());
    }

    IEnumerator LoadCoroutine()
    {
        operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        float timer = 0f;
        while (!operation.isDone)
        {
            yield return null;

            timer += Time.deltaTime;
            if (operation.progress < 0.9f)
            {
                float value = Mathf.Lerp(operation.progress, 1f, timer);
                _progressBar.style.width = new Length(value*100, LengthUnit.Percent);
                if (value >= operation.progress)
                    timer = 0f;
            }
            else
            {
                float value = Mathf.Lerp(_progressBar.style.width.value.value / 100f, 1f, timer);
                _progressBar.style.width = new Length(value*100, LengthUnit.Percent);
                if (_progressBar.style.width.value.value >= 99f)
                    operation.allowSceneActivation = true;
            }
        }

        gameObject.SetActive(false);
    }
}
